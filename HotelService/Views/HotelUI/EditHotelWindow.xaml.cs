using HotelService.ViewModels.HotelVM;
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

namespace HotelService.Views.HotelUI
{
    /// <summary>
    /// Логика взаимодействия для EditHotelWindow.xaml
    /// </summary>
    public partial class EditHotelWindow : Window
    {
        public EditHotelWindow(EditHotelVM editHotelVM)
        {
            InitializeComponent();
            DataContext = editHotelVM;
        }
    }
}
