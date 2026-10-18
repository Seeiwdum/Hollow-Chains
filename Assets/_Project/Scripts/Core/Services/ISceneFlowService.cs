using System;

namespace HollowChains.Core.Services
{
    /// <summary>
    /// Loads content scenes (S01_Bedroom, S02_Astraon…) additively on top of the persistent _Boot scene.
    /// Only one content scene is active at a time; loading a new one unloads the previous one.
    /// </summary>
    public interface ISceneFlowService : IService
    {
        bool IsLoading { get; }

        string CurrentScene { get; }

        /// <summary>Raised after a content scene finished loading and became the active scene.</summary>
        event Action<string> SceneLoaded;

        void Load(string sceneName);

        /// <summary>Fades out and unloads the current content scene (back to the title over _Boot).</summary>
        void UnloadCurrent();
    }
}
