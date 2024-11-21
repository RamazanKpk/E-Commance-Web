using System.Collections.Generic;

namespace Buisness.Interface.Abstract.WebAbstract
{
    public interface IAllWebService<T>
    {
        List<T> GetApiToWeb();
    }
}
