using HotelService.DataContext.Repositories;
using HotelService.Models;
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
    public class CreateServiceVM : ViewModelBase
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly HotelRepository _hotelRepository;
        private Service? _selectedService;

        public ObservableCollection<Hotel>? Hotels { get; set; }
        public Hotel? SelectedHotel { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public Service? SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                RaiseProperty();
            }
        }
        public ObservableCollection<Service> CreatedServices { get; set; }

        public CreateServiceVM(ServiceRepository serviceRepository, HotelRepository hotelRepository)
        {
            _serviceRepository = serviceRepository;
            CreatedServices = new();
            _hotelRepository = hotelRepository;
            FillPropertiesAsync();
        }

        public ICommand Create => new DelegateCommand<object>(CreateServiceAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteServiceAsync);

        private async void CreateServiceAsync(object obj)
        {
            if (ServiceName == null || Description == null || Price == null || SelectedHotel == null)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
                return;
            }

            try
            {
                var parsedPrice = Convert.ToDecimal(Price);
                var service = new Service()
                {
                    ServiceName = ServiceName,
                    Description = Description,
                    Price = parsedPrice,
                    CreateDateTime = DateTime.Now,
                };
                service.Hotels = new List<Hotel>
                {
                    SelectedHotel
                };
                await _serviceRepository.CreateServiceAsync(service);
                CreatedServices.Add(service);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время сохранения сервиса: {ex.Message}");
            }
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

                CreatedServices.Remove(SelectedService);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении сервиса: {ex.Message}");
            }
        }

        private async Task FillPropertiesAsync()
        {
            await Task.Delay(1000);
            Hotels = new(await _hotelRepository.GetListHotelsAsync());
            while (true)
            {
                var hotelsList = await _hotelRepository.GetListHotelsAsync();
                foreach (var hotel in hotelsList)
                {
                    if (Hotels.FirstOrDefault(h => h.Id == hotel.Id) == null)
                    {
                        Hotels.Add(hotel);
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}
