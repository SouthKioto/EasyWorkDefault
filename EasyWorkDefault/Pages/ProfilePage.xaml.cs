using EasyWorkDefault.Classes;
using EasyWorkDefault.View;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            currentUser = UserManager.CurrentUser;
            RadioButtonProfile.Checked += RadioButtonProfile_Checked;
            RadioButtonProfile_Checked(null, null);

            CreateAdvertRadio.Checked += RadioButtonCreateAdvert_Checked;
        }

        private void LogOutAndBackToMainPage(object sender, RoutedEventArgs e)
        {
            UserManager.Logout();

            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }

        private void BackToMainPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }

        private void RadioButtonProfile_Checked(object sender, RoutedEventArgs e)
        {
            PageMainView.Navigate(new UserProfileView(currentUser));
        }

        private void RadioButtonCreateAdvert_Checked(object sender, RoutedEventArgs e)
        {
            PageMainView.Navigate(new AddAdvertView(currentUser));
        }

        private void DisplayLikedAnnoucemends(object sender, RoutedEventArgs e)
        {
            PageMainView.Navigate(new LikedAnnoucemends());
        }
    }
}
