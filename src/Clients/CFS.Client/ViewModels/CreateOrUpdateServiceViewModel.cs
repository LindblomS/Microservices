using CFS.Application.Application.Queries;
using CFS.Client.Models;
using CFS.Client.Services;
using CFS.Client.Views;
using System;
using System.Windows.Input;

namespace CFS.Client.ViewModels
{
    public class CreateOrUpdateServiceViewModel : BaseViewModel
    {
        private readonly ServiceService _serviceService;
        private ServiceViewModel _service;
        private CreateOrUpdateServiceWindow _window;

        public CreateOrUpdateServiceViewModel(ServiceService serviceService)
        {
            _serviceService = serviceService ?? throw new ArgumentNullException(nameof(serviceService));
            CreateOrUpdateCommand = new RelayCommand(execute => CreateOrUpdate());
            CloseCommand = new RelayCommand(execute => Close());
            _service = new ServiceViewModel();
            _window = new CreateOrUpdateServiceWindow();
            _window.DataContext = this;
            _window.Show();
        }

        public ICommand CreateOrUpdateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ServiceViewModel Service
        {
            get { return _service; }
            set
            {
                _service = value;
                NotifyPropertyChanged();
            }
        }
        public string ErrorMessage { get; set; }

        private async void CreateOrUpdate()
        {

            if (_service.ServiceId > 0)
            {
                ErrorMessage = await _serviceService.UpdateServiceAsync(_service)
                ? "Service updated"
                : "Error updating service";
            }
            else
            {
                ErrorMessage = await _serviceService.CreateServiceAsync(_service)
                ? "Service created"
                : "Error creating service";
            }

            NotifyPropertyChanged(nameof(ErrorMessage));
        }

        private void Close()
        {
            _window.Close();
        }
    }
}
