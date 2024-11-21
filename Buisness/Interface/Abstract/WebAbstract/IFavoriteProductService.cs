using DataModel.ProductViewModels;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Interface.Abstract.WebAbstract
{
    public interface IFavoriteProductService
    {
        void AddFavoriteProducut(int prductId, int userId);
        IEnumerable<ProductListModel> GetByuserId(int userId);
    }
}
