using System;
using System.Linq;
using System.Threading.Tasks;
using IotHome.Service.App.Configuration;
using IotHome.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IotHome.Service.App.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private readonly AppConfiguration _configuration;
        private readonly IStorageExplorer _storageExplorer;

        public ReadingsController(AppConfiguration configuration, IStorageExplorer storageExplorer)
        {
            _configuration = configuration;
            _storageExplorer = storageExplorer;
        }

        [HttpGet("{sensorType}")]
        public async Task<ActionResult> Get(string sensorType, [FromQuery]string secret)
        {
            if (secret != _configuration.Secret)
            {
                return BadRequest();
            }

            var latest = await GetLatestAsync(sensorType);
            if (latest == null)
            {
                return NotFound();
            }

            return Ok(latest);
        }

        private async Task<decimal?> GetLatestAsync(string sensorType)
        {
            var details = await _storageExplorer.ListSensorDetailsAsync(DateTimeOffset.Now.AddMinutes(-15), DateTimeOffset.Now);
            var latest = details.Where(d => d.Body.Sensor == sensorType).OrderByDescending(d => d.EnqueuedTimeUtc).FirstOrDefault();

            return latest?.Body.Value;
        }
    }
}