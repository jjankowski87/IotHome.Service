using System;
using System.Threading.Tasks;
using IotHome.Service.Model;
using Microsoft.Extensions.Logging;

namespace IotHome.Service.ReadingsMonitor
{
    public interface IAppNotifier
    {
        Task NotifyNewReadingAsync(DateTimeOffset date, Reading reading, ILogger logger);
    }
}