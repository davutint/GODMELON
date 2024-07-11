using Gley.Common;

namespace Gley.Notifications.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/Gley/Notifications";

        public const string GLEY_NOTIFICATIONS_ANDROID = "GLEY_NOTIFICATIONS_ANDROID";
        public const string GLEY_NOTIFICATIONS_IOS = "GLEY_NOTIFICATIONS_IOS";
        internal const string notificationExample = "Example/Scenes/NotificationsExample.unity";
        internal static string documentation= "https://gley.gitbook.io/mobile-notifications/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Notifications - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "Notifications";

        public string parentFolder => "Gley";
    }
}