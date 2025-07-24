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
        // This interface inherits all methods from IBaseService
        // Add carrier-specific methods here if needed in the future
    }
}
