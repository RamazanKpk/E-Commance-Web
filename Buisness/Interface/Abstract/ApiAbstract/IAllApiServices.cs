using System.Collections.Generic;

namespace Buisness.Interface.Abstract.ApiAbstract
{
    public interface IAllApiServices<T>
    {
        List<T> GetList();
    }
}
