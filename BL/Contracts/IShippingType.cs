using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOConfiguration;
using Domains;

namespace BL.Contracts
{
    public interface IShippingType : IBaseService<TbShippingType, DTOShippingType>
    {
        // This interface inherits all methods from IBaseService
        // Add shipping type-specific methods here if needed in the future
    }
}
