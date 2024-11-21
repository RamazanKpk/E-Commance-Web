using DataAccess.Repositories.Abstract;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DataAccess.Repositories.DAConcrate
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _dbContext;
        private DbSet<TEntity> _entities;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Delete(int Id)
        {
            var entity = GetById(Id);
            if (entity == null)
            {
                return;
            }
            else
            {
                if (entity.GetType().GetProperty("Deleted") != null)
                {
                    TEntity _entity = entity;
                    _entity.GetType().GetProperty("Deleted").SetValue(_entity, true);
                    this.Update(entity);
                }
                else
                {
                    Delete(entity);
                }
            }
        }
        public void Delete(TEntity entity)
        {
            if(entity.GetType().GetProperty("Deleted") != null)
            {
                TEntity _entity = entity;
                _entity.GetType().GetProperty("Deleted").SetValue(_entity, true);
                Update(entity);
            }
            else
            {
                DbEntityEntry _entityEntry = _dbContext.Entry(entity);
                if (_entityEntry.State != EntityState.Deleted)
                {
                    _entityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _entities.Attach(entity);
                    _entities.Remove(entity);
                }
            }
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public TEntity GetById(int Id)
        {
            return _entities.Find(Id);
        }

        public void Update(TEntity entity)
        {
            var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<TEntity>();
            var entitySet = objectSet.EntitySet;
            var keyMembers = entitySet.ElementType.KeyMembers;

            var keyValues = new object[keyMembers.Count];
            for ( int i = 0; i<keyMembers.Count; i++)
            {
                var keyProperty = typeof(TEntity).GetProperty(keyMembers[i].Name);
                keyValues[i] = keyProperty.GetValue(entity, null);
            }
            var existEntity = _entities.Find(keyValues);
            if (existEntity != null)
            {
                _dbContext.Entry(existEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _entities.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }  
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _entities.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
