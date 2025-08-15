using AutoMapper;
using BL.Contracts;
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
    public class ShippmentServices : BaseServices<Domains.TbShipment , DTOShippment>, BL.Contracts.IShippment
    {
        public ShippmentServices(ITableRepository<TbShipment> repo , IMapper Mapper, IUserService userService) : base(repo , Mapper, userService)
        {
            
        }
    }
}
