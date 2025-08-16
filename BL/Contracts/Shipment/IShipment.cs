using BL.Contracts;
using BL.DTOConfiguration;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract
{
    public interface IShipment : IBaseService<TbShipment, DTOShipment>
    {
        public Task Create(DTOShipment DTO);
    }
}
