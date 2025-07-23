using BL.DTOConfiguration;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface ICarrierServices : IBaseService<TbCarrier, DTOCarrier>
    {
        List<TbCarrier> GetAll();
        TbCarrier GetById(Guid id);
        bool Add(TbCarrier entity, Guid UserID);
        bool Update(TbCarrier entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);

    }
}
