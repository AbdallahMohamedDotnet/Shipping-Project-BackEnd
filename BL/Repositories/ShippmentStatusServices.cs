using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class ShippmentStatusServices : BaseServices<Domains.TbShippmentStatus>, Contracts.IShippmentStatus
    {
        public ShippmentStatusServices(ITableRepository<TbShippmentStatus> repo) : base(repo)
        {
            
        }
    }
}
