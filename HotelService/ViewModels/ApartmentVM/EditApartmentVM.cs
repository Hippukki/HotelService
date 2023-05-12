using HotelService.DataContext.Repositories;
using HotelService.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.ApartmentVM
{
    public class EditApartmentVM : ViewModelBase
    {
        private readonly ApartmentRepository _apartmentRepository;
        private ApartmentDto _selectedApartment;

        public Dictionary<int, string>? Categories { get; set; }
        public Dictionary<int, string>? Levels { get; set; }
        public string? Stage { get; set; }
        public string? Number { get; set; }
        public int? SelectedCategory { get; set; }
        public int? SelectedLevel { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }

        public EditApartmentVM(ApartmentRepository apartmentRepository, ApartmentDto selectedApartment)
        {
            _apartmentRepository = apartmentRepository;
            _selectedApartment = selectedApartment;
            FillProperties();
        }

        public ICommand Save => new DelegateCommand<object>(EditApartmentAsync);

        private async void EditApartmentAsync(object obj)
        {
            try
            {
                var parsedPrice = Convert.ToDecimal(Price);
                var parsedNumber = Convert.ToInt32(Number);
                var parsedStage = Convert.ToInt32(Stage);
                var editedApartment = await _apartmentRepository.GetApartmentByIdAsync(_selectedApartment.Id);
                editedApartment.Number = parsedNumber;
                editedApartment.Stage = parsedStage;
                editedApartment.Price = parsedPrice;
                editedApartment.ShortDescription = Description;
                editedApartment.Category = (ApartmentCategory)SelectedCategory;
                editedApartment.Level = (ApartmentLevel)SelectedLevel;
                editedApartment.CreateDateTime = _selectedApartment.CreateDateTime;
                editedApartment.UpdateDateTime = DateTime.Now;
                await _apartmentRepository.UpdateApartmentAsync();

                MessageBox.Show("Сохранение прошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении номера: {ex.Message}");
            }
        }

        private void FillProperties()
        {
            Stage = _selectedApartment.Stage.ToString();
            Number = _selectedApartment.Number.ToString();
            Description = _selectedApartment.ShortDescription;
            Price = _selectedApartment.Price.ToString();

            Categories = new();
            var categoriesList = Enum.GetValues(typeof(ApartmentCategory));
            foreach (var category in categoriesList)
            {
                Categories.Add((int)category, category.GetType()
                        .GetMember(category.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName());
            }
            SelectedCategory = (from pair in Categories where pair.Value == _selectedApartment.Category select pair.Key).FirstOrDefault();

            Levels = new();
            var levelsList = Enum.GetValues(typeof(ApartmentLevel));
            foreach (var level in levelsList)
            {
                Levels.Add((int)level, level.GetType()
                    .GetMember(level.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName());
            }
            SelectedLevel = (from pair in Levels where pair.Value == _selectedApartment.Level select pair.Key).FirstOrDefault();
        }
    }
}
