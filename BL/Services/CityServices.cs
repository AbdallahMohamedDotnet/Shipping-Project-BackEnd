using AutoMapper;
using BL.DTOConfiguration;
using DAL.Contracts;
using DAL.Repositories;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BL.Services
{
    public class CityServices : BaseServices<Domains.TbCity , DTOCity >, BL.Contracts.ICity
    {

        public CityServices(ITableRepository<TbCity> repo , IMapper Mapper ) : base(repo , Mapper )
        {

        }
        //public List<DTOCity> GetAllCitites()
        //{
        //   // var cities = _ViewRepo.GetAll().Where(a => a.CurrentState == 1).ToList();
        //    //return Mapper.Map<List<VwCities>, List<DTOCity>>(cities);
        //}
    }
}
