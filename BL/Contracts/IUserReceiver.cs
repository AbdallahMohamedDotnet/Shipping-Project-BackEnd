using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IUserReceiver
    {
        List<TbUserReceiver> GetAll();
        TbUserReceiver GetById(Guid id);
        bool Add(TbUserReceiver entity, Guid UserID);
        bool Update(TbUserReceiver entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
