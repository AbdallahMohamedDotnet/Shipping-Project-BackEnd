using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IUserSubscription
    {
        List<TbUserSubscription> GetAll();
        TbUserSubscription GetById(Guid id);
        bool Add(TbUserSubscription entity, Guid UserID);
        bool Update(TbUserSubscription entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
