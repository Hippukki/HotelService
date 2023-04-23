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

namespace HotelService.ViewModels.HotelVM
{
    public class CreateHotelVM : ViewModelBase
    {
        private readonly HotelRepository _hotelRepository;
        private Hotel? _selectedHotel;

        public string? HotelName { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value;
                RaiseProperty();
            }
        }
        public ObservableCollection<Hotel> CreatedHotels { get; set; }

        public CreateHotelVM(HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
            CreatedHotels = new();
        }

        public ICommand Create => new DelegateCommand<object>(CreateHotelAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteHotelAsync);

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

                CreatedHotels.Remove(SelectedHotel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении отеля: {ex.Message}");
            }
        }

        public async void CreateHotelAsync(object obj)
        {
            if (HotelName == null || City == null || Street == null || House == null)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
            }

            try
            {
                var fullAddress = $"г.{City}, ул.{Street}, д.{House}";
                var hotel = new Hotel()
                {
                    Name = HotelName,
                    FullAddress = fullAddress,
                    CreateDateTime = DateTime.Now
                };
                await _hotelRepository.CreateHotelAsync(hotel);
                CreatedHotels.Add(hotel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время сохранения отеля: {ex.Message}");
            }
        }
    }
}
