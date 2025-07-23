using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IShippment
    {
        List<TbShippment> GetAll();
        TbShippment GetById(Guid id);
        bool Add(TbShippment entity, Guid UserID);
        bool Update(TbShippment entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
