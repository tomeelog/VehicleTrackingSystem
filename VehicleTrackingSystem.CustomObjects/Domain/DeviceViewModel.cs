using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleTrackingSystem.CustomObjects.Domain
{
    public class DeviceViewModel
    {
        public long DeviceId { get; set; }
        public Guid ImeiNumber { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
