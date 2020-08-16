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
    public class CustomerService
    {
        private readonly string _remoteServiceBaseUrl;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(string remoteServiceBaseUrl, ILogger<CustomerService> logger)
        {
            _remoteServiceBaseUrl = $"{remoteServiceBaseUrl}/customers";
            _logger = logger;
        }

        public async Task<CustomerViewModel> GetCustomerAsync(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync($"{_remoteServiceBaseUrl }/{id}");
                    return JsonConvert.DeserializeObject<CustomerViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR getting customer: {CustomerId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomersAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync(_remoteServiceBaseUrl);
                    return JsonConvert.DeserializeObject<IEnumerable<CustomerViewModel>>(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- ERROR getting customers");
                return null;
            }
        }

        public async Task<bool> CreateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var command = new CreateCustomerCommand(customerViewModel.FirstName, customerViewModel.LastName, customerViewModel.PhoneNumber, customerViewModel.Email, new Address());
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
                _logger.LogError(ex, "ERROR creating customer: ({@Command})", command);
                return false;
            }
        }

        public async Task<bool> UpdateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var command = new UpdateCustomerCommand(customerViewModel.CustomerId, customerViewModel.FirstName, customerViewModel.LastName, customerViewModel.PhoneNumber, customerViewModel.Email, new Address());
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
                _logger.LogError(ex, "ERROR updating customer: ({@Command})", command);
                return false;
            }
        }
    }
}
