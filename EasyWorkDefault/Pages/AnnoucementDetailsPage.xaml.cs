using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
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
    /// Logika interakcji dla klasy AnnoucementDetailsPage.xaml
    /// </summary>
    public partial class AnnoucementDetailsPage : Page
    {
        public NotificationoOfWork SelectedAnnouncement { get; set; }

        public AnnoucementDetailsPage(NotificationoOfWork selectedAnnouncement)
        {
            InitializeComponent();
            SelectedAnnouncement = selectedAnnouncement;
            DataContext = this;
        }

        private void BackToSearchPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new SearchPage(""));
            }
        }

        private void AddToLiked(object sender, RoutedEventArgs e)
        {
            if (UserManager.CurrentUser != null)
            {
                Data.Database.AddToLike(UserManager.CurrentUser.ID, SelectedAnnouncement.NotificationId);
            }
            else
            {
                MessageBox.Show("Proszę się zalogować", "Warning", MessageBoxButton.OK);
            }
            
        }
    }
}
