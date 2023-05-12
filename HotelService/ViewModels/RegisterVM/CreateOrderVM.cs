using HotelService.DataContext.Repositories;
using HotelService.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.RegisterVM
{
    public class CreateOrderVM : ViewModelBase
    {
        private Hotel _selectedHotel;
        private readonly ResidenceOrderRepository _residenceOrderRepository;
        private readonly ApartmentRepository _apartmentRepository;
        private readonly VisitorRepository _visitorRepository;
        private readonly ServiceRepository _serviceRepository;

        public string? VisitorId { get; set; }
        public string? ApartmentsId { get; set; }
        public string? ServicesId { get; set; }
        public DateTime SettleDate { get; set; } = DateTime.Now;
        public DateTime EvectionDate { get; set; } = DateTime.Now;

        public CreateOrderVM(Hotel selectedHotel, ResidenceOrderRepository residenceOrderRepository, ApartmentRepository apartmentRepository, VisitorRepository visitorRepository, ServiceRepository serviceRepository)
        {
            _selectedHotel = selectedHotel;
            _residenceOrderRepository = residenceOrderRepository;
            _apartmentRepository = apartmentRepository;
            _visitorRepository = visitorRepository;
            _serviceRepository = serviceRepository;
        }

        public ICommand Save => new DelegateCommand<object>(CreateOrderAsync);

        private async void CreateOrderAsync(object obj)
        {
            if(VisitorId == null || ApartmentsId == null || ServicesId == null || EvectionDate.Date == DateTime.Today)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
                return;
            }

            try
            {
                string[] apartmentsSubStrings = ApartmentsId.Split(',');
                string[] servicesSubStrings = ServicesId.Split(',');

                var order = new ResidenceOrder
                {
                    GuidId = Guid.NewGuid(),
                    Hotel = _selectedHotel,
                    Visitor = await _visitorRepository.GetVisitorByIdAsync(Convert.ToInt32(VisitorId.Trim())),
                    SettleDate = DateOnly.FromDateTime(SettleDate),
                    Evection = DateOnly.FromDateTime(EvectionDate),
                    CreateDateTime = DateTime.Now
                };
                order.Apartments = new List<Apartment>();
                foreach (string s in apartmentsSubStrings)
                {
                    order.Apartments.Add(await _apartmentRepository.GetApartmentByIdAsync(Convert.ToInt32(s.Trim())));
                }
                order.Services = new List<Service>();
                foreach(string s in servicesSubStrings)
                {
                    order.Services.Add(await _serviceRepository.GetServiceByIdAsync(Convert.ToInt32(s.Trim())));
                }

                await _residenceOrderRepository.CreateOrderAsync(order);

                MessageBox.Show("Создание прошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время сохранения: {ex.InnerException}");
            }
        }
    }
}
