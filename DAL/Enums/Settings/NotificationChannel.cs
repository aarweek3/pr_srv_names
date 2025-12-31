using System;

namespace DAL.Enums.Settings;

[Flags]
public enum NotificationChannel
{
    None = 0,
    Email = 1,
    InApp = 2,
    Sms = 4,
    Push = 8
}
