using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Exceptions;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class TableRepository<T> : ITableRepository<T> where T : BaseTable
    {
        private readonly ShippingContext Context;
        private readonly DbSet<T> DbSet;
        private readonly ILogger<TableRepository<T>> Logger;
       
        public TableRepository(ShippingContext context, ILogger<TableRepository<T>> log)
        {
            this.Context = context;
            this.DbSet = Context.Set<T>();
            this.Logger = log;
        }

        public List<T> GetAll()
        {
            try
            {
                return DbSet.Where(a => a.CurrentState > 0).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public T GetById(Guid id)
        {
            try
            {
                return DbSet.Where(a => a.Id == id).AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public bool Add(T entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                DbSet.Add(entity);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public bool Update(T entity)
        {
            try
            {
                var dbData = GetById(entity.Id);
                entity.CreatedDate = dbData.CreatedDate;
                entity.CreatedBy = dbData.CreatedBy;
                entity.UpdatedDate = DateTime.Now;
                entity.CurrentState = dbData.CurrentState;
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    DbSet.Remove(entity);
                    Context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public bool ChangeStatus(Guid id, Guid userId, int status = 1)
        {
            try
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    entity.CurrentState = status;
                    entity.UpdatedBy = userId;
                    entity.UpdatedDate = DateTime.Now;
                    Context.Entry(entity).State = EntityState.Modified;
                    Context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }
    }

}
