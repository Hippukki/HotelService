using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.HotelUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.HotelVM
{
    public class HotelsVM : ViewModelBase
    {
        private readonly HotelRepository _hotelRepository;
        private ObservableCollection<Hotel>? _hotelsList;
        private Hotel? _selectedHotel;

        public ObservableCollection<Hotel>? Hotels
        {
            get => _hotelsList;
            set
            {
                _hotelsList = value;
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
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По названию",
            "По адресу",
            "По к-ву этажей"
        };
        public string? SelectedType { get; set; }

        public HotelsVM(HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
            LoadHotelsAsync();
        }

        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand Edit => new DelegateCommand<object>(EditHotelAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteHotelAsync);

        private async Task LoadHotelsAsync()
        {
            Hotels = new(await _hotelRepository.GetListHotelsAsync());
        }

        private async void EditHotelAsync(object obj)
        {
            if (SelectedHotel == null)
            {
                MessageBox.Show("Ошибка! Невозможно редактировать пустую запись");
                return;
            }

            new EditHotelWindow(new EditHotelVM(_hotelRepository, SelectedHotel)).Show();
            await LoadHotelsAsync();
        }

        private async void DeleteHotelAsync(object obj)
        {
            try
            {
                var result = await _hotelRepository.DeleteHotelAsync(SelectedHotel.Id);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить отель, который не существует.");
                    return;
                }

                Hotels.Remove(SelectedHotel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении отеля: {ex.Message}");
            }
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if (Hotels == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По названию":
                    FilterByName();
                    break;

                case "По адресу":
                    FilterByAddress();
                    break;

                case "По к-ву этажей":
                    FilterByStageCount();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByStageCount()
        {
            var hotelsList = new ObservableCollection<Hotel>();
            foreach (var hotel in Hotels)
            {
                var parsedStageCount = hotel.StageCount.ToString();
                if (parsedStageCount.StartsWith(SearchData.ToLower()))
                {
                    hotelsList.Add(hotel);
                }
            }
            Hotels = hotelsList;
            return;
        }

        private void FilterByAddress()
        {
            var hotelsList = new ObservableCollection<Hotel>();
            foreach (var hotel in Hotels)
            {
                if (hotel.FullAddress.ToLower().StartsWith(SearchData.ToLower()) ||
                    hotel.FullAddress.ToLower().Contains(SearchData.ToLower()))
                {
                    hotelsList.Add(hotel);
                }
            }
            Hotels = hotelsList;
            return;
        }

        private void FilterByName()
        {
            var hotelsList = new ObservableCollection<Hotel>();
            foreach (var hotel in Hotels)
            {
                if (hotel.Name.ToLower().StartsWith(SearchData.ToLower()) ||
                    hotel.Name.ToLower().Contains(SearchData.ToLower()))
                {
                    hotelsList.Add(hotel);
                }
            }
            Hotels = hotelsList;
            return;
        }
    }
}
