using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Linq;
using Ndrome.Data;
using Ndrome.Model;

namespace Ndrome.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : AuditableEntity;
        bool BeginNewTransaction();
        bool RollBackTransaction();
        int SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction _transation;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region IUnitOfWork Members
        public IRepository<T> GetRepository<T>() where T : AuditableEntity
        {
           return new Repository<T>(_dbContext);
        }

        public bool BeginNewTransaction()
        {
            try
            {
                _transation = _dbContext.Database.BeginTransaction();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RollBackTransaction()
        {
            try
            {
                _transation.Rollback();
                _transation = null;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int SaveChanges()
        {
            var transaction = _transation != null ? _transation : _dbContext.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    if (_dbContext == null)
                    {
                        throw new ArgumentException("Context is null");
                    }
                    int result = _dbContext.SaveChanges();

                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }
        #endregion

        #region IDisposable Members
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
