﻿using HotelService.ViewModels.ApartmentVM;
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

namespace HotelService.Views.ApartmentUI
{
    /// <summary>
    /// Логика взаимодействия для EditApartmentWindow.xaml
    /// </summary>
    public partial class EditApartmentWindow : Window
    {
        public EditApartmentWindow(EditApartmentVM editApartmentVM)
        {
            InitializeComponent();
            DataContext = editApartmentVM;
        }
    }
}
