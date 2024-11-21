using Entity;
using System.Collections.Generic;

namespace Buisness.Interface.Abstract.WebAbstract
{
    public interface IUserInfoService <T>
    {
        UserContact UserAdd(T model);
        List<T> CheckRegistered(int userId);
    }
}
