using System.Linq;
using System.Threading.Tasks;
using IotHome.Service.App.Configuration;
using IotHome.Service.App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IotHome.Service.App.Controllers
{
    [Route("api")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private readonly AppConfiguration _configuration;
        private readonly IReadingsService _readingsService;

        public ReadingsController(AppConfiguration configuration, IReadingsService readingsService)
        {
            _configuration = configuration;
            _readingsService = readingsService;
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
            var latestReadings = await _readingsService.GetLatestReadingsAsync();
            var matching = latestReadings.FirstOrDefault(r => r.Key.Type == sensorType);

            return matching.Value?.Value;
        }
    }
}