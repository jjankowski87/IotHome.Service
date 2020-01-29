using System.Collections.Generic;
using System.Threading.Tasks;
using IotHome.Service.Model;
using IotHome.Service.Services.Models;

namespace IotHome.Service.Services.Interfaces
{
    public interface IStorageHelper
    {
        Task<IEnumerable<DateDirectory>> ListDateDirectoriesAsync();

        Task<IEnumerable<TimeBlob>> ListTimeBlobsAsync(DateDirectory directory);

        Task<IEnumerable<IotMessage<Reading>>> ListMessagesAsync(TimeBlob blob);

        Task<IEnumerable<IotMessage<Reading>>> ListMessagesAsync(string path);
    }
}