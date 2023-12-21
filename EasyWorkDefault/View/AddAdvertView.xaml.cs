using EasyWorkDefault.Classes;
using EasyWorkDefault.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyWorkDefault.View
{
    public partial class AddAdvertView : Page
    {
        public User CurrUser { get; set; }
        List<Category> categories = Database.GetCategoriesFromDatabase();
        List<WorkPosition> workPositions = Database.GetWorkPositionsFromDatabase();
        List<JobLevel> jobLevels = Database.GetJobLevelsFromDatabase();
        List<ContractType> contractTypes = Database.GetContractTypesFromDatabase();
        List<WorkType> workTypes = Database.GetWorkTypesFromDatabase();
        List<EmploymentDimension> Dimensions  = Database.GetEmploymentDimensionsFromDatabase();

        public AddAdvertView(User currUser)
        {
            InitializeComponent();
            CurrUser = currUser;

            foreach (var category in categories)
            {
                var categoryItem = new ComboBoxItem
                {
                    Content = category.CategoryName,
                    Tag = category.Category_Id
                };

                CatSelectAdd.Items.Add(categoryItem);
            }

            foreach (var workPosition in workPositions)
            {
                var workPositionItem = new ComboBoxItem
                {
                    Content = workPosition.WorkPosition_Name,
                    Tag = workPosition.WorkPosition_Id
                };

                workPozition_ComboBox.Items.Add(workPositionItem);
            }

            foreach (var jobLevel in jobLevels)
            {
                var jobLevelItem = new ComboBoxItem
                {
                    Content = jobLevel.LevelName,
                    Tag = jobLevel.JobLevel_Id
                };

                workLevel_ComboBox.Items.Add(jobLevelItem);
            }

            foreach (var contractType in contractTypes)
            {
                var contractTypeItem = new ComboBoxItem
                {
                    Content = contractType.TypeName,
                    Tag = contractType.ContractType_Id
                };

                ConcraktTypeComboBox.Items.Add(contractTypeItem);
            }

            foreach (var workType in workTypes)
            {
                var workTypeItem = new ComboBoxItem
                {
                    Content = workType.TypeName,
                    Tag = workType.WorkType_Id
                };

                workType_ComboBox.Items.Add(workTypeItem);
            }

            foreach (var employmentDimen in Dimensions)
            {
                var employmentDimenItem = new ComboBoxItem
                {
                    Content = employmentDimen.DimensionName,
                    Tag = employmentDimen.EmploymentDimensions_Id
                };

                dimensions_ComboBox.Items.Add(employmentDimenItem);
            }

        }

        private void AddAdvertClick(object sender, RoutedEventArgs e)
        {
            string advertTitle = advertTitle_TextBox.Text;
            string advertDescribe = advertDescribe_TextBox.Text;
            int selectedWorkPositionId = GetSelectedItemId(workPozition_ComboBox);
            int selectedJobLevelId = GetSelectedItemId(workLevel_ComboBox);
            int selectedContractTypeId = GetSelectedItemId(ConcraktTypeComboBox);
            int selectedDimensionsId = GetSelectedItemId(dimensions_ComboBox);
            int selectedWorkTypeId = GetSelectedItemId(workType_ComboBox);
            string WorkDays = WorkDays_ComboBox.Text;
            decimal salaryStart = decimal.Parse(SalaryStart_TextBox.Text);
            decimal salaryEnd = decimal.Parse(SalaryEnd_TextBox.Text);
            int workHoursStart = int.Parse(WorkHoursStart_TextBox.Text);
            int workHoursEnd = int.Parse(WorkHoursEnd_TextBox.Text);
            string expiryAdvertDate = ExpiryAdvertDate_TextBox.Text;
            string responsibilities = Responsibilities_TextBox.Text;

            string selectedCategoryName = string.IsNullOrEmpty(CatNameAdd_TextBox.Text) ? CatSelectAdd.Text : CatNameAdd_TextBox.Text;

            Category existingCategory = Database.GetCategoryByName(selectedCategoryName);

            int categoryId;

            if (existingCategory != null)
            {
                categoryId = existingCategory.Category_Id;
            }
            else
            {
                categoryId = Database.AddCategoryToDatabase(selectedCategoryName);
                MessageBox.Show($"Dodano nową kategorię: {selectedCategoryName}.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                CatSelectAdd.Items.Add(selectedCategoryName);
            }

            NotificationoOfWork newAdvert = new NotificationoOfWork
            {
                Notification_title = advertTitle,
                notification_descript = advertDescribe,
                Notification_work_position = selectedWorkPositionId,
                Job_level = selectedJobLevelId,
                Contract_type = selectedContractTypeId,
                Employment_dimensions = selectedDimensionsId,
                Salary_range_start = salaryStart,
                Salary_range_end = salaryEnd,
                Working_days = WorkDays,
                Working_hours_start = TimeSpan.FromHours(workHoursStart),
                Working_hours_end = TimeSpan.FromHours(workHoursEnd),
                Date_of_expiry_start = DateTime.Now,
                Date_of_expiry_end = DateTime.Parse(expiryAdvertDate),
                Category = categoryId,
                Responsibilities = responsibilities,
                WorkType = selectedWorkTypeId,
                User_Id = CurrUser.ID
            };

            Database.AddAdvertToDatabase(newAdvert, selectedCategoryName, CurrUser);

            MessageBox.Show("Ogłoszenie zostało dodane do bazy danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private int GetSelectedItemId(ComboBox comboBox)
        {
            var selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem != null)
            {
                return (int)selectedItem.Tag;
            }
            return -1;
        }

    }
}
