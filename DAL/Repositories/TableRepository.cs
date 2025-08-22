using DAL.Contracts;
using DAL.Exceptions;
using DAL.Models;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public bool Add(T entity , out Guid Id)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                DbSet.Add(entity);
                Context.SaveChanges();
                Id = entity.Id;
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

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            try
            {
                return DbSet.Where(filter).AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        // Method to get a list of records based on a filter
        public List<T> GetList(Expression<Func<T, bool>> filter)
        {
            try
            {
                return DbSet.Where(filter).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }

        public async Task<PagedResult<TResult>> GetPagedList<TResult>(
       int pageNumber,
       int pageSize,
       Expression<Func<T, bool>>? filter = null,
       Expression<Func<T, TResult>>? selector = null,
       Expression<Func<T, object>>? orderBy = null,
       bool isDescending = false,
       params Expression<Func<T, object>>[] includers)
        {
            try
            {
                IQueryable<T> query = DbSet.AsQueryable();

                // Apply includes
                foreach (var include in includers)
                    query = query.Include(include);

                // Apply filter
                if (filter != null)
                    query = query.Where(filter);

                // Total count before pagination
                int totalCount = await query.CountAsync();

                // Apply ordering
                if (orderBy != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                }

                query = query.AsNoTracking();

                // Apply paging
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                // Apply projection
                List<TResult> items;
                if (selector != null)
                    items = await query.Select(selector).ToListAsync();
                else
                    items = await query.Cast<TResult>().ToListAsync();

                // Calculate total pages
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new PagedResult<TResult>
                {
                    Items = items,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", Logger);
            }
        }
    }

}
