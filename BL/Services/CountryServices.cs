using Domains;
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
    public class CountryServices : BaseServices<TbCountry, DTOCountry>, ICountry
    {
        public CountryServices(ITableRepository<TbCountry> repo, IMapper mapper) : base(repo, mapper)
        {
        }
    }
}
