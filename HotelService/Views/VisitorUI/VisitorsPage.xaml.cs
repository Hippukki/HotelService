﻿using HotelService.ViewModels.VisitorVM;
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

namespace HotelService.Views.VisitorUI
{
    /// <summary>
    /// Логика взаимодействия для VisitorsPage.xaml
    /// </summary>
    public partial class VisitorsPage : Page
    {
        public VisitorsPage(VisitorsVM visitorsVM)
        {
            InitializeComponent();
            DataContext = visitorsVM;
        }
    }
}
