using AutoMapper;
using BL.Contracts;
using BL.DTOConfiguration;
using BL.Services;
using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BL.Services
{
    public class UserSubscriptionServices : BaseServices<Domains.TbUserSubscription , DTOUserSubscription>, Contracts.IUserSubscription
    {
        public UserSubscriptionServices(ITableRepository<TbUserSubscription> repo , IMapper Mapper, IUserService userService) : base(repo , Mapper, userService)
        {

        }


    }
}

