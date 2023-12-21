using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Classes
{
    public class AnnouncementDetails
    {
        public int NotificationOfWorkId { get; set; }
        public string AnnouncementTitle { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
