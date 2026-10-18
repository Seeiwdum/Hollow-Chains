using UnityEngine.Localization;

namespace HollowChains.Data.Localization
{
    /// <summary>
    /// Resolves a LocalizedString for the current locale (demo: th, D-025), falling back to a dev string when no table
    /// entry is assigned. Keeps "where does this text come from" in one place for every system.
    /// </summary>
    public static class LocalizedText
    {
        public static string Resolve(LocalizedString localized, string fallback)
        {
            if (localized == null || localized.IsEmpty) return fallback;
            var value = localized.GetLocalizedString();
            return string.IsNullOrEmpty(value) ? fallback : value;
        }
    }
}
