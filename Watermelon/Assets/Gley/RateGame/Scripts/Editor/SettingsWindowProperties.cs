using Gley.Common;

namespace Gley.RateGame.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/Gley/Rate Game";

        public const string GLEY_NATIVE_GOOGLEPLAY = "GLEY_NATIVE_GOOGLEPLAY";
        public const string GLEY_NATIVE_APPSTORE = "GLEY_NATIVE_APPSTORE";
        internal const string documentation= "https://gley.gitbook.io/rate-game/";
        internal const string exampleScene = "Example/Scenes/RateGameExample.unity";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Rate Game - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "RateGame";

        public string parentFolder => "Gley";
    }
}