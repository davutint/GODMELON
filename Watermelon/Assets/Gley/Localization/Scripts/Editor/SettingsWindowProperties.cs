using Gley.Common;

namespace Gley.Localization.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/Gley/Localization";

        public const string GLEY_TMPRO_LOCALIZATION = "GLEY_TMPRO_LOCALIZATION";
        public const string GLEY_NGUI_LOCALIZATION = "GLEY_NGUI_LOCALIZATION";
        public const string GLEY_LOCALIZATION = "GLEY_LOCALIZATION";
        internal const string componentExample= "Example/Scenes/ComponentExample.unity";
        internal const string localizationExample= "Example/Scenes/LocalizationExample.unity";
        internal const string documentation= "https://gley.gitbook.io/localization/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Localization - v.";

        public int minWidth =>520;

        public int minHeight => 520;

        public string folderName => "Localization";

        public string parentFolder => "Gley";
    }
}