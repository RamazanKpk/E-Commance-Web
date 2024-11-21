using Buisness.Interface.Abstract.WebAbstract;
using DataAccess;
using DataAccess.Context;
using DataModel.ProductViewModels;
using Entity;
using System.Collections.Generic;
using System.Linq;

namespace Buisness.Interface.Service.WebServices.UserFavoriteProductServices
{
    public class UserFavoriteProductService : IFavoriteProductService
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        ProductService productService = new ProductService();
        public void AddFavoriteProducut(int prductId, int userId)
        {
            var favoriteProduct = new UserFavoriteProduct
            {
                ProductId = prductId,
                UserId = userId
            };
            unitOfWork.GetRepository<UserFavoriteProduct>().Add(favoriteProduct);
            unitOfWork.Complate();
        }
        public IEnumerable<ProductListModel> GetByuserId(int userId)
        {
            var favoriteProducts = unitOfWork.GetRepository<UserFavoriteProduct>().GetAll();
            var favoriteProductsByUserId = favoriteProducts.Where(x => x.UserId == userId).ToList();
            var products = productService.GetApiToWeb();
            var favoriteProductsIds = favoriteProductsByUserId.Select(x=> x.ProductId).ToList();
            var productList = products.Where(v=> favoriteProductsIds.Contains(v.Id)).ToList();
            return productList;
        }
    }
}
