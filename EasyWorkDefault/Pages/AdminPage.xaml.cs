﻿using EasyWorkDefault.View;
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

namespace EasyWorkDefault.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            UserListViewRadioButton(null, null);
        }

        private void BackToMainPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }

        private void UserListViewRadioButton(object sender, RoutedEventArgs e)
        {
            AdminPageView.Navigate(new UserListView());
        }

        private void AdvertListViewRadioButton(object sender, RoutedEventArgs e)
        {
            AdminPageView.Navigate(new AnnoucementListView());
        }
    }
}
