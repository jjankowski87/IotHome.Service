using System;

namespace IotHome.Service.Model
{
    public class IotMessage<TBody>
    {
        public DateTimeOffset EnqueuedTimeUtc { get; set; }

        public SystemProperties SystemProperties { get; set; }

        public TBody Body { get; set; }
    }
}