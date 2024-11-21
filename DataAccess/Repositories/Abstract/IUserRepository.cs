using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstract
{
    public interface IUserRepository
    {
        User LogUser(User user);
        bool CheckUser(string email, string password);
        User GetLogUser(string name);
    }
}
