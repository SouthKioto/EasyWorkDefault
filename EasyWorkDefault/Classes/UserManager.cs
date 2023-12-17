using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWorkDefault.Classes
{
    public class UserManager
    {
        public static User CurrentUser { get; set; }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool IsUserLoggedIn()
        {
            return CurrentUser != null;
        }
    }
}
