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

namespace HotelService.ViewModels.VisitorVM
{
    public class CreateVisitorVM : ViewModelBase
    {
        private readonly VisitorRepository _visitorRepository;
        private readonly PassportRepository _passportRepository;
        private Visitor? _selectedVisitor;

        public ObservableCollection<Visitor> CreatedVisitors { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatromicName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string? PassData { get; set; }
        public Visitor? SelectedVisitor
        {
            get => _selectedVisitor;
            set
            {
                _selectedVisitor = value;
                RaiseProperty();
            }
        }

        public CreateVisitorVM(VisitorRepository visitorRepository, PassportRepository passportRepository)
        {
            _visitorRepository = visitorRepository;
            _passportRepository = passportRepository;
            CreatedVisitors = new();
        }

        public ICommand Create => new DelegateCommand<object>(CreateVisitorAsync);
        public ICommand Delete => new DelegateCommand<object>(DeleteVisitorAsync);

        private async void CreateVisitorAsync(object obj)
        {
            if(FirstName == null || LastName == null || PatromicName == null || PhoneNumber == null
                || PassData == null || BirthDate.Date == DateTime.Today.Date)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены.");
                return;
            }

            try
            {
                var visitor = new Visitor
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    PatromicName = PatromicName,
                    PhoneNumber = PhoneNumber,
                    BirthDate = DateOnly.FromDateTime(BirthDate),
                    CreateDateTime = DateTime.Now
                };
                await _visitorRepository.CreateVisitorAsync(visitor);
                CreatedVisitors.Add(visitor);

                var passport = new Passport
                {
                    Data = PassData,
                    Visitor = visitor,
                    CreateDateTime = DateTime.Now
                };
                await _passportRepository.CreatePassportAsync(passport);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении посетителя: {ex.Message}");
            }
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
                CreatedVisitors.Remove(SelectedVisitor);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка во время удалении посетителя: {ex.Message}");
            }
        }
    }
}
