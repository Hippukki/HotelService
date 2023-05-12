using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.RegisterUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.RegisterVM
{
    public class RegistersVM : ViewModelBase
    {
        private readonly ResidenceOrderRepository _residenceOrderRepository;
        private readonly ApartmentRepository _apartmentRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly VisitorRepository _visitorRepository;
        private ObservableCollection<Service>? _serviceslList;
        private ObservableCollection<ApartmentDto>? _apartmentslList;
        private ObservableCollection<ResidenceOrder>? _residenceOrders;
        private ApartmentDto? _selectedApartmnet;
        private Hotel _selectedHotel;
        private Service? _selectedService;
        private ResidenceOrder? _selectedOrder;

        public ObservableCollection<ResidenceOrder>? Orders
        {
            get => _residenceOrders;
            set
            {
                _residenceOrders = value;
                RaiseProperty();
            }
        }
        public ObservableCollection<Service>? Services
        {
            get => _serviceslList;
            set
            {
                _serviceslList = value;
                RaiseProperty();
            }
        }
        public ObservableCollection<ApartmentDto>? Apartments
        {
            get => _apartmentslList;
            set
            {
                _apartmentslList = value;
                RaiseProperty();
            }
        }
        public ApartmentDto? SelectedApartment
        {
            get => _selectedApartmnet;
            set
            {
                _selectedApartmnet = value;
                RaiseProperty();
            }
        }
        public Service? SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                RaiseProperty();
            }
        }
        public ResidenceOrder? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                RaiseProperty();
            }
        }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
        }
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По имени",
            "По фамилии",
            "По телефону"
        };
        public string? SelectedType { get; set; }

        public RegistersVM(ResidenceOrderRepository residenceOrderRepository, ApartmentRepository apartmentRepository, ServiceRepository serviceRepository, VisitorRepository visitorRepository, Hotel selectedHotel)
        {
            _residenceOrderRepository = residenceOrderRepository;
            _apartmentRepository = apartmentRepository;
            _serviceRepository = serviceRepository;
            _visitorRepository = visitorRepository;
            _selectedHotel = selectedHotel;
            LoadDataAsync();
        }

        public ICommand CreateOrder => new DelegateCommand<object>(CreateOrderAsync);
        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand EditOrder => new DelegateCommand<object>(TrySearch);
        public ICommand DeleteOrder => new DelegateCommand<object>(DeleteOrderAsync);

        private void CreateOrderAsync(object obj)
        {
            new CreateOrderWindow(new CreateOrderVM(SelectedHotel, _residenceOrderRepository, _apartmentRepository, _visitorRepository, _serviceRepository)).Show();
            return;
        }

        private async void DeleteOrderAsync(object obj)
        {
            try
            {
                var result = await _residenceOrderRepository.DeleteOrderAsync(SelectedOrder.GuidId);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить бронь, которая не существует.");
                    return;
                }

                Orders.Remove(SelectedOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении номера: {ex.Message}");
            }
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if (Orders == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По имени":
                    FilterByName();
                    break;

                case "По фамилии":
                    FilterByLastName();
                    break;

                case "По телефону":
                    FilterByPhoneNumber();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByName()
        {
            var ordersList = new ObservableCollection<ResidenceOrder>();
            foreach (var order in Orders)
            {
                if (order.Visitor.FirstName.ToLower().StartsWith(SearchData.ToLower()))
                {
                    ordersList.Add(order);
                }
            }
            Orders = ordersList;
            return;
        }

        private void FilterByLastName()
        {
            var ordersList = new ObservableCollection<ResidenceOrder>();
            foreach (var order in Orders)
            {
                if (order.Visitor.LastName.ToLower().StartsWith(SearchData.ToLower()))
                {
                    ordersList.Add(order);
                }
            }
            Orders = ordersList;
            return;
        }

        private void FilterByPhoneNumber()
        {
            var ordersList = new ObservableCollection<ResidenceOrder>();
            foreach (var order in Orders)
            {
                if (order.Visitor.PhoneNumber.ToLower().StartsWith(SearchData.ToLower()))
                {
                    ordersList.Add(order);
                }
            }
            Orders = ordersList;
            return;
        }

        private async Task LoadDataAsync()
        {
            await Task.Delay(1000);
            Orders = new(await _residenceOrderRepository.GetOrdersByHotelIdAsync(SelectedHotel.Id));
            Apartments = new();
            Services = new();
            while (true)
            {
                await Task.Delay(3000);
                if(Orders != null)
                {
                    var ordersList = await _residenceOrderRepository.GetOrdersByHotelIdAsync(SelectedHotel.Id);
                    foreach (var order in ordersList)
                    {
                        if (Orders.FirstOrDefault(o => o.GuidId == order.GuidId) == null)
                        {
                            Orders.Add(order);
                        }
                    }

                    foreach (var order in Orders)
                    {
                        decimal? totalSumm = 0;
                        foreach (var apartment in order.Apartments)
                        {
                            totalSumm += apartment.Price;
                        }
                        foreach (var service in order.Services)
                        {
                            totalSumm += service.Price;
                        }
                        var livingTime = order.Evection.Value.DayNumber - order.SettleDate.Value.DayNumber;
                        order.TotalSumm = totalSumm.Value * livingTime;
                    }

                    if (SelectedOrder != null)
                    {
                        Apartments.Clear();
                        foreach (var apartment in SelectedOrder.Apartments)
                        {
                            var categories = new Dictionary<int, string>();
                            var levels = new Dictionary<int, string>();
                            var categoriesList = Enum.GetValues(typeof(ApartmentCategory));
                            foreach (var category in categoriesList)
                            {
                                categories.Add((int)category, category.GetType()
                                        .GetMember(category.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName());
                            }
                            var levelsList = Enum.GetValues(typeof(ApartmentLevel));
                            foreach (var level in levelsList)
                            {
                                levels.Add((int)level, level.GetType()
                                    .GetMember(level.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName());
                            }

                            var apartToAdd = await _apartmentRepository.GetApartmentByIdAsync(apartment.Id);
                            var mappedApartment = new ApartmentDto
                            {
                                Id = apartToAdd.Id,
                                Stage = apartToAdd.Stage,
                                Number = apartToAdd.Number,
                                Category = (from pair in categories where pair.Key == (int)apartToAdd.Category select pair.Value).FirstOrDefault(),
                                Level = (from pair in levels where pair.Key == (int)apartToAdd.Level select pair.Value).FirstOrDefault(),
                                Price = Convert.ToInt32(apartToAdd.Price)
                            };

                            Apartments.Add(mappedApartment);
                        }

                        Services.Clear();
                        foreach (var service in SelectedOrder.Services)
                        {
                            Services.Add(await _serviceRepository.GetServiceByIdAsync(service.Id));
                        }
                    }
                    await Task.Delay(5000);
                }
            }
        }
    }
}
