using System;
using System.Linq;
using System.Threading;

namespace TaskManager.Universal.DateTime
{
    public class TimeZoneHelper
    {
        private static readonly Lazy<TimeZoneInfo> _moscowTimeZone = new Lazy<TimeZoneInfo>(GetMoscowTimeZoneInternal, LazyThreadSafetyMode.ExecutionAndPublication);

        public static TimeZoneInfo GetMoscowTimeZone()
        {
            return _moscowTimeZone.Value;
        }

        private static TimeZoneInfo GetMoscowTimeZoneInternal()
        {
            return TimeZoneInfo
                .GetSystemTimeZones()
                .First(IsMockowTimezone);
        }

        private static bool IsMockowTimezone(TimeZoneInfo t)
        {
            var lower = t.DisplayName.ToLower();
            var containsMoscow = lower.Contains("moscow") || lower.Contains("москва");
            var offsetIsValid = t.BaseUtcOffset == TimeSpan.FromHours(3);
            return offsetIsValid && containsMoscow;
        }
    }
}
