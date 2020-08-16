using CFS.Application.Application.Commands.Commands;
using CFS.Application.Application.Models;
using CFS.Application.Application.Queries;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace CFS.Client.Services
{
    public class FacilityService
    {
        private readonly string _remoteServiceBaseUrl;
        private readonly ILogger<FacilityService> _logger;

        public FacilityService(string remoteServiceBaseUrl, ILogger<FacilityService> logger)
        {
            _remoteServiceBaseUrl = $"{remoteServiceBaseUrl}/facilites";
            _logger = logger;
        }

        public async Task<FacilityViewModel> GetFacilityAsync(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{_remoteServiceBaseUrl }/{id}");
                    return JsonConvert.DeserializeObject<FacilityViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR Getting Facility: {FacilityId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<FacilityViewModel>> GetFacilitesAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(_remoteServiceBaseUrl);
                    return JsonConvert.DeserializeObject<IEnumerable<FacilityViewModel>>(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR Getting Facility");
                return null;
            }
        }

        public async Task<bool> CreateFacilityAsync(FacilityViewModel facilityViewModel)
        {
            var command = new CreateFacilityCommand(facilityViewModel.CustomerId, facilityViewModel.FacilityName, new Address());
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
                _logger.LogError(ex, "ERROR creating facility: ({@Command})", command);
                return false;
            }
        }

        public async Task<bool> UpdateFacilityAsync(FacilityViewModel facilityViewModel)
        {
            var command = new UpdateFacilityCommand(facilityViewModel.FacilityId, facilityViewModel.CustomerId, facilityViewModel.FacilityName, new Address());
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
                _logger.LogError(ex, "ERROR updating facility: ({@Command})", command);
                return false;
            }
        }
    }
}
