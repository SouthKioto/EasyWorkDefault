using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
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
using System.Xml.Linq;

namespace EasyWorkDefault.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        

        public LoginPage()
        {
            InitializeComponent();
        }

        private void BackGoMainPage(object sender, RoutedEventArgs e)
        {
            BackToMainPage();
        }

        private void GoToRegisterPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new RegisterPage());
            }
        }

        private void LoginAndBackToMainPage(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            User existingUser = Database.GetUserFromDatabase(email);

            if (existingUser != null)
            {
                if(existingUser.PasswordHash == passwordTextBox.Text)
                {
                    UserManager.SetCurrentUser(existingUser);
                    BackToMainPageExistUser(existingUser);
                }
                else
                {
                    MessageBox.Show("Nie poprawne hasło", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("Użytkownik nie istnieje w bazie", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void BackToMainPageExistUser(User user)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {                
                mainWindow.ChangePage(new MainPage(user, true));
            }
        }

        void BackToMainPage()
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }
    }
}
