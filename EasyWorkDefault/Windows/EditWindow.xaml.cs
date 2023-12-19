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
    using System.Windows.Shapes;

    namespace EasyWorkDefault.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public ObservableCollection<LanguageSkill> LanguageSkills { get; set; }
        private int languageSkillCounter = 1;

        public EditWindow(User user)
        {
            LanguageSkills = new ObservableCollection<LanguageSkill>();
            InitializeComponent();
            InitializeLanguageSkillForm();

            userName.Text = user.Name;
            userSurname.Text = user.Surname;
            userEmail.Text = user.Email;
            userBirthDate.Text = user.BirthDate.ToLongDateString();
            userTelumber.Text = user.TelNumber.ToString();
            userResidencePlace.Text = user.ResidencePlace;
            userCurrPosition.Text = user.CurrPosition;
            userCurrPositionDescription.Text = user.CurrPositionDescription;
            userCareerSummary.Text = user.CareerSummary;
            userWorkExperience.Text = user.WorkExperience;
            userEducation.Text = user.Education;

            userSkills.Text = user.Skills;
            userCourses.Text = user.Courses;
        }

        private void AddEditedDataToDatabase(object sender, RoutedEventArgs e)
        {
           
            User updatedUser = new User
            {
                Name = userName.Text,
                Surname = userSurname.Text,
                BirthDate = DateTime.Parse(userBirthDate.Text),
                Email = userEmail.Text,
                TelNumber = int.Parse(userTelumber.Text),
                ResidencePlace = userResidencePlace.Text,
                CurrPosition = userCurrPosition.Text,
                CurrPositionDescription = userCurrPositionDescription.Text,
                CareerSummary = userCareerSummary.Text,
                WorkExperience = userWorkExperience.Text,
                Education = userEducation.Text,
                Skills = userSkills.Text,
                Courses = userCourses.Text
            };

            foreach (var element in languageSkillsStackPanel.Children)
            {
                if (element is StackPanel languageSkillForm)
                {
                    TextBox languageTextBox = (TextBox)languageSkillForm.Children[0];
                    ComboBox levelComboBox = (ComboBox)languageSkillForm.Children[1];

                    updatedUser.LanguageSkills.Add(new LanguageSkill { Language = languageTextBox.Text, Level = levelComboBox.Text });
                }
            }


            Database.UpdateUserData(updatedUser);
            MessageBox.Show("Dane zaktualizowane i zapisane do bazy danych.");
        }

        private void AddLanguageSkill(object sender, RoutedEventArgs e)
        {
            InitializeLanguageSkillForm();
        }

        private void InitializeLanguageSkillForm()
        {

            StackPanel languageSkillForm = new StackPanel { Background = Brushes.AliceBlue };
            TextBox languageSkillTextBox = new TextBox { Name = $"userLanguageSkill{languageSkillCounter}", FontSize = 20 };
            ComboBox languageSkillComboBox = new ComboBox { Name = $"userLanguageSkillLevel{languageSkillCounter}" };
            languageSkillComboBox.Items.Add("poziom A1");
            languageSkillComboBox.Items.Add("poziom A2");
            languageSkillComboBox.Items.Add("poziom B1");
            languageSkillComboBox.Items.Add("poziom B2");
            languageSkillComboBox.Items.Add("poziom C1");
            languageSkillComboBox.Items.Add("poziom C2");

            languageSkillForm.Children.Add(languageSkillTextBox);
            languageSkillForm.Children.Add(languageSkillComboBox);

            if (languageSkillsStackPanel.Children.Count > 0)
            {
                languageSkillsStackPanel.Children.Insert(languageSkillsStackPanel.Children.Count - 1, languageSkillForm);
            }
            else
            {
                languageSkillsStackPanel.Children.Add(languageSkillForm);
            }

            LanguageSkills.Add(new LanguageSkill
            {
                Language = languageSkillTextBox.Text,
                Level = languageSkillComboBox.SelectedItem?.ToString()
            });


            languageSkillCounter++;
        }
    }
}
