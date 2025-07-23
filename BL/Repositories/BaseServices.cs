    using BL.Contracts;
    using DAL.Contracts;
    using Domains;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace BL.Repositories
    {
        public class BaseServices<T> : IBaseService<T> where T : BaseTable
        {
            private readonly ITableRepository<T> repo;
        
            public BaseServices(ITableRepository<T> repo)
            {
                this.repo = repo;
            }
        
            public List<T> GetAll()
            {
                return repo.GetAll();
            }
        
            public T GetById(Guid id)
            {
                return repo.GetById(id);
            }
        
            public bool Add(T entity, Guid UserID)
            {
                entity.CreatedBy = UserID;
                return repo.Add(entity);
        }
        
            public bool Update(T entity, Guid UserID)
            {
                entity.UpdatedBy = UserID;
                return repo.Update(entity);
        }

            public bool ChangeState(Guid id, int state, Guid UserID)
            {
                return repo.ChangeState(id, state);
        }
        }
    }


