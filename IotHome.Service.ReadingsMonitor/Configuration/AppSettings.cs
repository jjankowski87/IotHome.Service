﻿using System;

namespace IotHome.Service.ReadingsMonitor.Configuration
{
    public class AppSettings
    {
        public Uri ServiceBaseAddress { get; set; }

        public string StorageContainerName { get; set; }

        public string StorageDirectory { get; set; }

        public string BlobStorageConnectionString { get; set; }
    }
}
