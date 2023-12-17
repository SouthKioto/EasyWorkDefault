using EasyWorkDefault.Classes;
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
    /// Logika interakcji dla klasy ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private User currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            currentUser = UserManager.CurrentUser;

            Title.Text = "Witaj" + currentUser.Name + "!";
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

        private void UpdateUserProfile()
        {

           
            
        }
    }
}
