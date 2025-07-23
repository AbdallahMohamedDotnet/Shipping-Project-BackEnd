using AutoMapper;
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
        public class BaseServices<T,DTO> : IBaseService<T , DTO> where T : BaseTable
        {
            private readonly ITableRepository<T> repo;
            private readonly IMapper mapper;
            public BaseServices(ITableRepository<T> repo , IMapper mapper)
            {
                this.repo = repo;
                this.mapper = mapper;
            }
        
            public List<DTO> GetAll()
            {
                var list = repo.GetAll();
                return mapper.Map<List<T>,List<DTO>>(list).ToList();
            }
        
            public DTO GetById(Guid id)
            {
                var entity = repo.GetById(id);
                return mapper.Map<T,DTO>(entity);
            }
        
            public bool Add(DTO entity, Guid UserID)
            {
                var dbobject = mapper.Map<DTO, T>(entity);
                dbobject.CreatedBy = UserID;
                return repo.Add(dbobject);
            }

            public bool Update(DTO entity, Guid UserID)
            {
                var dbobject = mapper.Map<DTO, T>(entity);
                dbobject.UpdatedBy = UserID;
                return repo.Update(dbobject);
            }

            public bool ChangeState(Guid id, int state, Guid UserID)
            {
                return repo.ChangeState(id, state);
            }
        }
    }




