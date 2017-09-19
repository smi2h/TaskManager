using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Universal.DateTime.DateTimeProvider
{
    public static class DateTimeGetter
    {
        private static IDateTimeProvider _dateTimeProvider;

        public static void SetProvider(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public static System.DateTime Now => _dateTimeProvider.Now;
        public static System.DateTime NowUtc => _dateTimeProvider.Now.ToUniversalTime();
        public static System.DateTime Today => _dateTimeProvider.Today;
    }
}
