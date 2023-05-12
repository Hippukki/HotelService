using HotelService.DataContext.Repositories;
using HotelService.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelService.ViewModels.VisitorVM
{
    public class EditVisitorVM : ViewModelBase
    {
        private readonly VisitorRepository _visitorRepository;
        private readonly PassportRepository _passportRepository;
        private VisitorDto _selectedVisitor;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatromicName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PassData { get; set; }

        public EditVisitorVM(VisitorRepository visitorRepository, PassportRepository passportRepository, VisitorDto selectedVisistor)
        {
            _visitorRepository = visitorRepository;
            _passportRepository = passportRepository;
            _selectedVisitor = selectedVisistor;
            FillProperties();
        }

        public ICommand Save => new DelegateCommand<object>(EditVisitorAsync);

        private async void EditVisitorAsync(object obj)
        {
            try
            {
                var editedVisitor = await _visitorRepository.GetVisitorByIdAsync(_selectedVisitor.Id);
                editedVisitor.FirstName = FirstName;
                editedVisitor.LastName = LastName;
                editedVisitor.PhoneNumber = PhoneNumber;
                editedVisitor.PatromicName = PatromicName;
                await _visitorRepository.UpdateVisitorAsync();

                var editedPassport = await _passportRepository.GetPassportByIdAsync(_selectedVisitor.PassData.Id);
                editedPassport.Data = PassData;
                await _passportRepository.UpdatePassportAsync();

                MessageBox.Show("Сохранение прошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении пользователя: {ex.Message}");
            }
        }

        private void FillProperties()
        {
            FirstName = _selectedVisitor.FirstName;
            LastName = _selectedVisitor.LastName;
            PatromicName = _selectedVisitor.PatromicName;
            PhoneNumber = _selectedVisitor.PhoneNumber;
            PassData = _selectedVisitor.PassData.Data;
        }
    }
}
