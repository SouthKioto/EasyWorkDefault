using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Classes
{
    public class LikedAnnouncement
    {
        public int UserId { get; set; }
        public int NotificationOfWorkId { get; set; }
        public string NotificationTitle { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
