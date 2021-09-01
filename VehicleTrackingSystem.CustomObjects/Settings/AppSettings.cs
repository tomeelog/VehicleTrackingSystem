﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTrackingSystem.CustomObjects.Settings
{
    public class AppSettings
    {
        public string CacheConnectionString { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public bool InMemoryDatabase { get; set; }
        public string Version { get; set; }
        public string Secret { get; set; }
        public string ApiKey { get; set; }
        public string LocationUrl { get; set; }
    }

    public class ConnectionStrings
    {
        public Database SqlServer { get; set; }
        public Database MySql { get; set; }
        public Database Oracle { get; set; }
    }
    public class Database
    {
        public string Queries { get; set; }
        public string Commands { get; set; }
    }
}
