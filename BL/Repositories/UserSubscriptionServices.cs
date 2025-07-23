using BL.Repositories;
using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class UserSubscriptionServices : BaseServices<Domains.TbUserSubscription>, Contracts.IUserSubscription
    {
        public UserSubscriptionServices(ITableRepository<TbUserSubscription> repo) : base(repo)
        {

        }


    }
}

