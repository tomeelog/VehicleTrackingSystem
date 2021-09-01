using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTrackingSystem.Utilities.ExternalService
{
    public interface IExternalService
    {
        Task<object> CallServiceAsync<T>(Method method, string url, object requestobject, bool log = false, HeaderAPI header = null) where T : class;
    }
}
