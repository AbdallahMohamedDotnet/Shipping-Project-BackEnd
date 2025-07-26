using AutoMapper;
using BL.Contracts;
using DAL.Contracts;
using Domains;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BaseServices<T, DTO> : IBaseService<T, DTO> where T : BaseTable
    {
        private readonly ITableRepository<T> repo;
        private readonly IMapper mapper;
        
        public BaseServices(ITableRepository<T> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public List<DTO> GetAll()
        {
            var list = repo.GetAll();
            return mapper.Map<List<DTO>>(list);
        }

        public DTO GetById(Guid id)
        {
            var entity = repo.GetById(id);
            return mapper.Map<DTO>(entity);
        }

        public bool Add(DTO entity)
        {
            var dbobject = mapper.Map<T>(entity);
            
            dbobject.CreatedDate = DateTime.Now;
            dbobject.CurrentState = 1; // Active by default
            return repo.Add(dbobject);
        }

        public bool Update(DTO entity)
        {
            var dbobject = mapper.Map<T>(entity);
            
            dbobject.UpdatedDate = DateTime.Now;
            return repo.Update(dbobject);
            
        }
        
        public bool ChangeStatus(Guid id, int status = 1)
        {
            return repo.ChangeState(id , status);
        }
    }
}