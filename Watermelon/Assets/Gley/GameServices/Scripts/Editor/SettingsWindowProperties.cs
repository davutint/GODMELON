using Gley.Common;

namespace Gley.GameServices.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/Gley/Game Services";
        public const string GLEY_GAMESERVICES_ANDROID = "GLEY_GAMESERVICES_ANDROID";
        public const string GLEY_GAMESERVICES_IOS = "GLEY_GAMESERVICES_IOS";
        internal static object gameServicesExample = "Example/Scenes/GameServicesExample.unity";
        internal static object gameServicesTest = "Example/Scenes/GameServicesTest.unity";
        internal static string documentation= "https://gley.gitbook.io/easy-achievements/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Game Services - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "GameServices";

        public string parentFolder => "Gley";
    }
}