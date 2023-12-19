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

        public AddAdvertView(User currUser)
        {
            InitializeComponent();
            CurrUser = currUser;

            foreach (var category in categories)
            {
                CatSelectAdd.Items.Add(category.CategoryName);
            }
        }

        private void AddAdvertClick(object sender, RoutedEventArgs e)
        {
            /*try
            {*/
            string advertTitle = advertTitle_TextBox.Text;
            string advertDescribe = advertDescribe_TextBox.Text;
            string workPozition = workPozition_TextBox.Text;
            string workLevel = workLevel_TextBox.Text;
            string contractType = ConcraktTypeComboBox.Text;
            string dimensions = dimensions_TextBox.Text;
            string WorkDays = WorkDays_TextBox.Text;
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
                Notification_work_position = workPozition,
                Job_level = workLevel,
                Contract_type = contractType,
                Employment_dimensions = dimensions,
                Salary_range_start = salaryStart,
                Salary_range_end = salaryEnd,
                Working_days = WorkDays,
                Working_hours_start = TimeSpan.FromHours(workHoursStart),
                Working_hours_end = TimeSpan.FromHours(workHoursEnd),
                Date_of_expiry_start = DateTime.Now,
                Date_of_expiry_end = DateOnly.Parse(expiryAdvertDate),
                Category = categoryId,
                Responsibilities = responsibilities,
                User_Id = CurrUser.ID
            };

            Database.AddAdvertToDatabase(newAdvert, selectedCategoryName);
            MessageBox.Show("Ogłoszenie zostało dodane do bazy danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            /*}
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/

        }
    }
}
