using System;

namespace IotHome.Service.App.Utils
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }

        DateTimeOffset UtcNow { get; }
    }
}
