using BL.Services;
using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOConfiguration;
using AutoMapper;
namespace BL.Services
{
    public class UserSubscriptionServices : BaseServices<Domains.TbUserSubscription , DTOUserSubscription>, Contracts.IUserSubscription
    {
        public UserSubscriptionServices(ITableRepository<TbUserSubscription> repo , IMapper Mapper) : base(repo , Mapper)
        {

        }


    }
}

