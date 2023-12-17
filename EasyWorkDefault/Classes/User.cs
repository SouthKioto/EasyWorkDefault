using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Classes
{
    public class User
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Email { get; set; }
        public int TelNumber { get; set; }
        public Blob ProfImage { get; set; }
        public string? ResidencePlace { get; set; }
        public string? CurrPosition { get; set; }
        public string? CurrPositionDescription { get; set; }
        public string? CareerSummary { get; set; }
        public string? WorkExperience { get; set; }
        public string? Education { get; set; }
        public List<string> LanguageSkills { get; set; }
        public string? Skills { get; set; }
        public string? Courses { get; set; }
        public List<int> Links { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsAdmin { get; set; }

        public User()
        {
            
        }
    }
}
