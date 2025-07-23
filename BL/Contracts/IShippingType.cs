using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains;
namespace BL.Contracts
{
    public interface IShippingType
    {
        List<TbShippingType> GetAll();
        TbShippingType GetById(Guid id);
        bool Add(TbShippingType entity , Guid UserID);
        bool Update(TbShippingType entity , Guid UserID);
        bool ChangeState(Guid id, int state , Guid UserID);

    }

}
