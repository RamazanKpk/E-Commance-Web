using DataModel.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Interface.Abstract.ApiAbstract
{
    public interface IProductApiService<T>
    {
        List<T> GetProductByCategory(CategoryListModel model);
    }
}
