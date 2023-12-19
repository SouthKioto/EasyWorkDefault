using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Classes
{
    public class NotificationoOfWork
    {
        public int NotificationId { get; set; }
        public string? Notification_title { get; set; }
        public string? notification_descript { get; set; }
        public string? Notification_work_position { get; set; }
        public string? Job_level { get; set; }
        public string? Contract_type { get; set; }
        public string? Employment_dimensions { get; set; }
        public decimal Salary_range_start { get; set; }
        public decimal Salary_range_end { get; set; }
        public string Working_days { get; set; }
        public TimeSpan Working_hours_start { get; set; }
        public TimeSpan Working_hours_end { get; set; }
        public DateTime Date_of_expiry_start { get; set; }
        public DateOnly Date_of_expiry_end { get; set; }
        public int Category { get; set; }
        public string? Responsibilities { get; set; }
        public string? Candidate_requirements { get; set; }
        public string? Employer_offers { get; set; }
        public string? About_the_company { get; set; }
        public int User_Id { get; set; }

        public NotificationoOfWork()
        {

        }
    }
}
