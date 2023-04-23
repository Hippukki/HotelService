using HotelService.ViewModels;
using HotelService.ViewModels.HotelVM;
using HotelService.ViewModels.ServiceVM;
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

        public MainWindow(CreateHotelVM createHotelVM, MainWindowVM mainWindowVM, CreateServiceVM createServiceVM)
        {
            InitializeComponent();
            _createHotelVM = createHotelVM;
            DataContext = mainWindowVM;
            _createServiceVM = createServiceVM;
        }

        private void OpenManagmentWindow(object sender, RoutedEventArgs e)
        {
            new ManagmentWindow(_createHotelVM, _createServiceVM).ShowDialog();
        }

        private void VisitorRegister(object sender, RoutedEventArgs e)
        {

        }

        private void LoadApartments(object sender, RoutedEventArgs e)
        {

        }

        private void LoadVisitors(object sender, RoutedEventArgs e)
        {

        }

        private void LoadServices(object sender, RoutedEventArgs e)
        {

        }

        private void LoadReviews(object sender, RoutedEventArgs e)
        {

        }
    }
}
