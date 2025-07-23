using Domains;
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
        bool Add(DTO entity, Guid UserID);
        bool Update(DTO entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
