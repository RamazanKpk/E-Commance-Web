using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Buisness.SessionAuth
{
    public class SessionManeger
    {
        public static void SetSession(string userName, string role)
        {
            HttpContext.Current.Session["UserName"] = userName;
            HttpContext.Current.Session["Role"] = role;
        }
        public static string GetUserName()
        {
            return HttpContext.Current.Session["UserName"] as string;
        }
        public static string GetRole()
        {
            return HttpContext.Current.Session["Role"] as string;
        }
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
