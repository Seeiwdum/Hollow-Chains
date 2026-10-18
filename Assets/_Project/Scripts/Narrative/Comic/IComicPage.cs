using System;

namespace HollowChains.Narrative.Comic
{
    /// <summary>
    /// One page of the intro comic. S00 now uses <see cref="SubtitleComicPage"/> (black screen + narration); P7 adds a
    /// panel page (Timeline + sprites) that plugs into the same <see cref="ComicPlayer"/> without changing it (`06` SC-00).
    /// </summary>
    public interface IComicPage
    {
        void Play(Action onFinished);

        void Skip();
    }
}
