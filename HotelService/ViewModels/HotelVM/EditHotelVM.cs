using HotelService.DataContext.Repositories;
using HotelService.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.HotelVM
{
    public class EditHotelVM : ViewModelBase
    {
        private readonly HotelRepository _hotelRepository;
        private Hotel _selectedHotel;

        public string? HotelName { get; set; }
        public string? StageCount { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }

        public EditHotelVM(HotelRepository hotelRepository, Hotel selectedHotel)
        {
            _hotelRepository = hotelRepository;
            _selectedHotel = selectedHotel;
            FillProperties();
        }

        public ICommand Save => new DelegateCommand<object>(EditHotelAsync);

        private async void EditHotelAsync(object obj)
        {
            try
            {
                var editedHotel = await _hotelRepository.GetHotelByIdAsync(_selectedHotel.Id);
                editedHotel.Name = HotelName;
                editedHotel.StageCount = Convert.ToInt32(StageCount);
                editedHotel.FullAddress = $"г.{City}, ул.{Street}, д.{House}";
                editedHotel.UpdateDateTime = DateTime.Now;
                await _hotelRepository.UpdateHotelAsync();

                MessageBox.Show("Сохранение прошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении отеля: {ex.Message}");
            }
        }

        private void FillProperties()
        {
            HotelName = _selectedHotel.Name;
            StageCount = _selectedHotel.StageCount.ToString();

            string[] subStrings = _selectedHotel.FullAddress.Split(',');
            City = subStrings[0].TrimStart('г', '.');
            Street = subStrings[1].TrimStart(' ','у','л','.');
            House = subStrings[2].TrimStart(' ','д','.');
        }
    }
}
