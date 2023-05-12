using HotelService.DataContext.Repositories;
using HotelService.Models;
using HotelService.Views.VisitorUI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.VisitorVM
{
    public class VisitorsVM : ViewModelBase
    {
        private readonly VisitorRepository _visitorRepository;
        private readonly PassportRepository _passportRepository;
        private ObservableCollection<VisitorDto>? _visitorslList;
        private VisitorDto? _selectedVisitor;

        public ObservableCollection<VisitorDto>? Visitors
        {
            get => _visitorslList;
            set
            {
                _visitorslList = value;
                RaiseProperty();
            }
        }
        public VisitorDto? SelectedVisitor
        {
            get => _selectedVisitor;
            set
            {
                _selectedVisitor = value;
                RaiseProperty();
            }
        }
        public string? SearchData { get; set; }
        public List<string>? SearchTypes { get; set; } = new List<string>
        {
            "По имени",
            "По фамилии",
            "По отчеству",
            "По номеру телефона"
        };
        public string? SelectedType { get; set; }

        public VisitorsVM(VisitorRepository visitorRepository, PassportRepository passportRepository)
        {
            _visitorRepository = visitorRepository;
            _passportRepository = passportRepository;
            LoadVisitorsAsync();
        }

        public ICommand Search => new DelegateCommand<object>(TrySearch);
        public ICommand Delete => new DelegateCommand<object>(DeleteVisitorAsync);
        public ICommand Edit => new DelegateCommand<object>(EditVisitorAsync);

        private async void EditVisitorAsync(object obj)
        {
            if (SelectedVisitor == null)
            {
                MessageBox.Show("Ошибка! Невозможно редактировать пустую запись");
                return;
            }

            new EditVisitorWindow(new EditVisitorVM(_visitorRepository, _passportRepository, SelectedVisitor)).Show();
            await LoadVisitorsAsync();
        }

        private async void DeleteVisitorAsync(object obj)
        {
            try
            {
                var result = await _visitorRepository.DeteleVisitorAsync(SelectedVisitor.Id);
                if (!result)
                {
                    MessageBox.Show("Не удалось удалить посетителя, который не существует.");
                    return;
                }

                var passportList = await _passportRepository.GetPassports(SelectedVisitor.Id);
                passportList.ForEach(async p => await _passportRepository.DeletePassportAsync(p.Id));
                Visitors.Remove(SelectedVisitor);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении посетителя: {ex.Message}");
            }
        }

        private void TrySearch(object obj)
        {
            if (String.IsNullOrEmpty(SearchData))
            {
                MessageBox.Show("Ошибка! Поле поиска не заполнено.");
                return;
            }

            if (Visitors == null)
            {
                MessageBox.Show("Невозможно выполнить поиск, пока не существует ни одной записи.");
                return;
            }

            switch (SelectedType)
            {
                case "По имени":
                    FilterByName();
                    break;

                case "По фамилии":
                    FilterBySurname();
                    break;

                case "По отчеству":
                    FilterByLastName();
                    break;

                case "По номеру телефона":
                    FilterByPhoneNumber();
                    break;

                default:
                    MessageBox.Show("Ошибка! Не выбран параметр поиска.");
                    break;
            }
        }

        private void FilterByName()
        {
            var visitorsList = new ObservableCollection<VisitorDto>();
            foreach (var visitor in Visitors)
            {
                if (visitor.FirstName.ToLower().StartsWith(SearchData.ToLower()))
                {
                    visitorsList.Add(visitor);
                }
            }
            Visitors = visitorsList;
            return;
        }

        private void FilterBySurname()
        {
            var visitorsList = new ObservableCollection<VisitorDto>();
            foreach (var visitor in Visitors)
            {
                if (visitor.LastName.ToLower().StartsWith(SearchData.ToLower()))
                {
                    visitorsList.Add(visitor);
                }
            }
            Visitors = visitorsList;
            return;
        }

        private void FilterByLastName()
        {
            var visitorsList = new ObservableCollection<VisitorDto>();
            foreach (var visitor in Visitors)
            {
                if (visitor.PatromicName.ToLower().StartsWith(SearchData.ToLower()))
                {
                    visitorsList.Add(visitor);
                }
            }
            Visitors = visitorsList;
            return;
        }

        private void FilterByPhoneNumber()
        {
            var visitorsList = new ObservableCollection<VisitorDto>();
            foreach (var visitor in Visitors)
            {
                if (visitor.PhoneNumber.ToLower().StartsWith(SearchData.ToLower()))
                {
                    visitorsList.Add(visitor);
                }
            }
            Visitors = visitorsList;
            return; ;
        }

        private async Task LoadVisitorsAsync()
        {
            Visitors = new();
            var visitorsList = await _visitorRepository.GetVisitorsAsync();
            if(visitorsList != null)
            {
                foreach (var visitor in visitorsList)
                {
                    var visitorDto = new VisitorDto
                    {
                        Id = visitor.Id,
                        FirstName = visitor.FirstName,
                        LastName = visitor.LastName,
                        PatromicName = visitor.PatromicName,
                        BirthDate = visitor.BirthDate,
                        PhoneNumber = visitor.PhoneNumber,
                        CreateDateTime = visitor.CreateDateTime
                    };
                    visitorDto.PassData = new();
                    visitorDto.PassData = (await _passportRepository.GetPassports(visitor.Id)).FirstOrDefault();
                    Visitors.Add(visitorDto);
                }
            }
        }
    }
}
