using CFS.Application.Application.Commands.Commands;
using CFS.Application.Application.Queries;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CFS.Client.Services
{
    public class ServiceService
    {
        private readonly string _remoteServiceBaseUrl;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(string remoteServiceBaseUrl, ILogger<ServiceService> logger)
        {
            _remoteServiceBaseUrl = $"{remoteServiceBaseUrl}/services";
            _logger = logger;
        }

        public async Task<ServiceViewModel> GetServiceAsync(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{_remoteServiceBaseUrl }/{id}");
                    return JsonConvert.DeserializeObject<ServiceViewModel>(response);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR getting service: {ServiceId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<ServiceViewModel>> GetServicesAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(_remoteServiceBaseUrl);
                    return JsonConvert.DeserializeObject<IEnumerable<ServiceViewModel>>(response);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR getting services");
                return null;
            }
        }

        public async Task<bool> CreateServiceAsync(ServiceViewModel ServiceViewModel)
        {
            var command = new CreateServiceCommand(ServiceViewModel.FacilityId, DateTime.Now, null);
            var content = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(_remoteServiceBaseUrl, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR creating service: ({@Command})", command);
                return false;
            }
        }

        public async Task<bool> UpdateServiceAsync(ServiceViewModel ServiceViewModel)
        {
            var command = new UpdateServiceCommand(ServiceViewModel.ServiceId, ServiceViewModel.FacilityId, ServiceViewModel.StartDate, ServiceViewModel.StopDate);
            var content = new StringContent(JsonConvert.SerializeObject(command), System.Text.Encoding.UTF8, "application/json");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(_remoteServiceBaseUrl, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR updating service: ({@Command})", command);
                return false;
            }
        }
    }
}
