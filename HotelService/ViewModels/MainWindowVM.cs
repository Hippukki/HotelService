using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.ViewModels.ApartmentVM;
using HotelService.ViewModels.HotelVM;
using HotelService.ViewModels.RegisterVM;
using HotelService.ViewModels.ReviewVM;
using HotelService.ViewModels.ServiceVM;
using HotelService.ViewModels.VisitorVM;
using HotelService.Views.ApartmentUI;
using HotelService.Views.HotelUI;
using HotelService.Views.RegisterUI;
using HotelService.Views.ReviewUI;
using HotelService.Views.ServiceUI;
using HotelService.Views.VisitorUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelService.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly HotelRepository _hotelRepository;
        private readonly ApartmentRepository _apartmentRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly VisitorRepository _visitorRepository;
        private readonly ReviewRepository _reviewRepository;
        private readonly PassportRepository _passportRepository;
        private readonly ResidenceOrderRepository _residenceOrderRepository;
        private Hotel? _selectedHotel;
        private Page? _currentPage;

        public ObservableCollection<Hotel>? Hotels { get; set; }
        public Page? CurrentPage 
        { 
            get => _currentPage; 
            set 
            { 
                _currentPage = value; 
                RaiseProperty(); 
            } 
        }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value;
                RaiseProperty();
            }
        }

        public MainWindowVM(HotelRepository hotelRepository, ApartmentRepository apartmentRepository, ServiceRepository serviceRepository,
            VisitorRepository visitorRepository, ReviewRepository reviewRepository, PassportRepository passportRepository, ResidenceOrderRepository residenceOrderRepository)
        {
            _hotelRepository = hotelRepository;
            _apartmentRepository = apartmentRepository;
            _serviceRepository = serviceRepository;
            _visitorRepository = visitorRepository;
            _reviewRepository = reviewRepository;
            _passportRepository = passportRepository;
            _residenceOrderRepository = residenceOrderRepository;
            FillHotelsAsync();
        }

        public ICommand OpenRegisterPage => new DelegateCommand<object>(LoadRegisterPage);
        public ICommand OpenApartmentsPage => new DelegateCommand<object>(LoadApartmentsPage);
        public ICommand OpenVisitorsPage => new DelegateCommand<object>(LoadVisitorsPage);
        public ICommand OpenServicesPage => new DelegateCommand<object>(LoadServicesPage);
        public ICommand OpenReviewsPage => new DelegateCommand<object>(LoadReviewsPage);
        public ICommand OpenHotelsPage => new DelegateCommand<object>(LoadHotelsPage);

        private void LoadHotelsPage(object obj)
        {
            CurrentPage = new HotelsPage(new HotelsVM(_hotelRepository));
            return;
        }

        private void LoadReviewsPage(object obj)
        {
            if (SelectedHotel != null)
            {
                CurrentPage = new ReviewsPage(new ReviewsVM(_reviewRepository, SelectedHotel));
                return;
            }

            MessageBox.Show("Ошибка! Не выбран отель.");
        }

        private void LoadServicesPage(object obj)
        {
            if(SelectedHotel != null)
            {
                CurrentPage = new ServicesPage(new ServicesVM(_serviceRepository, SelectedHotel));
                return;
            }

            MessageBox.Show("Ошибка! Не выбран отель.");
        }

        private void LoadVisitorsPage(object obj)
        {
            CurrentPage = new VisitorsPage(new VisitorsVM(_visitorRepository, _passportRepository));
            return; ;
        }

        private void LoadApartmentsPage(object obj)
        {
            if(SelectedHotel != null)
            {
                CurrentPage = new ApartmentsPage(new ApartmentsVM(_apartmentRepository, SelectedHotel));
                return;
            }

            MessageBox.Show("Ошибка! Не выбран отель.");
        }

        private void LoadRegisterPage(object obj)
        {
            if(SelectedHotel != null)
            {
                CurrentPage = new RegisterPage(new RegistersVM(_residenceOrderRepository, _apartmentRepository, _serviceRepository, _visitorRepository, SelectedHotel));
                return;
            }

            MessageBox.Show("Ошибка! Не выбран отель.");
        }

        private async Task FillHotelsAsync()
        {
            Hotels = new(await _hotelRepository.GetListHotelsAsync());
            while (true)
            {
                var hotelsList = await _hotelRepository.GetListHotelsAsync();
                foreach(var hotel in hotelsList)
                {
                    if(Hotels.FirstOrDefault(h => h.Id == hotel.Id) == null)
                    {
                        Hotels.Add(hotel);
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}
