using EasyWorkDefault.Classes;
using EasyWorkDefault.View;
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
    /// Logika interakcji dla klasy MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        protected string imie { get; set; }
        protected string nazwisko { get; set; }
        protected string email { get; set; }
        protected bool isAdmin { get; set; }
        protected byte[] profImage { get; set; }
        protected bool isLogget { get; set; }

        public MainPage()
        {
            InitializeComponent();
            UpdateUserUI();

            AdvertisementPresentationMainView advertisementView = new AdvertisementPresentationMainView();
            mainWindowFrame.Content = advertisementView;
        }

        public MainPage(User user, bool isLogged)
        {
            InitializeComponent();

            this.imie = user.Name;
            this.nazwisko = user.Surname;
            this.email = user.Email;
            this.isAdmin = user.IsAdmin;
            this.profImage = user.ProfileImagePath;
            this.isLogget = isLogged;

            UpdateUserUI();
        }

        public void UpdateUserUI()
        {
            if (UserManager.IsUserLoggedIn())
            {   
                if(UserManager.UserIsAdmin())
                {
                    AdminPageButton.Visibility = Visibility.Visible;
                    AdminPageButton.Click += GoToAdminPage;
                }
                UserUiButton.Content = "Przejdź do profilu";
                UserUiButton.Click += GoToProfilePage;
            }
            else
            {
                UserUiButton.Content = "Zaloguj / Zarejestruj";
                UserUiButton.Click += GoToLoginPage;
            }
        }

        private void GoToLoginPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new LoginPage());
            }
        }

        private void GoToProfilePage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new ProfilePage());
            }
        }

        private void GoToAdminPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new AdminPage());
            }
        }
    }
}
