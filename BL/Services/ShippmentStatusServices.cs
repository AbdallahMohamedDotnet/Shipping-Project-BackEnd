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
    public class ShippmentStatusServices : BaseServices<Domains.TbShipmentStatus , DTOShippmentStatus>, Contracts.IShippmentStatus
    {
        public ShippmentStatusServices(ITableRepository<TbShipmentStatus> repo , IMapper Mapper, IUserService userService) : base(repo, Mapper, userService)
        {
            
        }
    }
}
