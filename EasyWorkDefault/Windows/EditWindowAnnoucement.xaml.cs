using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EasyWorkDefault.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditWindowAnnoucement.xaml
    /// </summary>
    public partial class EditWindowAnnoucement : Window
    {
        private NotificationoOfWork _editedAnnouncement;

        // Dodaj listy dla ComboBox-ów
        List<Category> categories = Database.GetCategoriesFromDatabase();
        List<WorkPosition> workPositions = Database.GetWorkPositionsFromDatabase();
        List<JobLevel> jobLevels = Database.GetJobLevelsFromDatabase();
        List<ContractType> contractTypes = Database.GetContractTypesFromDatabase();
        List<WorkType> workTypes = Database.GetWorkTypesFromDatabase();
        List<EmploymentDimension> dimensions = Database.GetEmploymentDimensionsFromDatabase();

        public EditWindowAnnoucement(NotificationoOfWork announcement)
        {
            InitializeComponent();

            _editedAnnouncement = announcement;

            TitleTextBox.Text = announcement.Notification_title;
            DescriptionTextBox.Text = announcement.notification_descript;

            foreach (var category in categories)
            {
                var categoryItem = new ComboBoxItem
                {
                    Content = category.CategoryName,
                    Tag = category.Category_Id
                };

                CategoryComboBox.Items.Add(categoryItem);
            }

            foreach (var workPosition in workPositions)
            {
                var workPositionItem = new ComboBoxItem
                {
                    Content = workPosition.WorkPosition_Name,
                    Tag = workPosition.WorkPosition_Id
                };

                PositionComboBox.Items.Add(workPositionItem);
            }

            foreach (var jobLevel in jobLevels)
            {
                var jobLevelItem = new ComboBoxItem
                {
                    Content = jobLevel.LevelName,
                    Tag = jobLevel.JobLevel_Id
                };

                JobLevelComboBox.Items.Add(jobLevelItem);
            }

            foreach (var contractType in contractTypes)
            {
                var contractTypeItem = new ComboBoxItem
                {
                    Content = contractType.TypeName,
                    Tag = contractType.ContractType_Id
                };

                ConctractTypeComboBox.Items.Add(contractTypeItem);
            }

            foreach (var workType in workTypes)
            {
                var workTypeItem = new ComboBoxItem
                {
                    Content = workType.TypeName,
                    Tag = workType.WorkType_Id
                };

                WorkTypeComboBox.Items.Add(workTypeItem);
            }

            foreach (var employmentDimen in dimensions)
            {
                var employmentDimenItem = new ComboBoxItem
                {
                    Content = employmentDimen.DimensionName,
                    Tag = employmentDimen.EmploymentDimensions_Id
                };

                EmploymentDimensionsComboBox.Items.Add(employmentDimenItem);
            }

            SalaryStartTextBox.Text = announcement.Salary_range_start.ToString();
            SalaryEndTextBox.Text = announcement.Salary_range_end.ToString();

            WorkingHoursStart.Text = announcement.Working_hours_start.ToString();
            WorkingHoursEnd.Text = announcement.Working_hours_end.ToString();

            DateOfExpiryStart.Text = announcement.Date_of_expiry_start.ToString();
            DateOfExpiryEnd.Text = announcement.Date_of_expiry_end.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _editedAnnouncement.Notification_title = TitleTextBox.Text;
            _editedAnnouncement.notification_descript = DescriptionTextBox.Text;
            _editedAnnouncement.Category = CategoryComboBox.SelectedIndex;
            _editedAnnouncement.Notification_work_position = PositionComboBox.SelectedIndex;
            _editedAnnouncement.Job_level = JobLevelComboBox.SelectedIndex;
            _editedAnnouncement.Contract_type = ConctractTypeComboBox.SelectedIndex;
            _editedAnnouncement.WorkType = WorkTypeComboBox.SelectedIndex;
            _editedAnnouncement.Employment_dimensions = EmploymentDimensionsComboBox.SelectedIndex;

            _editedAnnouncement.Salary_range_start = decimal.Parse(SalaryStartTextBox.Text);
            _editedAnnouncement.Salary_range_end = decimal.Parse(SalaryEndTextBox.Text);

            _editedAnnouncement.Working_hours_start = TimeSpan.Parse(WorkingHoursStart.Text);
            _editedAnnouncement.Working_hours_end = TimeSpan.Parse(WorkingHoursEnd.Text);

            _editedAnnouncement.Date_of_expiry_start = DateTime.Parse(DateOfExpiryStart.Text);
            _editedAnnouncement.Date_of_expiry_end = DateTime.Parse(DateOfExpiryEnd.Text);

            Data.Database.EditAdvertInDatabase(_editedAnnouncement);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
