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
                Courses = userCourses.Text,
                LanguageSkills = languageSkillsComboBox.Text,
            };

            Database.UpdateUserData(updatedUser);
            MessageBox.Show("Dane zaktualizowane i zapisane do bazy danych.");
        }
    }
}
