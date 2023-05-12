using HotelService.DataContext.Repositories;
using HotelService.ViewModels.ApartmentVM;
using HotelService.ViewModels.HotelVM;
using HotelService.ViewModels.ServiceVM;
using HotelService.ViewModels.VisitorVM;
using HotelService.Views.ApartmentUI;
using HotelService.Views.VisitorUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelService.Views
{
    /// <summary>
    /// Логика взаимодействия для ManagmentWindow.xaml
    /// </summary>
    public partial class ManagmentWindow : Window
    {
        private readonly CreateHotelVM _createHotelViewModel;
        private readonly CreateServiceVM _createServiceViewModel;
        private readonly CreateApartmentVM _createApartmentVM;
        private readonly CreateVisitorVM _createVisitorVM;

        public ManagmentWindow(CreateHotelVM createHotelViewModel, 
            CreateServiceVM createServiceViewModel, CreateApartmentVM createApartmentVM, CreateVisitorVM createVisitorVM)
        {
            InitializeComponent();
            _createHotelViewModel = createHotelViewModel;
            _createServiceViewModel = createServiceViewModel;
            _createApartmentVM = createApartmentVM;
            _createVisitorVM = createVisitorVM;
        }

        private void OpenCreateHotelPage(object sender, RoutedEventArgs e)
        {
            ManagmentFrame.Navigate(new CreateHotelPage(_createHotelViewModel));
        }

        private void OpenCreateServicePage(object sender, RoutedEventArgs e)
        {
            ManagmentFrame.Navigate(new CreateServicePage(_createServiceViewModel));
        }

        private void OpenCreateApartmentPage(object sender, RoutedEventArgs e)
        {
            ManagmentFrame.Navigate(new CreateApartmentsPage(_createApartmentVM));
        }

        private void OpenCreateVisitorPage(object sender, RoutedEventArgs e)
        {
            ManagmentFrame.Navigate(new CreateVisitorPage(_createVisitorVM));
        }
    }
}
