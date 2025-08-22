using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Models;
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
        public Task Edit(DTOShipment dto);
        public Task<List<DTOShipment>> GetShipments();

        public Task<PagedResult<DTOShipment>> GetShipments(int pageNumber, int pageSize);

        public Task<DTOShipment> GetShipment(Guid id);
    }
}
