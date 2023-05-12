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

namespace HotelService.ViewModels.ServiceVM
{
    public class EditServiceVM : ViewModelBase
    {
        private readonly ServiceRepository _serviceRepository;
        private Service _selectedService;

        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }

        public EditServiceVM(ServiceRepository serviceRepository, Service selectedService)
        {
            _serviceRepository = serviceRepository;
            _selectedService = selectedService;
            FillProperties();
        }

        public ICommand Save => new DelegateCommand<object>(EditServiceAsync);

        private async void EditServiceAsync(object obj)
        {
            try
            {
                var editedService = await _serviceRepository.GetServiceByIdAsync(_selectedService.Id);
                editedService.ServiceName = ServiceName;
                editedService.Description = Description;
                editedService.Price = Convert.ToDecimal(Price);
                editedService.UpdateDateTime = DateTime.Now;
                await _serviceRepository.UpdateServiceAsync();

                MessageBox.Show("Сохранение прошло успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении отеля: {ex.Message}");
            }
        }

        private void FillProperties()
        {
            ServiceName = _selectedService.ServiceName;
            Description = _selectedService.Description;
            Price = _selectedService.Price.ToString();
        }
    }
}
