using DAL.Exceptions;
using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IBaseService<T,DTO>
    {
        List<DTO> GetAll();
        DTO GetById(Guid id);
        bool Add(DTO entity);
        public bool Add(DTO entity, out Guid Id);
        bool Update(DTO entity);
        bool ChangeStatus(Guid id, int status );
        

    }
}
