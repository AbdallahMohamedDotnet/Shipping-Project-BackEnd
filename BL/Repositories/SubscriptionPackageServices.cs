using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class SubscriptionPackageServices : BaseServices<Domains.TbSubscriptionPackage>, Contracts.ISubscriptionPackage
    {
        public SubscriptionPackageServices(ITableRepository<TbSubscriptionPackage> repo) : base(repo)
        {
            
        }
    }
}
