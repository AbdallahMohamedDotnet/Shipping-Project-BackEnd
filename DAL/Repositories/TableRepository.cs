using DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains;
using DAL.Exceptions;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class TableRepository<T> : ITableRepository<T> where T : BaseTable
    {
        private readonly DbContext context;
        private readonly DbSet<T> dbSet;
        private readonly ILogger<TableRepository<T>> Logger;
        
        public TableRepository(DbContext context, ILogger<TableRepository<T>> logger)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            this.Logger = logger;
        }

        public List<T> GetAll()
        {
            try
            {
                return dbSet.ToList();
            }
            catch(Exception ex) 
            {
                throw new DataAccessException(ex, "Error retrieving all entities in Getall()", Logger);
            }

        }
        public T GetById(Guid id)
        {
            try
            {
                return dbSet.Where(e => e.Id == id).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error retrieving entity by ID {id}: {ex.Message}");
                throw new DataAccessException(ex, "Error retrieving entity by ID", Logger);
            }
        }
        public bool Add(T entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = Guid.NewGuid(); // Assuming CreatedBy is set to a new Guid, replace with actual user ID if available
                entity.UpdatedDate = null; // Initial creation does not have an updated date
                entity.UpdatedBy = null; // Initial creation does not have an updated by user
                dbSet.Add(entity);
                return context.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error adding entity: {ex.Message}");
                throw new DataAccessException(ex, "Error adding entity", Logger);
            }

        }
        public bool Update(T entity  )
        {
            try
            {
                var existingEntity = GetById(entity.Id);
                if (existingEntity == null)
                    return false;
                entity.CreatedDate = existingEntity.CreatedDate; // Keep the original creation date
                entity.UpdatedBy = new Guid();
                entity.UpdatedDate = DateTime.Now; // Set the updated date to now
                entity.UpdatedBy = null; // Assuming UpdatedBy is set to a new Guid, replace with actual user ID if available
                context.Entry(existingEntity).State = EntityState.Modified; 
                return context.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error updating entity: {ex.Message}");
                throw new DataAccessException(ex, "Error updating entity", Logger);
            }

        }
        public bool Delete(Guid id)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                    return false;
                dbSet.Remove(entity);
                return context.SaveChanges() > 0;
            }
            catch (DataAccessException ex)
            {
                Logger.LogError($"Error deleting entity with ID {id}: {ex.Message}");
                throw new DataAccessException(ex, "Error deleting entity", Logger);
            }
        }
        public bool ChangeState(Guid id, int state)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                    return false;

                entity.CurrentState = state;
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error changing state for entity with ID {id}: {ex.Message}");
                throw new DataAccessException(ex, "Error changing state for entity", Logger);



        }   }
    }
}