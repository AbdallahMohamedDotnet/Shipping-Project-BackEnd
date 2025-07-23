using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class CityServices : BaseServices<Domains.TbCity>, BL.Contracts.ICity
    {
        public CityServices(ITableRepository<TbCity> repo) : base(repo)
        {
            
        }
    }
}
