using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Universal.DateTime.DateTimeProvider
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public System.DateTime Now => System.DateTime.Now;
        public System.DateTime Today => System.DateTime.Today;
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}
