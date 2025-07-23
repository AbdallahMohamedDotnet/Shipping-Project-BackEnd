using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IShippmentStatus
    {
        List<TbShippmentStatus> GetAll();
        TbShippmentStatus GetById(Guid id);
        bool Add(TbShippmentStatus entity, Guid UserID);
        bool Update(TbShippmentStatus entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
