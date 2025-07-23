using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface ISetting
    {
        List<TbSetting> GetAll();
        TbSetting GetById(Guid id);
        bool Add(TbSetting entity, Guid UserID);
        bool Update(TbSetting entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
