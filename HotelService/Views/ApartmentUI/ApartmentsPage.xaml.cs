using HotelService.ViewModels.ApartmentVM;
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

namespace HotelService.Views.ApartmentUI
{
    /// <summary>
    /// Логика взаимодействия для ApartmentsPage.xaml
    /// </summary>
    public partial class ApartmentsPage : Page
    {
        public ApartmentsPage(ApartmentsVM apartmentsVM)
        {
            InitializeComponent();
            DataContext = apartmentsVM;
        }
    }
}
