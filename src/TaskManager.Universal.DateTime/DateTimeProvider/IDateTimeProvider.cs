using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Universal.DateTime.DateTimeProvider
{
    public interface IDateTimeProvider
    {
        System.DateTime Now { get; }
        System.DateTime Today { get; }
        System.DateTime UtcNow { get; }
    }
}
