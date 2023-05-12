using HotelService.DataContext.Repositories;
using HotelService.Models;
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

namespace HotelService.ViewModels.ApartmentVM
{
    public class CreateApartmentVM : ViewModelBase
    {
        private readonly ApartmentRepository _apartmentRepository;
        private readonly HotelRepository _hotelRepository;
        private Apartment? _selectedApartment;

        public ObservableCollection<Hotel>? Hotels { get; set; }
        public ObservableCollection<Apartment> CreatedApartments { get; set; }
        public Dictionary<int, string>? Categories { get; set; }
        public Dictionary<int, string>? Levels { get; set; }
        public string? Stage { get; set; }
        public string? Number { get; set; }
        public int? SelectedCategory { get; set; }
        public int? SelectedLevel { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public Hotel? SelectedHotel { get; set; }
        public Apartment? SelectedApartment
        {
            get => _selectedApartment;
            set
            {
                _selectedApartment = value;
                RaiseProperty();
            }
        }

        public CreateApartmentVM(ApartmentRepository apartmentRepository, HotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
            _apartmentRepository = apartmentRepository;
            CreatedApartments = new();
            FillPropertiesAsync();
        }

        public ICommand Create => new DelegateCommand<object>(CreateApartmentAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteApartmentAsync);

        private async void CreateApartmentAsync(object obj)
        {
            if(Stage == null || Number == null || SelectedCategory == null || SelectedLevel == null
                || Description == null || Price == null || SelectedHotel == null)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
                return;
            }

            try
            {
                var parsedPrice = Convert.ToDecimal(Price);
                var parsedNumber = Convert.ToInt32(Number);
                var parsedStage = Convert.ToInt32(Stage);
                var apartment = new Apartment
                {
                    Number = parsedNumber,
                    Stage = parsedStage,
                    Price = parsedPrice,
                    ShortDescription = Description,
                    Category = (ApartmentCategory)SelectedCategory,
                    Level = (ApartmentLevel)SelectedLevel,
                    Hotel = SelectedHotel,
                    CreateDateTime = DateTime.Now
                };
                await _apartmentRepository.CreateApartmentAsync(apartment);
                CreatedApartments.Add(apartment);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении номера: {ex.Message}");
            }
        }

        private async void DeleteApartmentAsync(object obj)
        {
            try
            {
                var result = await _apartmentRepository.DeleteApartmentAsync(SelectedApartment.Id);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить номер, который не существует.");
                    return;
                }

                CreatedApartments.Remove(SelectedApartment);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении номера: {ex.Message}");
            }
        }

        private async Task FillPropertiesAsync()
        {
            Categories = new();
            var categoriesList = Enum.GetValues(typeof(ApartmentCategory));
            foreach (var category in categoriesList)
            {
                Categories.Add((int)category, category.GetType()
                        .GetMember(category.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName());
            }

            Levels = new();
            var levelsList = Enum.GetValues(typeof(ApartmentLevel));
            foreach(var level in levelsList)
            {
                Levels.Add((int)level, level.GetType()
                    .GetMember(level.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName());
            }
            await Task.Delay(2000);
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
