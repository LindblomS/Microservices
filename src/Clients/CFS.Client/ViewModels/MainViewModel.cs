using CFS.Application.Application.Queries;
using CFS.Client.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace CFS.Client.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<(CustomerViewModel customer, List<FacilityViewModel> facilities, List<ServiceViewModel> services)> _data;
        private CustomerViewModel _currentCustomer;

        public MainViewModel()
        {
            _data = new List<(CustomerViewModel customer, List<FacilityViewModel> facilities, List<ServiceViewModel> services)>();
            _data = GetCustomers();

            CurrentCustomer = _data[0].customer;
            NotifyPropertyChanged(nameof(CurrentCustomer));

            FindCommand = new RelayCommand(p => Find());
            PreviousCommand = new RelayCommand(p => this.Previous());
            NextCommand = new RelayCommand(p => Next());
        }

        public ICommand FindCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }

        public CustomerViewModel CurrentCustomer
        { 
            get { return _currentCustomer; }
            set
            {
                _currentCustomer = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Facilites));
                NotifyPropertyChanged(nameof(Services));
            }
        }

        public List<FacilityViewModel> Facilites
        {
            get { return _data.Where(c => c.customer.CustomerId == CurrentCustomer.CustomerId).FirstOrDefault().facilities; }
        }

        public List<ServiceViewModel> Services
        {
            get { return _data.Where(c => c.customer.CustomerId == CurrentCustomer.CustomerId).FirstOrDefault().services; }
        }

        public void Find()
        {

        }

        public void Previous()
        {
            var current = _data.Where(c => c.customer == CurrentCustomer).FirstOrDefault();
            var oldIndex = _data.IndexOf(current);
            CurrentCustomer = _data.ElementAtOrDefault(oldIndex - 1).customer ?? CurrentCustomer;
        }

        public void Next()
        {
            var current = _data.Where(c => c.customer == CurrentCustomer).FirstOrDefault();
            var oldIndex = _data.IndexOf(current);
            CurrentCustomer = _data.ElementAtOrDefault(oldIndex + 1).customer ?? CurrentCustomer;
        }

        public List<(CustomerViewModel customer, List<FacilityViewModel> facilities, List<ServiceViewModel> services)> GetCustomers()
        {
            return new List<(CustomerViewModel customer, List<FacilityViewModel> facilities, List<ServiceViewModel> services)>
            {
                (new CustomerViewModel{ CustomerId = 1, FirstName = "Stan", LastName = "Bengtsson" }, 
                new List<FacilityViewModel> 
                { new FacilityViewModel { FacilityId = 1, FacilityName = "asd" } }, 
                new List<ServiceViewModel> { new ServiceViewModel { ServiceId = 1, ServiceName = "asd" } }),

                (new CustomerViewModel{ CustomerId = 2, FirstName = "Lisa", LastName = "HejOchHå" },
                new List<FacilityViewModel>
                { new FacilityViewModel { FacilityId = 2, FacilityName = "asdf" } },
                new List<ServiceViewModel> { new ServiceViewModel { ServiceId = 2, ServiceName = "asd" } }),

                (new CustomerViewModel{ CustomerId = 3, FirstName = "Olof", LastName = "Bryggebygd" },
                new List<FacilityViewModel>
                { new FacilityViewModel { FacilityId = 3, FacilityName = "asdf" } },
                new List<ServiceViewModel> { new ServiceViewModel { ServiceId = 3, ServiceName = "asd" } })
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
