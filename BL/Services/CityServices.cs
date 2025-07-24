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
    public class CityServices : BaseServices<Domains.TbCity , DTOCity >, BL.Contracts.ICity
    {
        public CityServices(ITableRepository<TbCity> repo , IMapper Mapper) : base(repo , Mapper)
        {
            
        }
    }
}
