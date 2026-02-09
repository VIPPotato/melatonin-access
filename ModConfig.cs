using MelonLoader;

namespace MelatoninAccess
{
    /// <summary>
    /// Runtime configuration for optional announcement groups.
    /// Preferences are stored in UserData/MelonPreferences.cfg.
    /// </summary>
    public static class ModConfig
    {
        private static MelonPreferences_Category _category;
        private static MelonPreferences_Entry<bool> _announceMapHotspots;
        private static MelonPreferences_Entry<bool> _announceRhythmCues;
        private static MelonPreferences_Entry<bool> _announceTutorialDialog;
        private static MelonPreferences_Entry<bool> _announceCreditsRoll;

        /// <summary>
        /// Whether map hotspot arrival and teleport destination lines are spoken.
        /// </summary>
        public static bool AnnounceMapHotspots => _announceMapHotspots == null || _announceMapHotspots.Value;

        /// <summary>
        /// Whether gameplay rhythm cue prompts are spoken.
        /// </summary>
        public static bool AnnounceRhythmCues => _announceRhythmCues == null || _announceRhythmCues.Value;

        /// <summary>
        /// Whether tutorial and dialog text is spoken.
        /// </summary>
        public static bool AnnounceTutorialDialog => _announceTutorialDialog == null || _announceTutorialDialog.Value;

        /// <summary>
        /// Whether credits title and scrolling names are spoken.
        /// </summary>
        public static bool AnnounceCreditsRoll => _announceCreditsRoll == null || _announceCreditsRoll.Value;

        /// <summary>
        /// Initializes configuration entries. Call once at startup.
        /// </summary>
        public static void Initialize()
        {
            if (_category != null) return;

            _category = MelonPreferences.CreateCategory("MelatoninAccess", "Melatonin Access");

            _announceMapHotspots = _category.CreateEntry(
                "AnnounceMapHotspots",
                true,
                description: "Speak map hotspot arrivals and teleport destination/star lines.");

            _announceRhythmCues = _category.CreateEntry(
                "AnnounceRhythmCues",
                true,
                description: "Speak gameplay rhythm cues (Space/Left/Right/Both/Hold).");

            _announceTutorialDialog = _category.CreateEntry(
                "AnnounceTutorialDialog",
                true,
                description: "Speak tutorial and dialog text.");

            _announceCreditsRoll = _category.CreateEntry(
                "AnnounceCreditsRoll",
                true,
                description: "Speak credits title and scrolling credits entries.");

            MelonPreferences.Save();
        }
    }
}
