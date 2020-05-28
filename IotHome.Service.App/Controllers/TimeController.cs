using IotHome.Service.App.Configuration;
using IotHome.Service.App.Utils;
using Microsoft.AspNetCore.Mvc;

namespace IotHome.Service.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : ControllerBase
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TimeController(IDateTimeProvider dateTimeProvider, AppConfiguration appConfiguration)
        {
            _dateTimeProvider = dateTimeProvider;
            _appConfiguration = appConfiguration;
        }

        [HttpGet("next-reading")]
        public ActionResult<int> Get()
        {
            var now = _dateTimeProvider.UtcNow;
            var samplingSeconds = _appConfiguration.ReadingsSamplingMinutes * 60;
            var currentSeconds = now.Minute * 60 + now.Second;

            return samplingSeconds - (currentSeconds % samplingSeconds);
        }
    }
}