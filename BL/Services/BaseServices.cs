using AutoMapperCore = AutoMapper;
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
        private readonly AutoMapperCore.IMapper mapper;
        private readonly IUserService userService;
        public BaseServices(ITableRepository<T> repo, AutoMapperCore.IMapper mapper , IUserService userService)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userService = userService;
        }
        //
        public List<DTO> GetAll()
        {
            var list = repo.GetAll().Where(e => e.CurrentState ==1).ToList();
            return mapper.Map<List<T>, List<DTO>>(list);
        }

        //
        public DTO GetById(Guid id)
        {
            var entity = repo.GetById(id);
            return mapper.Map<T, DTO>(entity);
        }


        public bool Add(DTO entity)
        {
            var dbobject = mapper.Map<DTO, T>(entity);
            dbobject.CreatedBy = userService.GetLoggedInUser();
            dbobject.CreatedDate = DateTime.Now;
            dbobject.CurrentState = 1; 
            return repo.Add(dbobject);
        }

        public bool Update(DTO entity)
        {
            var dbobject = mapper.Map<DTO, T>(entity);
            dbobject.UpdatedBy = userService.GetLoggedInUser();
            dbobject.UpdatedDate = DateTime.Now;
            return repo.Update(dbobject);
            
        }

        public bool ChangeStatus(Guid id, int status = 1)
        {
            // Fix: Use proper user ID and correct method name
            var userId = Guid.NewGuid(); // Generate a user ID (should be passed from controller in real scenario)
            return repo.ChangeStatus(id, userService.GetLoggedInUser(), status);
        }
    }
}
