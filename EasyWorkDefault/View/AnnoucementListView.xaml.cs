using EasyWorkDefault.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logika interakcji dla klasy AnnoucementListView.xaml
    /// </summary>
    public partial class AnnoucementListView : Page
    {
        readonly ObservableCollection<NotificationoOfWork> _notification;
        readonly ObservableCollection<NotificationoOfWork> _user;
        public AnnoucementListView()
        {
            InitializeComponent();
            _notification = new ObservableCollection<NotificationoOfWork>(Data.Database.GetAllAdvertisements());
            AnnoucementListView_Display.ItemsSource = _notification;
        }

        private void AnnoucementListView_Display_Details(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;

            if (listView.SelectedItem != null)
            {
                var annoucement = (NotificationoOfWork)listView.SelectedItem;
                string mess = "Nazwa: " + annoucement.Notification_title + "\n Desript: " + annoucement.notification_descript + "\n Add Date: " + annoucement.Date_of_expiry_start;
                MessageBox.Show(mess, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No item selected", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
