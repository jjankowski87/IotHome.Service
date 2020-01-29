using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IotHome.Service.Model;

namespace IotHome.Service.Services.Interfaces
{
    public interface IStorageExplorer
    {
        Task<IEnumerable<IotMessage<Reading>>> ListSensorDetailsAsync(DateTimeOffset @from, DateTimeOffset to);
    }
}