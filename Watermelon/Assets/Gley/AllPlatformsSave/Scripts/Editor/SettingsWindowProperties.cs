using Gley.Common;

namespace Gley.AllPlatformsSave.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/Gley/All Platforms Save";
        public const string testScene = "Example/Scenes/AllPlatformsSaveExample.unity";
        public const string documentation = "https://gley.gitbook.io/all-platforms-save/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "All Platforms Save - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "AllPlatformsSave";

        public string parentFolder => "Gley";
    }
}
