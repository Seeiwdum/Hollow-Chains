using System.Collections.Generic;
using HollowChains.Core.Events;
using HollowChains.Data.Dialogue;
using HollowChains.Narrative.Cutscenes;
using HollowChains.UI.Thai;
using TMPro;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;
using G = HollowChains.Editor.Greybox.GreyboxKit;

namespace HollowChains.Editor.Cutscenes
{
    /// <summary>
    /// Builds the greybox climax (SC-04 water magic → hug → bite, SC-05 Awakening, SC-06 rejection → father's message →
    /// zoom out → demo end) into S04_StreamCliff as one Timeline with three interactive beats. Re-runnable: deletes and
    /// rebuilds its own objects. Menu: Hollow Chains/Build SC-04..06 Cutscene. Beat timings are in <see cref="T"/>.
    /// </summary>
    public static class ClimaxCutsceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/S04_StreamCliff.unity";
        private const string TimelinePath = "Assets/_Project/Timeline/TL_SC04_06_Climax.playable";
        private const string RootName = "Cutscene_SC04_06 (built)";

        /// <summary>Key times (seconds) — tweak here and rebuild.</summary>
        private static class T
        {
            public const float WaterStart = 0.6f, WaterHit = 2.4f, WaterGone = 3.2f;
            public const float HugBeat = 4.0f, HugArrive = 6.6f, Bite = 6.8f;
            public const float AwakenStart = 7.6f, Tick = 8.2f, RootsBeat = 9.0f, RootsOut = 9.8f, SoulDrain = 10.0f;
            public const float Flash1 = 11.0f, Flash2 = 11.6f, Flash3 = 12.2f, AwakeningEnded = 13.0f;
            public const float ReachBeat = 14.5f, Elena1 = 15.6f, Elena2 = 18.8f, Elena3 = 22.0f, RunAway = 25.4f;
            public const float Ichor = 27.6f, Father = 28.6f, LastTick = 31.4f, ZoomOut = 31.8f, DemoEnd = 36.2f, End = 36.6f;
        }

        [MenuItem("Hollow Chains/Build SC-04..06 Cutscene")]
        public static void BuildMenu()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Build();
            EditorSceneManager.SaveScene(scene);
        }

        /// <summary>Builds into the currently open S04 scene (loads all assets after the scene is open).</summary>
        public static string Build()
        {
            var old = GameObject.Find(RootName);
            if (old != null) Object.DestroyImmediate(old);

            var ev = new Events();
            var lines = CreateLines();
            var root = new GameObject(RootName).transform;

            // --- actors & placeholder VFX
            var elena = GameObject.Find("Elena (cornered)").transform;
            var wolf = GameObject.Find("DarkWolf (placeholder)").transform;
            var actorsParent = G.Group("Actors", root).transform;
            var phineas = G.Capsule("Phineas_Actor", actorsParent, new Vector3(0, 0, -14f), 1.65f, 0, G.Mat("M_Capsule_Phineas", new Color(0.18f, 0.18f, 0.22f)));
            Object.DestroyImmediate(phineas.GetComponent<Collider>());
            phineas.SetActive(false);
            var hand = GameObject.CreatePrimitive(PrimitiveType.Cube); hand.name = "Phineas_Hand";
            Object.DestroyImmediate(hand.GetComponent<Collider>());
            hand.transform.SetParent(actorsParent, false); hand.transform.localScale = new Vector3(0.12f, 0.12f, 0.3f);
            hand.GetComponent<MeshRenderer>().sharedMaterial = phineas.GetComponent<MeshRenderer>().sharedMaterial; hand.SetActive(false);
            var water = GameObject.CreatePrimitive(PrimitiveType.Capsule); water.name = "VFX_WaterMagic (placeholder)";
            Object.DestroyImmediate(water.GetComponent<Collider>());
            water.transform.SetParent(root, false); water.GetComponent<MeshRenderer>().sharedMaterial = G.Water; water.SetActive(false);
            var rootsParent = G.Group("VFX_Roots (placeholder)", root).transform;
            var rootPivots = new List<Transform>();
            Vector3 armPos = new Vector3(0.25f, 1.05f, 8.0f);
            Vector3[] targets = { new Vector3(0, 1.4f, 4.2f), new Vector3(0.6f, 1.9f, 4.8f), new Vector3(-0.5f, 1.0f, 3.6f), new Vector3(0.4f, 0.8f, 2.6f), new Vector3(-0.3f, 2.1f, 5.4f) };
            var hollow = G.Hollow;
            for (int i = 0; i < targets.Length; i++)
            {
                var pivot = new GameObject("RootPivot_" + i).transform;
                pivot.SetParent(rootsParent, false);
                pivot.position = armPos;
                pivot.rotation = Quaternion.LookRotation(targets[i] - armPos);
                float len = Vector3.Distance(armPos, targets[i]);
                var cyl = GameObject.CreatePrimitive(PrimitiveType.Cylinder); cyl.name = "Root";
                Object.DestroyImmediate(cyl.GetComponent<Collider>());
                cyl.transform.SetParent(pivot, false);
                cyl.transform.localRotation = Quaternion.Euler(90, 0, 0);
                cyl.transform.localPosition = new Vector3(0, 0, len * 0.5f);
                cyl.transform.localScale = new Vector3(0.07f, len * 0.5f, 0.07f);
                cyl.GetComponent<MeshRenderer>().sharedMaterial = hollow;
                pivot.localScale = new Vector3(1, 1, 0.01f);
                pivot.gameObject.SetActive(false);
                rootPivots.Add(pivot);
            }
            var drop = GameObject.CreatePrimitive(PrimitiveType.Sphere); drop.name = "VFX_IchorDrop";
            Object.DestroyImmediate(drop.GetComponent<Collider>());
            drop.transform.SetParent(root, false); drop.transform.localScale = Vector3.one * 0.06f;
            drop.GetComponent<MeshRenderer>().sharedMaterial = hollow; drop.SetActive(false);
            var message = new GameObject("FatherMessage (ichor letters)");
            message.transform.SetParent(root, false);
            message.transform.SetPositionAndRotation(new Vector3(0.1f, 0.03f, 7.3f), Quaternion.Euler(90, 180, 0));
            var tmp = message.AddComponent<TextMeshPro>();
            tmp.font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/_Project/UI/Fonts/Mali-Regular SDF.asset");
            tmp.fontSize = 3.2f; tmp.alignment = TextAlignmentOptions.Center; tmp.color = new Color(0.16f, 0.07f, 0.2f);
            tmp.rectTransform.sizeDelta = new Vector2(5, 1.2f);
            tmp.text = "จงหนีไป... ผนึกกำลังจะแตกแล้ว"; // CANON (`06` SC-06) — the same string is in the Dialogue table
            message.AddComponent<ThaiMarkPositioner>();
            message.SetActive(false);

            // --- volume & audio
            var volGo = new GameObject("Volume_Awakening");
            volGo.transform.SetParent(root, false);
            var vol = volGo.AddComponent<Volume>(); vol.isGlobal = true; vol.priority = 40; vol.weight = 0f; vol.sharedProfile = AwakeningProfile();
            var audioGo = new GameObject("CutsceneAudio"); audioGo.transform.SetParent(root, false);
            var audio = audioGo.AddComponent<AudioSource>(); audio.playOnAwake = false; audio.spatialBlend = 0f;
            AudioClip Clip(string n) => AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/ThirdParty/Audio/Kenney_RPGAudio/" + n + ".ogg");

            // --- cameras
            var camsParent = G.Group("Cameras", root).transform;
            var cArrive = Cam("CM_Arrive", camsParent, new Vector3(7, 3.2f, -9), elena);
            var cHug = Cam("CM_Hug", camsParent, new Vector3(-4.5f, 2.2f, 3.5f), phineas.transform);
            var cAwaken = Cam("CM_Awaken", camsParent, new Vector3(2.6f, 1.5f, 6.6f), phineas.transform);
            var cReach = Cam("CM_Reach", camsParent, new Vector3(0.7f, 1.75f, 6.6f), elena);
            var cIchor = Cam("CM_Ichor", camsParent, new Vector3(1.0f, 1.4f, 6.4f), message.transform);
            var cZoom = Cam("CM_ZoomOut", camsParent, new Vector3(0, 4f, 3.5f), phineas.transform);

            // --- director + timeline
            var dirGo = new GameObject("Director");
            dirGo.transform.SetParent(root, false);
            var director = dirGo.AddComponent<PlayableDirector>();
            director.playOnAwake = false;
            G.EnsureFolder("Assets/_Project/Timeline");
            AssetDatabase.DeleteAsset(TimelinePath);
            var tl = ScriptableObject.CreateInstance<TimelineAsset>();
            AssetDatabase.CreateAsset(tl, TimelinePath);
            director.playableAsset = tl;

            var events = tl.CreateTrack<ChannelEventTrack>(null, "Story Events");
            Event(events, ev.CutsceneStarted, 0.0f, "Cutscene started");
            Event(events, ev.Bitten, T.Bite, "Wolf bites (HP = 1)");
            Event(events, ev.SoulDrain, T.SoulDrain, "Soul Drain");
            Event(events, ev.Flash, T.Flash1, "Flashback 1"); Event(events, ev.Flash, T.Flash2, "Flashback 2"); Event(events, ev.Flash, T.Flash3, "Flashback 3");
            Event(events, ev.AwakeningEnded, T.AwakeningEnded, "Awakening ended (Q001-P4)");
            Event(events, ev.DemoEnded, T.DemoEnd, "Demo end");

            var beats = tl.CreateTrack<InteractiveBeatTrack>(null, "Player Beats");
            Beat(beats, T.HugBeat, BeatKind.Press, "beat_hug", "วิ่งเข้าไปกอด");
            Beat(beats, T.RootsBeat, BeatKind.Hold, "beat_roots", "กดค้างเพื่อปล่อยราก");
            Beat(beats, T.ReachBeat, BeatKind.Press, "beat_reach", "ยื่นมือ");

            var subs = tl.CreateTrack<LineTrack>(null, "Subtitles");
            Line(subs, lines["SC04_Elena_Cry"], ev.PlayLine, 0.4f);
            Line(subs, lines["SC06_Elena_1"], ev.PlayLine, T.Elena1);
            Line(subs, lines["SC06_Elena_2"], ev.PlayLine, T.Elena2);
            Line(subs, lines["SC06_Elena_3"], ev.PlayLine, T.Elena3);
            Line(subs, lines["SC06_Father"], ev.PlayLine, T.Father);

            var camTrack = tl.CreateTrack<CinemachineTrack>(null, "Cameras");
            Shot(camTrack, director, cArrive, 0f, T.HugBeat);
            Shot(camTrack, director, cHug, T.HugBeat, T.AwakenStart);
            Shot(camTrack, director, cAwaken, T.AwakenStart, T.ReachBeat - 0.5f);
            Shot(camTrack, director, cReach, T.ReachBeat - 0.5f, T.Ichor);
            Shot(camTrack, director, cIchor, T.Ichor, T.ZoomOut);
            Shot(camTrack, director, cZoom, T.ZoomOut, T.End);

            Activate(tl, director, phineas, 0f, T.End);
            Activate(tl, director, water, T.WaterStart, T.WaterGone);
            Activate(tl, director, hand, T.ReachBeat, T.Elena2);
            Activate(tl, director, drop, T.Ichor, T.Father);
            Activate(tl, director, message, T.Father - 0.2f, T.End);
            foreach (var p in rootPivots) Activate(tl, director, p.gameObject, T.RootsBeat, T.AwakeningEnded + 0.8f);

            // Elena: cry/cornered → knocked by the wolf → recoils from Phineas → runs away
            var e = Tween(tl, director, elena, "Elena");
            Rot(e, T.WaterGone, 0.4f, new Vector3(0, 180, 0), new Vector3(-28, 180, 0));
            Move(e, T.Elena1, 0.5f, new Vector3(0, 0.775f, 9f), new Vector3(0, 0.775f, 9.8f));
            Rot(e, T.Elena1, 0.4f, new Vector3(-28, 180, 0), new Vector3(0, 180, 0));
            Move(e, T.RunAway, 2.6f, new Vector3(0, 0.775f, 9.8f), new Vector3(-15f, 0.775f, -26f));

            // Water magic: from Elena's hands to the wolf, then swallowed by the black mist
            var w = Tween(tl, director, water.transform, "Water magic");
            var wc = Move(w, T.WaterStart, T.WaterHit - T.WaterStart, new Vector3(0, 1.1f, 8.5f), new Vector3(0, 1.4f, 5.4f));
            wc.Rotation = true; wc.FromEuler = wc.ToEuler = new Vector3(90, 0, 0);
            wc.Scale = true; wc.FromScale = new Vector3(0.18f, 0.25f, 0.18f); wc.ToScale = new Vector3(0.45f, 0.7f, 0.45f);
            Scale(w, T.WaterHit, T.WaterGone - T.WaterHit, new Vector3(0.45f, 0.7f, 0.45f), new Vector3(0.01f, 0.01f, 0.01f));

            // Wolf: advances on Elena, lunges at Phineas, withers into a mummy after the Soul Drain
            var wf = Tween(tl, director, wolf, "Dark Wolf");
            Move(wf, T.WaterGone, 0.6f, new Vector3(0, 1.3f, 3.5f), new Vector3(0, 1.3f, 5.6f));
            Move(wf, T.HugArrive, 0.25f, new Vector3(0, 1.3f, 5.6f), new Vector3(0, 1.3f, 6.2f));
            var wither = Move(wf, T.SoulDrain, 1.8f, new Vector3(0, 1.3f, 6.2f), new Vector3(0, 0.45f, 6.0f));
            wither.Scale = true; wither.FromScale = Vector3.one; wither.ToScale = new Vector3(0.75f, 0.38f, 0.85f);

            // Phineas actor: runs to shield Elena, collapses from the bite, kneels after the flashback
            var ph = Tween(tl, director, phineas.transform, "Phineas");
            Move(ph, T.HugBeat, T.HugArrive - T.HugBeat, new Vector3(0, 0.825f, -14f), new Vector3(0, 0.825f, 8.15f));
            var crouch = Move(ph, T.Bite, 0.4f, new Vector3(0, 0.825f, 8.15f), new Vector3(0, 0.62f, 8.15f));
            crouch.Scale = true; crouch.FromScale = new Vector3(0.594f, 0.825f, 0.594f); crouch.ToScale = new Vector3(0.594f, 0.62f, 0.594f);
            var kneel = Move(ph, T.Flash3, 0.6f, new Vector3(0, 0.62f, 8.15f), new Vector3(0, 0.5f, 8.1f));
            kneel.Scale = true; kneel.FromScale = crouch.ToScale; kneel.ToScale = new Vector3(0.594f, 0.5f, 0.594f);
            var stand = Move(ph, T.ReachBeat - 0.6f, 0.6f, kneel.ToPosition, new Vector3(0, 0.825f, 8.1f));
            stand.Scale = true; stand.FromScale = kneel.ToScale; stand.ToScale = new Vector3(0.594f, 0.825f, 0.594f);

            var hd = Tween(tl, director, hand.transform, "Reaching hand");
            Move(hd, T.ReachBeat, 0.8f, new Vector3(0.2f, 1.0f, 8.4f), new Vector3(0.1f, 1.05f, 9.0f));
            Move(hd, T.Elena1 + 0.2f, 0.25f, new Vector3(0.1f, 1.05f, 9.0f), new Vector3(0.35f, 0.9f, 8.5f)); // slapped away

            for (int i = 0; i < rootPivots.Count; i++)
            {
                var rt = Tween(tl, director, rootPivots[i], "Root " + i);
                Scale(rt, T.RootsBeat + i * 0.06f, T.RootsOut - T.RootsBeat, new Vector3(1, 1, 0.01f), Vector3.one);
                Scale(rt, T.AwakeningEnded, 0.8f, Vector3.one, new Vector3(1, 1, 0.01f));
            }
            var dp = Tween(tl, director, drop.transform, "Ichor drop");
            Move(dp, T.Ichor, 0.9f, new Vector3(0.3f, 1.0f, 8.0f), new Vector3(0.2f, 0.03f, 7.6f)).Ease = false;

            Move(Tween(tl, director, cZoom.transform, "Zoom-out camera"), T.ZoomOut, T.End - T.ZoomOut, new Vector3(0, 4f, 3.5f), new Vector3(0, 46f, -12f));

            var vt = tl.CreateTrack<VolumeWeightTrack>(null, "Awakening look");
            director.SetGenericBinding(vt, vol);
            VolumeClip(vt, T.AwakenStart, 1.4f, 0f, 1f);
            VolumeClip(vt, T.AwakeningEnded, 1.0f, 1f, 0.35f);
            VolumeClip(vt, T.ZoomOut, T.End - T.ZoomOut, 0.35f, 1f);

            var at = tl.CreateTrack<AudioTrack>(null, "SFX");
            director.SetGenericBinding(at, audio);
            AudioAt(at, Clip("metalClick"), T.Tick, "Watch — first tick ever (D-032)");
            AudioAt(at, Clip("creak1"), T.Tick + 0.3f, "Chain drag (placeholder)");
            AudioAt(at, Clip("chop"), T.RootsOut - 0.6f, "Roots burst (placeholder)");
            AudioAt(at, Clip("creak3"), T.RootsOut, "Chain (placeholder)");
            AudioAt(at, Clip("metalClick"), T.LastTick, "Single tick in the silence (option C)");

            // runtime components
            var player = dirGo.AddComponent<CutscenePlayer>();
            Set(player, "_playOn", ev.ArenaReached); Set(player, "_onStarted", null); Set(player, "_onFinished", null);
            var runner = dirGo.AddComponent<InteractiveBeatRunner>();
            Set(runner, "_interact", InteractRef());
            Set(runner, "_onPrompt", ev.Prompt);

            EditorUtility.SetDirty(tl);
            AssetDatabase.SaveAssets();
            return "built: tracks=" + tl.outputTrackCount + " duration=" + tl.duration.ToString("F1") + "s";
        }

        // ---------- helpers ----------
        private sealed class Events
        {
            public readonly VoidEventChannelSO CutsceneStarted = V("EVT_CutsceneStarted"), Bitten = V("EVT_PlayerBitten"), SoulDrain = V("EVT_SoulDrain"),
                Flash = V("EVT_Flash"), AwakeningEnded = V("EVT_AwakeningEnded"), DemoEnded = V("EVT_DemoEnded"), ArenaReached = V("EVT_ArenaReached");
            public readonly StringEventChannelSO Prompt = AssetDatabase.LoadAssetAtPath<StringEventChannelSO>("Assets/_Project/Data/Events/EVT_InteractPrompt.asset");
            public readonly DialogueLineEventChannelSO PlayLine = AssetDatabase.LoadAssetAtPath<DialogueLineEventChannelSO>("Assets/_Project/Data/Events/EVT_PlayLine.asset");

            private static VoidEventChannelSO V(string n)
            {
                string path = "Assets/_Project/Data/Events/" + n + ".asset";
                var a = AssetDatabase.LoadAssetAtPath<VoidEventChannelSO>(path);
                if (a == null) { a = ScriptableObject.CreateInstance<VoidEventChannelSO>(); AssetDatabase.CreateAsset(a, path); }
                return a;
            }
        }

        private static Dictionary<string, DialogueLineSO> CreateLines()
        {
            var dlg = LocalizationEditorSettings.GetStringTableCollection("Dialogue");
            var ui = LocalizationEditorSettings.GetStringTableCollection("UI");
            void Entry(StringTableCollection c, string key, string th, string en)
            {
                if (c.SharedData.GetEntry(key) == null) c.SharedData.AddKey(key);
                ((StringTable)c.GetTable("th")).AddEntry(key, th);
                ((StringTable)c.GetTable("en")).AddEntry(key, en);
                EditorUtility.SetDirty(c.GetTable("th")); EditorUtility.SetDirty(c.GetTable("en")); EditorUtility.SetDirty(c.SharedData);
            }
            Entry(dlg, "speaker_elena", "เอเลนา", "Elena");
            Entry(ui, "beat_hug", "วิ่งเข้าไปกอด", "Run and shield her");
            Entry(ui, "beat_roots", "กดค้างเพื่อปล่อยราก", "Hold to release the roots");
            Entry(ui, "beat_roots_press", "กดเพื่อปล่อยราก", "Press to release the roots");
            Entry(ui, "beat_reach", "ยื่นมือ", "Reach out");

            // [PROPOSAL] team line (D-035) + CANON lines copied verbatim from 06 SC-06 (split only at the original punctuation)
            var spec = new (string id, string th, string en, bool elena, bool thought)[]
            {
                ("SC04_Elena_Cry", "อย่าเข้ามานะ!", "Stay back!", true, false),
                ("SC06_Elena_1", "อย่าเข้ามาใกล้ฉัน! พลังดูดวิญญาณนั่น...", "Don't come near me! That soul-draining power...", true, false),
                ("SC06_Elena_2", "แกมันก็สัตว์ประหลาดเหมือนพ่อของแก!", "You're a monster, just like your father!", true, false),
                ("SC06_Elena_3", "ที่ผ่านมาแกแกล้งทำเป็นคนดีเพื่อจะฆ่าฉันใช่ไหม?!", "All this time you pretended to be kind so you could kill me, didn't you?!", true, false),
                ("SC06_Father", "จงหนีไป... ผนึกกำลังจะแตกแล้ว", "Run... the seal is about to break.", false, true),
            };
            G.EnsureFolder("Assets/_Project/Data/Dialogue/S04");
            var result = new Dictionary<string, DialogueLineSO>();
            foreach (var s in spec)
            {
                Entry(dlg, s.id, s.th, s.en);
                string path = "Assets/_Project/Data/Dialogue/S04/DL_" + s.id + ".asset";
                var line = AssetDatabase.LoadAssetAtPath<DialogueLineSO>(path);
                if (line == null) { line = ScriptableObject.CreateInstance<DialogueLineSO>(); AssetDatabase.CreateAsset(line, path); }
                var so = new SerializedObject(line);
                so.FindProperty("_id").stringValue = s.id;
                so.FindProperty("_text").stringValue = s.th;
                so.FindProperty("_speakerName").stringValue = s.elena ? "เอเลนา" : string.Empty;
                so.FindProperty("_isThought").boolValue = s.thought;
                so.FindProperty("_duration").floatValue = 3.0f;
                so.FindProperty("_localizedText").boxedValue = new LocalizedString("Dialogue", s.id);
                so.FindProperty("_localizedSpeaker").boxedValue = s.elena ? new LocalizedString("Dialogue", "speaker_elena") : new LocalizedString();
                so.ApplyModifiedPropertiesWithoutUndo();
                result[s.id] = line;
            }
            return result;
        }

        private static VolumeProfile AwakeningProfile()
        {
            const string path = "Assets/_Project/Settings/Volumes/VP_Awakening.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile != null) return profile;
            profile = ScriptableObject.CreateInstance<VolumeProfile>();
            AssetDatabase.CreateAsset(profile, path);
            var ca = profile.Add<ChromaticAberration>(true); ca.intensity.Override(0.85f);
            var ld = profile.Add<LensDistortion>(true); ld.intensity.Override(-0.35f);
            var col = profile.Add<ColorAdjustments>(true); col.saturation.Override(-85f); col.contrast.Override(25f);
            var vig = profile.Add<Vignette>(true); vig.intensity.Override(0.45f); vig.color.Override(new Color(0.16f, 0.09f, 0.22f));
            foreach (var c in profile.components) AssetDatabase.AddObjectToAsset(c, profile);
            return profile;
        }

        private static CinemachineCamera Cam(string name, Transform parent, Vector3 pos, Transform lookAt)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.LookAt(lookAt.position + Vector3.up * 0.6f);
            var cam = go.AddComponent<CinemachineCamera>();
            cam.Priority = -10; // only the Timeline drives these
            cam.Target.LookAtTarget = lookAt;
            var lens = cam.Lens; lens.FieldOfView = 45f; lens.NearClipPlane = 0.1f; cam.Lens = lens;
            go.AddComponent<CinemachineHardLookAt>();
            return cam;
        }

        private static void Event(ChannelEventTrack track, VoidEventChannelSO channel, float at, string label)
        {
            var clip = track.CreateClip<ChannelEventClip>();
            clip.start = at; clip.duration = 0.1; clip.displayName = label;
            ((ChannelEventClip)clip.asset).Channel = channel;
        }

        private static void Beat(InteractiveBeatTrack track, float at, BeatKind kind, string key, string fallback)
        {
            var clip = track.CreateClip<InteractiveBeatClip>();
            clip.start = at; clip.duration = 0.1; clip.displayName = kind + ": " + fallback;
            var beat = (InteractiveBeatClip)clip.asset;
            beat.Kind = kind; beat.HoldSeconds = 1.5f; beat.Prompt = new LocalizedString("UI", key); beat.PromptFallback = fallback;
            if (kind == BeatKind.Hold) beat.PressPrompt = new LocalizedString("UI", key + "_press"); // "press instead of hold" option
        }

        private static void Line(LineTrack track, DialogueLineSO line, DialogueLineEventChannelSO channel, float at)
        {
            var clip = track.CreateClip<LineClip>();
            clip.start = at; clip.duration = line.Duration; clip.displayName = line.Id;
            var lc = (LineClip)clip.asset; lc.Line = line; lc.PlayLine = channel;
        }

        private static void Shot(CinemachineTrack track, PlayableDirector director, CinemachineCamera cam, float from, float to)
        {
            var clip = track.CreateClip<CinemachineShot>();
            clip.start = from; clip.duration = to - from; clip.displayName = cam.name;
            var shot = (CinemachineShot)clip.asset;
            shot.VirtualCamera.exposedName = GUID.Generate().ToString();
            director.SetReferenceValue(shot.VirtualCamera.exposedName, cam);
        }

        private static void Activate(TimelineAsset tl, PlayableDirector director, GameObject go, float from, float to)
        {
            var track = tl.CreateTrack<ActivationTrack>(null, "Show " + go.name);
            director.SetGenericBinding(track, go);
            var clip = track.CreateDefaultClip();
            clip.start = from; clip.duration = to - from;
            track.postPlaybackState = ActivationTrack.PostPlaybackState.LeaveAsIs;
        }

        private static TransformTweenTrack Tween(TimelineAsset tl, PlayableDirector director, Transform target, string name)
        {
            var track = tl.CreateTrack<TransformTweenTrack>(null, name);
            director.SetGenericBinding(track, target);
            return track;
        }

        private static TransformTweenClip Move(TransformTweenTrack track, float at, float duration, Vector3 from, Vector3 to)
        {
            var clip = track.CreateClip<TransformTweenClip>();
            clip.start = at; clip.duration = Mathf.Max(0.05f, duration);
            var t = (TransformTweenClip)clip.asset;
            t.Position = true; t.FromPosition = from; t.ToPosition = to;
            return t;
        }

        private static void Rot(TransformTweenTrack track, float at, float duration, Vector3 from, Vector3 to)
        {
            var clip = track.CreateClip<TransformTweenClip>();
            clip.start = at; clip.duration = duration;
            var t = (TransformTweenClip)clip.asset;
            t.Position = false; t.Rotation = true; t.FromEuler = from; t.ToEuler = to;
        }

        private static void Scale(TransformTweenTrack track, float at, float duration, Vector3 from, Vector3 to)
        {
            var clip = track.CreateClip<TransformTweenClip>();
            clip.start = at; clip.duration = duration;
            var t = (TransformTweenClip)clip.asset;
            t.Position = false; t.Scale = true; t.FromScale = from; t.ToScale = to;
        }

        private static void VolumeClip(VolumeWeightTrack track, float at, float duration, float from, float to)
        {
            var clip = track.CreateClip<VolumeWeightClip>();
            clip.start = at; clip.duration = duration;
            var v = (VolumeWeightClip)clip.asset; v.From = from; v.To = to;
        }

        private static void AudioAt(AudioTrack track, AudioClip audio, float at, string label)
        {
            if (audio == null) return;
            var clip = track.CreateClip(audio);
            clip.start = at; clip.displayName = label;
        }

        private static InputActionReference InteractRef()
        {
            foreach (var o in AssetDatabase.LoadAllAssetsAtPath("Assets/_Project/Settings/Input/InputSystem_Actions.inputactions"))
                if (o is InputActionReference r && r.action != null && r.action.actionMap.name == "Player" && r.action.name == "Interact") return r;
            return null;
        }

        private static void Set(Object target, string prop, Object value)
        {
            var so = new SerializedObject(target);
            so.FindProperty(prop).objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
