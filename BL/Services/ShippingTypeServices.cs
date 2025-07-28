using DAL.Exceptions;
using DAL.Repositories;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Contracts;
using DAL.Contracts;
using BL.DTOConfiguration;
using AutoMapper;

namespace BL.Services
{
    public class ShippingTypeServices : BaseServices<TbShippingType, DTOShippingType>, IShippingType
    {
        public ShippingTypeServices(ITableRepository<TbShippingType> repo, IMapper mapper , IUserService userService) : base(repo, mapper, userService)
        {
        }
    }
}
