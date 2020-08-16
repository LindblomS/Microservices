using CFS.Application.Application.Queries;
using CFS.Client.Models;
using CFS.Client.Services;
using CFS.Client.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace CFS.Client.ViewModels
{
    public class CreateOrUpdateFacilityViewModel : BaseViewModel
    {
        private readonly FacilityService _facilityService;
        private FacilityViewModel _facility;
        private CreateOrUpdateFacilityWindow _window;

        public CreateOrUpdateFacilityViewModel(FacilityService facilityService)
        {
            _facilityService = facilityService ?? throw new ArgumentNullException(nameof(facilityService));
            CreateOrUpdateCommand = new RelayCommand(execute => CreateOrUpdate());
            CloseCommand = new RelayCommand(execute => Close());
            _facility = new FacilityViewModel();
            _window = new CreateOrUpdateFacilityWindow();
            _window.DataContext = this;
            _window.Show();
        }

        public ICommand CreateOrUpdateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public FacilityViewModel Facility
        {
            get { return _facility; }
            set
            {
                _facility = value;
                NotifyPropertyChanged();
            }
        }
        public string ErrorMessage { get; set; }

        private async void CreateOrUpdate()
        {
            if (_facility.FacilityId > 0)
            {
                ErrorMessage = await _facilityService.UpdateFacilityAsync(_facility)
                ? "Facility updated"
                : "Error updating facility";
            }
            else
            {
                ErrorMessage = await _facilityService.CreateFacilityAsync(_facility)
                ? "Facility created"
                : "Error creating facility";
            }

            NotifyPropertyChanged(nameof(ErrorMessage));
        }

        private void Close()
        {
            _window.Close();
        }
    }
}
