using HotelService.DataContext.Repositories;
using HotelService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HotelService.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly HotelRepository _hotelRepository;
        private Hotel? _selectedHotel;

        public List<Hotel>? Hotels { get; set; }
        public Hotel? SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value;
                RaiseProperty();
            }
        }

        public MainWindowVM(HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
            FillHotelsAsync();
        }

        private async Task FillHotelsAsync()
        {
            Hotels = await _hotelRepository.GetListHotelsAsync();
        }
    }
}
