using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using System.Data.Entity.Validation;
using System.Linq;
using System;
using DataAccess.Repositories.DAConcrate;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ETicaretContext _context;
        private IUserRepository _userRepository;
        private IProductCategoryRepository _productCategoryRepository;
        public UnitOfWork(ETicaretContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(_context));
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                return _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_context));
            }
        }

        public int Complate()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessage);
                var exceptionMessage = $"Doğrulama hatası: {fullErrorMessage}";

                throw new Exception(exceptionMessage, ex);
            }
            catch(Exception ex)
            {
                throw new Exception("Veritabanı İşlemi Sırasında bir hata oluştu.", ex);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context); 
        }
    }
}
