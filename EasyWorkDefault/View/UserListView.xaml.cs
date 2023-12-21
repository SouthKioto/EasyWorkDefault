using EasyWorkDefault.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logika interakcji dla klasy UserListView.xaml
    /// </summary>
    public partial class UserListView : Page
    {
        readonly ObservableCollection<User> _users;
        public UserListView()
        {
            InitializeComponent();
            _users = new ObservableCollection<User>(Data.Database.GetAllUsers());
            UserListView_Display.ItemsSource = _users;
        }

        private void ShowUserData(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;

            if (listView != null)
            {
                var user = (User)listView.SelectedItem;
                string mess = "Imie: " + user.Name + "\nNazwisko: " + user.Surname + "\nBirth Date: " + user.BirthDate + "\nEmail: " + user.Email + "\nIsAdmin: " + user.IsAdmin;
                MessageBox.Show(mess, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No item selected", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
