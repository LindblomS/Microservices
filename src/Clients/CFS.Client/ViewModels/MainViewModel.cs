using CFS.Application.Application.Queries;
using CFS.Client.Models;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using CFS.Client.Services;
using System.Threading.Tasks;

namespace CFS.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly CustomerService _customerService;
        private readonly FacilityService _facilityService;
        private readonly ServiceService _serviceService;
        private CustomerViewModel _currentCustomer;
        private List<CFSViewModel> _customers;

        public MainViewModel(CustomerService customerService, FacilityService facilityService, ServiceService serviceService)
        {
            _customerService = customerService;
            _facilityService = facilityService;
            _serviceService = serviceService;
            _customers = new List<CFSViewModel>();

            FindCommand = new RelayCommand(p => Find());
            PreviousCommand = new RelayCommand(p => this.Previous());
            NextCommand = new RelayCommand(p => Next());
            ReloadDataCommand = new RelayCommand(execute => ReloadData());

            CreateCustomerCommand = new RelayCommand(
                execute => new CreateOrUpdateCustomerViewModel(_customerService));
            UpdateCustomerCommand = new RelayCommand(
                execute => UpdateCustomer(), canExecute => _currentCustomer != null);
            CreateFacilityCommand = new RelayCommand(
                execute => new CreateOrUpdateFacilityViewModel(_facilityService));
            UpdateFacilityCommand = new RelayCommand(
                execute => UpdateFacility(), canExecute => SelectedFacility != null);
            CreateServiceCommand = new RelayCommand(
                execute => new CreateOrUpdateServiceViewModel(_serviceService));
            UpdateServiceCommand = new RelayCommand(
                execute => UpdateService(), canExecute => SelectedService != null);

            GetCustomers();
        }

        public ICommand FindCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand ReloadDataCommand { get; set; }
        public ICommand CreateCustomerCommand { get; }
        public ICommand UpdateCustomerCommand { get; }
        public ICommand CreateFacilityCommand { get; }
        public ICommand UpdateFacilityCommand { get; }
        public ICommand CreateServiceCommand { get; }
        public ICommand UpdateServiceCommand { get; }

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

        public FacilityViewModel SelectedFacility { get; set; }
        public ServiceViewModel SelectedService { get; set; }

        public List<FacilityViewModel> Facilites
        {
            get { return _customers.SingleOrDefault(c => c.Customer.CustomerId == _currentCustomer?.CustomerId)?.Facilites; }
        }

        public List<ServiceViewModel> Services
        {
            get { return _customers.SingleOrDefault(c => c.Customer.CustomerId == _currentCustomer?.CustomerId)?.Services; }
        }

        private void Find()
        {

        }

        private void Previous()
        {
            var current = _customers.Where(c => c.Customer.CustomerId == _currentCustomer?.CustomerId).FirstOrDefault();
            var indexOfCurrent = _customers.IndexOf(current);
            CurrentCustomer = _customers.ElementAtOrDefault(indexOfCurrent - 1)?.Customer ?? _customers.LastOrDefault()?.Customer;
        }

        private void Next()
        {
            var current = _customers.Where(c => c.Customer.CustomerId == _currentCustomer?.CustomerId).FirstOrDefault();
            var indexOfCurrent = _customers.IndexOf(current);
            CurrentCustomer = _customers.ElementAtOrDefault(indexOfCurrent + 1)?.Customer ?? _customers.FirstOrDefault()?.Customer;
        }

        private void UpdateCustomer()
        {
            var vm = new CreateOrUpdateCustomerViewModel(_customerService);
            vm.Customer = _currentCustomer;
        }

        private void UpdateFacility()
        {
            var vm = new CreateOrUpdateFacilityViewModel(_facilityService);
            vm.Facility = SelectedFacility;
        }

        private void UpdateService()
        {
            var vm = new CreateOrUpdateServiceViewModel(_serviceService);
            vm.Service = SelectedService;
        }

        private async void GetCustomers()
        {
            var GetCustomersTask = _customerService.GetCustomersAsync();
            var GetFacilitiesTask = _facilityService.GetFacilitesAsync();
            var GetServicesTask = _serviceService.GetServicesAsync();

            var customers = await GetCustomersTask;
            var facilites = await GetFacilitiesTask;
            var services = await GetServicesTask;

            if (customers != null)
            {
                Parallel.ForEach(customers, (customer) =>
                {
                    _customers.Add(new CFSViewModel
                    {
                        Customer = customer,
                        Facilites = facilites.Where(f => f.CustomerId == customer.CustomerId).ToList(),
                        Services = services.Where(s => facilites.Any(f => f.CustomerId == customer.CustomerId && f.FacilityId == s.FacilityId)).ToList()
                    });
                });

                _currentCustomer = _customers.First().Customer;
            }
        }
        private void ReloadData()
        {
            _customers.Clear();
            GetCustomers();
        }
    }
}
