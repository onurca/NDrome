using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using Ndrome.Model;

namespace Ndrome.Common
{
    public interface IRepository<T> where T : AuditableEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> predicate);

        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Delete(Guid id);
    }

    public class Repository<T> : IRepository<T> where T : AuditableEntity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;

            _dbSet = dbContext.Set<T>();
        }

        #region IRepository Members
        public IQueryable<T> GetAll()
        {
            return _dbSet.Where(x => x.IsDeleted == false).OrderByDescending(x => x.UpdatedDate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Where(x => x.IsDeleted == false);
        }

        public T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public T Create(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (entity.GetType().GetProperty("IsDeleted") != null)
            {
                T _entity = entity;

                _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);

                this.Update(_entity);
            }
            else
            {
                EntityEntry dbEntityEntry = _dbContext.Entry(entity);

                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }

        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                if (entity.GetType().GetProperty("IsDeleted") != null)
                {
                    T _entity = entity;
                    _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);

                    this.Update(_entity);
                }
                else
                {
                    Delete(entity);
                }
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var items = GetAll(predicate);

            foreach (var item in items)
            {
                Delete(item);
            }
        }
        #endregion
    }
}
