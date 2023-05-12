using HotelService.ViewModels;
using HotelService.ViewModels.ApartmentVM;
using HotelService.ViewModels.HotelVM;
using HotelService.ViewModels.ServiceVM;
using HotelService.ViewModels.VisitorVM;
using HotelService.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CreateHotelVM _createHotelVM;
        private readonly CreateServiceVM _createServiceVM;
        private readonly CreateApartmentVM _createApartmentVM;
        private readonly CreateVisitorVM _createVisitorVM;

        public MainWindow(CreateHotelVM createHotelVM, MainWindowVM mainWindowVM, 
            CreateServiceVM createServiceVM, CreateApartmentVM createApartmentVM, CreateVisitorVM createVisitorVM)
        {
            InitializeComponent();
            _createHotelVM = createHotelVM;
            DataContext = mainWindowVM;
            _createServiceVM = createServiceVM;
            _createApartmentVM = createApartmentVM;
            _createVisitorVM = createVisitorVM;
        }

        private void OpenManagmentWindow(object sender, RoutedEventArgs e)
        {
            new ManagmentWindow(_createHotelVM, _createServiceVM, _createApartmentVM, _createVisitorVM).ShowDialog();
        }
    }
}
