using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOConfiguration;
using AutoMapper;
namespace BL.Services
{
    public class SubscriptionPackageServices : BaseServices<Domains.TbSubscriptionPackage, DTOSubscriptionPackage>, Contracts.ISubscriptionPackage
    {
        public SubscriptionPackageServices(ITableRepository<TbSubscriptionPackage> repo , IMapper Mapper) : base(repo, Mapper)
        {
            
        }
    }
}
