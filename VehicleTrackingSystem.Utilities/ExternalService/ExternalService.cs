using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTrackingSystem.Utilities.ExternalService
{
    public class ExternalService : IExternalService
    {
        private readonly ILogger<ExternalService> _logger;

        public ExternalService(ILogger<ExternalService> logger)
        {
            _logger = logger;
        }

        public async Task<object> CallServiceAsync<T>(Method method, string url, object requestobject, bool log = false, HeaderAPI header = null) where T : class
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(method);
                if (header != null)
                    request.AddHeader(header.Key, header.Value);

                if (method == Method.POST)
                    request.AddParameter("application/json", JsonConvert.SerializeObject(requestobject), ParameterType.RequestBody);

                ServicePointManager
             .ServerCertificateValidationCallback +=
             (sender, cert, chain, sslPolicyErrors) => true;

                IRestResponse response = await client.ExecuteAsync(request);
                if (log)
                {
                    _logger.LogInformation("API Call - " + url);
                    if (requestobject != null)
                        _logger.LogInformation("API Request - " + JsonConvert.SerializeObject(requestobject));
                    _logger.LogInformation("API Response - " + JsonConvert.SerializeObject(response.Content));
                }

                var model = JsonConvert.DeserializeObject<T>(response.Content);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new object();
            }
        }
    }
}
