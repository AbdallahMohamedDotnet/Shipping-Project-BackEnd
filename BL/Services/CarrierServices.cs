using AutoMapper;
using BL.DTOConfiguration;
using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CarrierServices : BaseServices<Domains.TbCarrier , DTOCarrier> , BL.Contracts.ICarrierServices
    {
        public CarrierServices(ITableRepository<TbCarrier> repo , IMapper mapper) : base(repo, mapper)
        {
            
        }
    }
}


