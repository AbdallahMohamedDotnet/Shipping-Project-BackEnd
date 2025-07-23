using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Contracts;
using DAL.Contracts;
namespace BL.Repositories
{
    public class CountryServices : BaseServices<TbCountry>, ICountry
    {
        public CountryServices(ITableRepository<TbCountry> repo) : base(repo)
        {

        }
    }
}
