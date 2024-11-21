using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.DAConcrate
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ETicaretContext context) : base(context)
        {

        }
        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = ETicaretContext.ProductCategories
                .Where(z=>z.ParentId == null)
                .Include(x=> x.ProductCategories1)
                .ToList();
            return productCategories;
        }
        public ETicaretContext ETicaretContext
        {
            get
            {
                return _dbContext as ETicaretContext;
            }
        }
        
    }
}
