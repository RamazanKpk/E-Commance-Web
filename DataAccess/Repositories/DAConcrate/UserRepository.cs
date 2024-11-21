using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.DAConcrate
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ETicaretContext context): base (context) {
            
        }
        public User LogUser(User user)
        {
            return ETicaretContext.Users.Where(x => x.EmailAddress == user.EmailAddress && x.Password == user.Password).SingleOrDefault();
        }

        public bool CheckUser(string email, string phoneNumber)
        {
            return ETicaretContext.Users.Where(x=> x.EmailAddress ==  email && x.PhoneNumber == phoneNumber ).Any();
        }
        public User GetLogUser(string name)
        {
            return ETicaretContext.Users.Where(x=> x.Name == name).SingleOrDefault();
        }

        public ETicaretContext ETicaretContext { get {
                return _dbContext as ETicaretContext; } }
    }
}
