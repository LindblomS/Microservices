using CFS.Application.Application.Queries;
using CFS.Client.Models;
using CFS.Client.Services;
using CFS.Client.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace CFS.Client.ViewModels
{
    public class CreateOrUpdateCustomerViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private CustomerViewModel _customer;
        private CreateOrUpdateCustomerWindow _window;

        public CreateOrUpdateCustomerViewModel(CustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            CreateOrUpdateCommand = new RelayCommand(execute => CreateOrUpdate());
            CloseCommand = new RelayCommand(execute => Close());
            _customer = new CustomerViewModel();
            _window = new CreateOrUpdateCustomerWindow();
            _window.DataContext = this;
            _window.Show();
        }

        public ICommand CreateOrUpdateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public CustomerViewModel Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged();
            }
        }
        public string ErrorMessage { get; set; }

        private async void CreateOrUpdate()
        {
            if (_customer.CustomerId > 0)
            {
                ErrorMessage = await _customerService.UpdateCustomerAsync(_customer)
                ? "Customer updated"
                : "Error updating customer";
            }
            else
            {
                ErrorMessage = await _customerService.CreateCustomerAsync(_customer)
                ? "Customer created"
                : "Error creating customer";
            }
                
            NotifyPropertyChanged(nameof(ErrorMessage));
        }

        private void Close()
        {
            _window.Close();
        }
    }

}
