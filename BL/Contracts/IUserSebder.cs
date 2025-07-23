using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IUserSebder
    {
        List<TbUserSebder> GetAll();
        TbUserSebder GetById(Guid id);
        bool Add(TbUserSebder entity, Guid UserID);
        bool Update(TbUserSebder entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
