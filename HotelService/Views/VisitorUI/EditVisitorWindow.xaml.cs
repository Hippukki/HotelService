using HotelService.ViewModels.VisitorVM;
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

namespace HotelService.Views.VisitorUI
{
    /// <summary>
    /// Логика взаимодействия для EditVisitorWindow.xaml
    /// </summary>
    public partial class EditVisitorWindow : Window
    {
        public EditVisitorWindow(EditVisitorVM editVisitorVM)
        {
            InitializeComponent();
            DataContext = editVisitorVM;
        }
    }
}
