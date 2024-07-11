using Gley.Common;

namespace Gley.DailyRewards.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        internal const string menuItem = "Tools/Gley/Daily Rewards";

        internal const string GLEY_DAILY_REWARDS = "GLEY_DAILY_REWARDS";
        internal const string documentation = "https://gley.gitbook.io/daily-rewards/";
        internal const string timerButtonExample = "Example/Scenes/TimerButtonExample.unity";
        internal const string calendarExample = "Example/Scenes/CalendarExample.unity";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Daily Rewards - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "DailyRewards";

        public string parentFolder => "Gley";
    }
}