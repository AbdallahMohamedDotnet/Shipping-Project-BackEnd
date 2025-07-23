using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IPaymentMethodServices
    {
        List<TbPaymentMethod> GetAll();
        TbPaymentMethod GetById(Guid id);
        bool Add(TbPaymentMethod entity, Guid UserID);
        bool Update(TbPaymentMethod entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
