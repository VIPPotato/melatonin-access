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
        private static MelonPreferences_Entry<bool> _announceMenuPositions;
        private static MelonPreferences_Entry<bool> _announceTutorialDialog;
        private static MelonPreferences_Entry<bool> _announceCreditsRoll;
        private static MelonPreferences_Entry<bool> _debugModeEnabled;

        /// <summary>
        /// Whether map hotspot arrival and teleport destination lines are spoken.
        /// </summary>
        public static bool AnnounceMapHotspots => _announceMapHotspots == null || _announceMapHotspots.Value;

        /// <summary>
        /// Whether gameplay rhythm cue prompts are spoken.
        /// </summary>
        public static bool AnnounceRhythmCues => _announceRhythmCues == null || _announceRhythmCues.Value;

        /// <summary>
        /// Whether menu position context (e.g., "1 of 4") is spoken.
        /// </summary>
        public static bool AnnounceMenuPositions => _announceMenuPositions == null || _announceMenuPositions.Value;

        /// <summary>
        /// Whether tutorial and dialog text is spoken.
        /// </summary>
        public static bool AnnounceTutorialDialog => _announceTutorialDialog == null || _announceTutorialDialog.Value;

        /// <summary>
        /// Whether credits title and scrolling names are spoken.
        /// </summary>
        public static bool AnnounceCreditsRoll => _announceCreditsRoll == null || _announceCreditsRoll.Value;

        /// <summary>
        /// Whether debug logging is enabled.
        /// </summary>
        public static bool DebugModeEnabled => _debugModeEnabled != null && _debugModeEnabled.Value;

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

            _announceMenuPositions = _category.CreateEntry(
                "AnnounceMenuPositions",
                true,
                description: "Speak menu positions such as '1 of 4'.");

            _announceTutorialDialog = _category.CreateEntry(
                "AnnounceTutorialDialog",
                true,
                description: "Speak tutorial and dialog text.");

            _announceCreditsRoll = _category.CreateEntry(
                "AnnounceCreditsRoll",
                true,
                description: "Speak credits title and scrolling credits entries.");

            _debugModeEnabled = _category.CreateEntry(
                "DebugModeEnabled",
                false,
                description: "Enable [SR]/[INPUT]/[STATE]/[HANDLER]/[GAME] debug logging.");

            MelonPreferences.Save();
        }

        /// <summary>
        /// Toggles rhythm cue announcements and saves immediately.
        /// </summary>
        public static bool ToggleRhythmCues()
        {
            if (_announceRhythmCues == null) return true;

            _announceRhythmCues.Value = !_announceRhythmCues.Value;
            MelonPreferences.Save();
            return _announceRhythmCues.Value;
        }

        /// <summary>
        /// Toggles menu position announcements and saves immediately.
        /// </summary>
        public static bool ToggleMenuPositions()
        {
            if (_announceMenuPositions == null) return true;

            _announceMenuPositions.Value = !_announceMenuPositions.Value;
            MelonPreferences.Save();
            return _announceMenuPositions.Value;
        }

        /// <summary>
        /// Toggles debug mode and saves immediately.
        /// </summary>
        public static bool ToggleDebugMode()
        {
            if (_debugModeEnabled == null) return false;

            _debugModeEnabled.Value = !_debugModeEnabled.Value;
            MelonPreferences.Save();
            return _debugModeEnabled.Value;
        }
    }
}
