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

namespace EasyWorkDefault.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string repeatPassword = RepeatPasswordTextBox.Text;


            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(repeatPassword))
            {
                MessageBox.Show("Dane: " + "\n" + name + "\n" + surname + "\n" + email + "\n" + password + "\n" + repeatPassword);
                MessageBox.Show("Proszę uzupełnic pola", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!email.Contains('@'))
                {
                    MessageBox.Show("Proszę o podanie poprawnego emailu", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (password != repeatPassword)
                    {
                        MessageBox.Show("Haslo powtorzone ma być takie samo jak glowne", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        User newUser = new User
                        {
                            Name = name,
                            Surname = surname,
                            Email = email,
                            PasswordHash = password,
                            IsAdmin = false
                        };
                        MessageBox.Show("Pomyślnie zarejestrowano", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Database.SaveUserToDatabase(newUser);

                        GoToLoginPage();
                    }
                }
            }
        }

        private void GoToRegisterPage(object sender, RoutedEventArgs e)
        {

        }

        private void BackGoMainPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }

        private void GoToLoginrPage(object sender, RoutedEventArgs e)
        {
            GoToLoginPage();
        }

        public void GoToLoginPage()
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new LoginPage());
            }
        }
    }
}
