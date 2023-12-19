using EasyWorkDefault.Classes;
using EasyWorkDefault.Windows;
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

namespace EasyWorkDefault.View
{
    /// <summary>
    /// Logika interakcji dla klasy UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : Page
    {
        public User CurrUser { get; set; }

        public UserProfileView(User user)
        {
            InitializeComponent();
            CurrUser = user;

            userNameSurname.Text = user.Name + " " + user.Surname;
            userEmail.Text = user.Email;
            userBirthDate.Text = user.BirthDate.ToLongDateString();
        }

        private void EditProfile_Window(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow(CurrUser);
            editWindow.Show();
        }
    }
}
