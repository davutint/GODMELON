using Gley.Common;

namespace Gley.Jumpy.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        internal const string menuItem = "Tools/Gley/Jumpy";
        internal const string GLEY_JUMPY = "GLEY_JUMPY";
        internal const string documentation = "https://gley.gitbook.io/mobile-tools/";
        internal const string SETTINGS = "Settings";
        internal const string gameScene = "Scenes/Game.unity";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Jumpy - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "Jumpy";

        public string parentFolder => "Gley";
    }
}