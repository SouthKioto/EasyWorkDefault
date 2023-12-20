using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
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
using System.Windows.Threading;

namespace EasyWorkDefault.View
{
    /// <summary>
    /// Logika interakcji dla klasy AdvertisementPresentationMainView.xaml
    /// </summary>
    public partial class AdvertisementPresentationMainView : Page
    {
        private int currentAdvertisementIndex = 0;
        private DispatcherTimer timer;
        public ObservableCollection<NotificationoOfWork> Advertisements { get; set; }

        public AdvertisementPresentationMainView()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();

            Advertisements = new ObservableCollection<NotificationoOfWork>();

            UpdateAdvertisements();
            DataContext = this;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (Advertisements.Any())
            {
                currentAdvertisementIndex = (currentAdvertisementIndex + 1) % Advertisements.Count;
            }
            else
            {
                currentAdvertisementIndex = 0;
            }

            UpdateAdvertisements();
        }

        private void UpdateAdvertisements()
        {

            var advertisements = Database.GetAllAdvertisements();

            Advertisements.Clear();

            if (advertisements != null && advertisements.Any())
            {
                Advertisements.Add(advertisements[currentAdvertisementIndex]);
            }
        }
    }
}
