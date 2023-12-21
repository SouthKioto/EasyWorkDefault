using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
using EasyWorkDefault.Pages;
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
    /// Logika interakcji dla klasy LikedAnnoucemends.xaml
    /// </summary>
    public partial class LikedAnnoucemends : Page
    {
        public LikedAnnoucemends()
        {
            InitializeComponent();
            LoadLikedAnnouncements();
        }

        private void LoadLikedAnnouncements()
        {
            List<LikedAnnouncement> likedAnnouncements = Database.GetLikedAnnouncementsForUser(UserManager.CurrentUser.ID);

            foreach (var likedAnnouncement in likedAnnouncements)
            {
                AnnouncementDetails announcementDetails = Database.GetAnnouncementDetails(likedAnnouncement.NotificationOfWorkId);

                likedAnnouncement.NotificationTitle = announcementDetails.AnnouncementTitle;
                likedAnnouncement.ExpiryDate = announcementDetails.ExpiryDate;
            }

            LikedAnnouncements.ItemsSource = likedAnnouncements;
        }

        private void ShowAnnouncementDetails(object sender, MouseButtonEventArgs e)
        {
            if (LikedAnnouncements.SelectedItem != null && LikedAnnouncements.SelectedItem is LikedAnnouncement)
            {
                LikedAnnouncement selectedAnnouncement = (LikedAnnouncement)LikedAnnouncements.SelectedItem;
                if (selectedAnnouncement != null)
                {
                    AnnouncementDetails announcementDetails = Database.GetAnnouncementDetails(selectedAnnouncement.NotificationOfWorkId);

                    if (announcementDetails != null)
                    {
                        string message = $"Title: {announcementDetails.AnnouncementTitle}\n" +
                                         $"Expiry Date: {announcementDetails.ExpiryDate.ToString("yyyy-MM-dd")}\n";

                        MessageBox.Show(message, "Announcement Details", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unable to retrieve announcement details.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

    }
}
