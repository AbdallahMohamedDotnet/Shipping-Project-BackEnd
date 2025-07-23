using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface ICity 
    {
        List<TbCity> GetAll();
        TbCity GetById(Guid id);
        bool Add(TbCity entity, Guid UserID);
        bool Update(TbCity entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
