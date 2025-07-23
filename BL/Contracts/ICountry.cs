using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface ICountry 
    {
        List<TbCountry> GetAll();
        TbCountry GetById(Guid id);
        bool Add(TbCountry entity, Guid UserID);
        bool Update(TbCountry entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
