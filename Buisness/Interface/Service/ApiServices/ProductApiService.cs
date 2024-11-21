using DataAccess.Context;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DataModel.ProductViewModels;
using Buisness.Interface.Abstract.ApiAbstract;
using DataModel.ImageViewModels;
using DataModel.CategoryViewModels;

namespace Buisness.Interface.Service.ApiServices
{
    public class ProductApiService : IAllApiServices<ProductListModel>, IProductApiService<ProductListModel>
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public List<ProductListModel> GetList()
        {
            var product = unitOfWork.GetRepository<Product>().GetAll()
                .Where(c=> c.IsActive)
                .Select(x => new ProductListModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Title,
                    CategoryId = x.CategoryId,
                    SalePrice = x.SalePrice,
                    StockAmount = x.StockAmount,
                    VatRate = x.VatRate,
                    Images = unitOfWork.GetRepository<ProductImage>().GetAll()
                    .Where(b=> b.IsDefaultImage && b.ProductId == x.Id)
                    .Select(a=> new ImageListModel
                    {
                        ImageUrl = a.ImagePath,
                    }).ToList(),
                    BrandId = x.ProductCategory.BrandId,
                }).ToList();
            return product;
        }
        public List<ProductListModel> GetProductByCategory(CategoryListModel model)
        {
            var category = unitOfWork.GetRepository<ProductCategory>().GetById(model.Id);
            var subCategory = GetSubCategoriesRecursive(category.Id);

            List<ProductListModel> productLists = GetProductsByCategories(subCategory);
            return productLists;
        }
        private List<int> GetSubCategoriesRecursive(int parentId)
        {
            var subCategories = unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(c => c.ParentId == parentId)
                .Select(c => c.Id).ToList();
            var subSubCategories = new List<int>();
            foreach (var subCategoryId in subCategories)
            {
                subSubCategories.AddRange(GetSubCategoriesRecursive(subCategoryId));
            }
            subCategories.AddRange(subSubCategories);
            return subCategories;
        }
        private List<ProductListModel> GetProductsByCategories(List<int> categoryIds)
        {
            var categoryies = unitOfWork.GetRepository<Product>().GetAll()
                .Where(x => x.IsActive && categoryIds.Contains(x.CategoryId.Value))
                .Select(v => new ProductListModel
                {
                    Id = v.Id,
                    Name = v.Title,
                    CategoryId = v.CategoryId.Value,
                    SalePrice = v.SalePrice,
                    StockAmount = v.StockAmount,
                    Description = v.Description,
                    VatRate = v.VatRate,
                    Images = unitOfWork.GetRepository<ProductImage>().GetAll()
                    .Where(c => c.ProductId == v.Id && c.IsDefaultImage)
                    .Select(b => new ImageListModel
                    {
                        ImageUrl = b.ImagePath,
                    }).ToList(),
                    BrandId = v.ProductCategory.BrandId,

                }).ToList();
            return categoryies;
        }
    }
}
