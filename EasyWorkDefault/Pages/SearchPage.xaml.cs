using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
using EasyWorkDefault.View;
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

namespace EasyWorkDefault.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        private ObservableCollection<WorkPosition> workPositions = new ObservableCollection<WorkPosition>(Data.Database.GetWorkPositionsFromDatabase());
        private ObservableCollection<Category> categories = new ObservableCollection<Category>(Data.Database.GetCategoriesFromDatabase());
        private ObservableCollection<JobLevel> jobLevels = new ObservableCollection<JobLevel>(Data.Database.GetJobLevelsFromDatabase());
        private ObservableCollection<WorkType> workTypes = new ObservableCollection<WorkType>(Data.Database.GetWorkTypesFromDatabase());
        private ObservableCollection<EmploymentDimension> employmentDimensions = new ObservableCollection<EmploymentDimension>(Data.Database.GetEmploymentDimensionsFromDatabase());

        private readonly ObservableCollection<NotificationoOfWork> _allNotifications;
        private readonly ObservableCollection<NotificationoOfWork> _filteredNotifications;

        public SearchPage(string searchText)
        {
            InitializeComponent();
            DisplayMessageForSearchValue(searchText);

            _allNotifications = new ObservableCollection<NotificationoOfWork>(Data.Database.GetAllAdvertisements());
            _filteredNotifications = new ObservableCollection<NotificationoOfWork>(_allNotifications);

            AnnoucementList.ItemsSource = _filteredNotifications;

            ApplySearchFilter(searchText);

            WorkPosition.ItemsSource = workPositions;
            Category.ItemsSource = categories;
            WorkLevel.ItemsSource = jobLevels;
            WorkType.ItemsSource = workTypes;
            WorkDimensions.ItemsSource = employmentDimensions;

            WorkPosition.DisplayMemberPath = "WorkPosition_Name";
            Category.DisplayMemberPath = "CategoryName";
            WorkLevel.DisplayMemberPath = "LevelName";
            WorkType.DisplayMemberPath = "TypeName";
            WorkDimensions.DisplayMemberPath = "DimensionName";
        }

        private void DisplayMessageForSearchValue(string searchValue)
        {
            SearchBar.Text = searchValue;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allNotifications != null && _filteredNotifications != null)
            {
                string searchText = SearchBar.Text.ToLower();
                _filteredNotifications.Clear();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (var notification in _allNotifications)
                    {
                        _filteredNotifications.Add(notification);
                    }
                }
                else
                {
                    ApplySearchFilter(searchText);
                }
            }
        }

        private void ApplySearchFilter(string searchText)
        {
            _filteredNotifications.Clear();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var notification in _allNotifications)
                {
                    _filteredNotifications.Add(notification);
                }
            }
            else
            {
                foreach (var notification in _allNotifications.Where(n => n.Notification_title.ToLower().Contains(searchText) || n.notification_descript.ToLower().Contains(searchText)))
                {
                    _filteredNotifications.Add(notification);
                }
            }
        }

        private void GoToAnnoucementDetailsPage(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.DataContext is NotificationoOfWork selectedAnnouncement)
                {
                    AnnoucementDetailsPage detailsPage = new AnnoucementDetailsPage(selectedAnnouncement);
                    if (App.Current.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ChangePage(detailsPage);
                    }
                }
            }
        }

        private void BackToMainPage(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ChangePage(new MainPage());
            }
        }

        private void AddFiltersFromCombobox(object sender, RoutedEventArgs e)
        {
            WorkPosition selectedWorkPosition = (WorkPosition)WorkPosition.SelectedItem;
            Category selectedCategory = (Category)Category.SelectedItem;
            JobLevel selectedJobLevel = (JobLevel)WorkLevel.SelectedItem;
            WorkType selectedWorkType = (WorkType)WorkType.SelectedItem;
            EmploymentDimension selectedEmploymentDimension = (EmploymentDimension)WorkDimensions.SelectedItem;

            _filteredNotifications.Clear();

            foreach (var notification in _allNotifications
                .Where(n =>
                    (selectedWorkPosition == null || n.Notification_work_position == selectedWorkPosition.WorkPosition_Id) ||
                    (selectedCategory == null || n.Category == selectedCategory.Category_Id) ||
                    (selectedJobLevel == null || n.Job_level == selectedJobLevel.JobLevel_Id) ||
                    (selectedWorkType == null || n.WorkType == selectedWorkType.WorkType_Id) ||
                    (selectedEmploymentDimension == null || n.Employment_dimensions == selectedEmploymentDimension.EmploymentDimensions_Id))
                )
            {
                _filteredNotifications.Add(notification);
            }
        }

        private void RemoveFiltersFromCombobox(object sender, RoutedEventArgs e)
        {
            WorkPosition.SelectedItem = null;
            Category.SelectedItem = null;
            WorkLevel.SelectedItem = null;
            WorkType.SelectedItem = null;
            WorkDimensions.SelectedItem = null;

            _filteredNotifications.Clear();

            foreach (var notification in _allNotifications)
            {
                _filteredNotifications.Add(notification);
            }

            AnnoucementList.ItemsSource = _filteredNotifications;
        }
    }
}
