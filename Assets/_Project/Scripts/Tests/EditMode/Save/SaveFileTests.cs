using System.IO;
using HollowChains.Gameplay.Save;
using HollowChains.Gameplay.Stats;
using NUnit.Framework;

namespace HollowChains.Tests.Save
{
    public class SaveFileTests
    {
        private string _folder;

        [SetUp]
        public void SetUp() => _folder = Path.Combine(Path.GetTempPath(), "HollowChainsTests_" + System.Guid.NewGuid().ToString("N"));

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_folder)) Directory.Delete(_folder, true);
        }

        [Test]
        public void WriteThenRead_RoundTrips()
        {
            var file = new SaveFile(_folder);
            var data = new SaveData { CheckpointId = "CP_02", ResumeScene = "S03_Forest" };
            data.Entries.Add(new SaveEntry { Key = "player", Json = "{\"Health\":7}" });

            file.Write(data);
            var read = file.Read();

            Assert.IsTrue(file.Exists);
            Assert.AreEqual("S03_Forest", read.ResumeScene);
            Assert.AreEqual("{\"Health\":7}", read.Find("player"));
            Assert.IsNull(read.Find("missing"));
        }

        [Test]
        public void SecondWrite_ReplacesFirst_AndLeavesNoTempFile()
        {
            var file = new SaveFile(_folder);
            file.Write(new SaveData { CheckpointId = "CP_01" });
            file.Write(new SaveData { CheckpointId = "CP_02" });

            Assert.AreEqual("CP_02", file.Read().CheckpointId);
            Assert.AreEqual(0, Directory.GetFiles(_folder, "*.tmp").Length);
        }

        [Test]
        public void CorruptOrOldVersion_ReadsAsNoSave()
        {
            var file = new SaveFile(_folder);
            File.WriteAllText(Path.Combine(_folder, "save_0.json"), "{ not json");
            Assert.IsNull(file.Read());

            File.WriteAllText(Path.Combine(_folder, "save_0.json"), "{\"SaveVersion\":999}");
            Assert.IsNull(file.Read());
        }

        [Test]
        public void Humanity_IsClampedBetweenZeroAndHundred()
        {
            var h = new HumanityModel();
            h.Change(-30);
            Assert.AreEqual(70, h.Value);
            h.Change(-500);
            Assert.AreEqual(0, h.Value);
            h.Change(500);
            Assert.AreEqual(100, h.Value);
        }
    }
}
