using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class ShippmentServices : BaseServices<Domains.TbShippment>, BL.Contracts.IShippment
    {
        public ShippmentServices(ITableRepository<TbShippment> repo) : base(repo)
        {
            
        }
    }
}
