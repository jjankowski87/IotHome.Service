using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IotHome.Service.Model;

namespace IotHome.Service.App.Services.Interfaces
{
    public interface IReadingsService
    {
        Task<IDictionary<SensorDetails, SensorReading>> GetLatestReadingsAsync();

        Task<IDictionary<SensorDetails, IList<SensorReading>>> GetReadingsAsync(DateTime fromDate, DateTime toDate);
    }
}
