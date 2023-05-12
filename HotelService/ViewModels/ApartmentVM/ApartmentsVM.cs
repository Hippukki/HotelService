using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.ApartmentUI;
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
    public class ApartmentsVM : ViewModelBase
    {
        private readonly ApartmentRepository _apartmentRepository;
        private Hotel _selectedHotel;
        private ObservableCollection<ApartmentDto>? _apartmentslList;
        private ApartmentDto? _selectedApartmnet;

        public ObservableCollection<ApartmentDto>? Apartments 
        { 
            get => _apartmentslList; 
            set
            {
                _apartmentslList = value;
                RaiseProperty();
            }
        }
        public ApartmentDto? SelectedApartment
        {
            get => _selectedApartmnet;
            set
            {
                _selectedApartmnet = value;
                RaiseProperty();
            }
        }
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По категории",
            "По виду",
            "По цене",
            "По этажу",
            "По номеру"
        };
        public string? SelectedType { get; set; }

        public ApartmentsVM(ApartmentRepository apartmentRepository, Hotel selectedHotel)
        {
            _apartmentRepository = apartmentRepository;
            _selectedHotel = selectedHotel;
            LoadApartments();
        }

        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand Edit => new DelegateCommand<object>(EditApartmentAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteApartmentAsync);

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

                Apartments.Remove(SelectedApartment);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении номера: {ex.Message}");
            }
        }

        private async void EditApartmentAsync(object obj)
        {
            if(SelectedApartment == null)
            {
                MessageBox.Show("Ошибка! Невозможно редактировать пустую запись");
                return;
            }

            new EditApartmentWindow(new EditApartmentVM(_apartmentRepository, SelectedApartment)).Show();
            await LoadApartments();
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if(Apartments == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По категории":
                    FilterByCategory();
                    break;

                case "По виду":
                    FilterByLevel();
                    break;

                case "По цене":
                    FilterByPrice();
                    break;

                case "По этажу":
                    FilterByStage();
                    break;

                case "По номеру":
                    FilterByNumber();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByNumber()
        {
            var apartmentsList = new ObservableCollection<ApartmentDto>();
            foreach (var apartment in Apartments)
            {
                var parsedNumber = apartment.Number.ToString();
                if (parsedNumber.ToLower().StartsWith(SearchData.ToLower()))
                {
                    apartmentsList.Add(apartment);
                }
            }
            Apartments = apartmentsList;
            return;
        }

        private void FilterByStage()
        {
            var apartmentsList = new ObservableCollection<ApartmentDto>();
            foreach (var apartment in Apartments)
            {
                var parsedStage = apartment.Stage.ToString();
                if (parsedStage.ToLower().StartsWith(SearchData.ToLower()))
                {
                    apartmentsList.Add(apartment);
                }
            }
            Apartments = apartmentsList;
            return;
        }

        private void FilterByPrice()
        {

            var apartmentsList = new ObservableCollection<ApartmentDto>();
            foreach (var apartment in Apartments)
            {
                var parsedPrice = apartment.Price.ToString();
                if (parsedPrice.ToLower().StartsWith(SearchData.ToLower()))
                {
                    apartmentsList.Add(apartment);
                }
            }
            Apartments = apartmentsList;
            return;
        }

        private void FilterByLevel()
        {
            var apartmentsList = new ObservableCollection<ApartmentDto>();
            foreach (var apartment in Apartments)
            {
                if (apartment.Level.ToLower().StartsWith(SearchData.ToLower()) ||
                    apartment.Level.ToLower().Contains(SearchData.ToLower()))
                {
                    apartmentsList.Add(apartment);
                }
            }
            Apartments = apartmentsList;
            return;
        }

        private void FilterByCategory()
        {
            var apartmentsList = new ObservableCollection<ApartmentDto>();
            foreach (var apartment in Apartments)
            {
                if (apartment.Category.ToLower().StartsWith(SearchData.ToLower()))
                {
                    apartmentsList.Add(apartment);
                }
            }
            Apartments = apartmentsList;
            return;
        }

        private async Task LoadApartments()
        {
            Apartments = new();
            var apartments = await _apartmentRepository.GetApartmentsByHotelIdAsync(_selectedHotel.Id);
            if(apartments != null)
            {
                foreach(var apartment in apartments)
                {
                    Apartments.Add(new ApartmentDto
                    {
                        Id = apartment.Id,
                        Number = apartment.Number,
                        Stage = apartment.Stage,
                        Category = apartment.Category.GetType()
                        .GetMember(apartment.Category.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName(),
                        Level = apartment.Level.GetType()
                        .GetMember(apartment.Level.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName(),
                        ShortDescription = apartment.ShortDescription,
                        Price = Convert.ToInt32(apartment.Price),
                        Hotel = apartment.Hotel,
                        CreateDateTime = apartment.CreateDateTime
                    });
                }
            }
        }
    }
}
