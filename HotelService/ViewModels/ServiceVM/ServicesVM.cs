using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.ServiceUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.ServiceVM
{
    public class ServicesVM : ViewModelBase
    {
        private readonly ServiceRepository _serviceRepository;
        private Hotel? _selectedHotel;
        private ObservableCollection<Service>? _serviceslList;
        private Service? _selectedService;

        public ObservableCollection<Service>? Services
        {
            get => _serviceslList;
            set
            {
                _serviceslList = value;
                RaiseProperty();
            }
        }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
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
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По названию",
            "По описанию",
            "По цене"
        };
        public string? SelectedType { get; set; }

        public ServicesVM(ServiceRepository serviceRepository, Hotel selectedHotel)
        {
            _serviceRepository = serviceRepository;
            _selectedHotel = selectedHotel;
            LoadServicesAsync();
        }

        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand Edit => new DelegateCommand<object>(EditServiceAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteServiceAsync);

        private async void EditServiceAsync(object obj)
        {
            if (SelectedService == null)
            {
                MessageBox.Show("Ошибка! Невозможно редактировать пустую запись");
                return;
            }

            new EditServiceWindow(new EditServiceVM(_serviceRepository, SelectedService)).Show();
            await LoadServicesAsync();
        }

        private async void DeleteServiceAsync(object obj)
        {
            try
            {
                var result = await _serviceRepository.DeleteServiceAsync(SelectedService.Id);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить сервис, который не существует.");
                    return;
                }

                Services.Remove(SelectedService);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении сервиса: {ex.Message}");
            }
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if (Services == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По названию":
                    FilterByName();
                    break;

                case "По описанию":
                    FilterByDescription();
                    break;

                case "По цене":
                    FilterByPrice();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByName()
        {
            var servicesList = new ObservableCollection<Service>();
            foreach (var service in Services)
            {
                if (service.ServiceName.ToLower().StartsWith(SearchData.ToLower()) ||
                    service.ServiceName.ToLower().Contains(SearchData.ToLower()))
                {
                    servicesList.Add(service);
                }
            }
            Services = servicesList;
            return;
        }

        private void FilterByDescription()
        {
            var servicesList = new ObservableCollection<Service>();
            foreach (var service in Services)
            {
                if (service.Description.ToLower().StartsWith(SearchData.ToLower()) ||
                    service.Description.ToLower().Contains(SearchData.ToLower()))
                {
                    servicesList.Add(service);
                }
            }
            Services = servicesList;
            return;
        }

        private void FilterByPrice()
        {
            var servicesList = new ObservableCollection<Service>();
            foreach (var service in Services)
            {
                var parsedPrice = service.Price.ToString();
                if (parsedPrice.StartsWith(SearchData))
                {
                    servicesList.Add(service);
                }
            }
            Services = servicesList;
            return;
        }

        private async Task LoadServicesAsync()
        {
            var servicesList = await _serviceRepository.GetServisesAsync();
            Services = new(servicesList.Where(s => s.Hotels.Contains(SelectedHotel)));
        }
    }
}
