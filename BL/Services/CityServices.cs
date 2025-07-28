using AutoMapper;
using BL.Contracts;
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
    public class CityServices : BaseServices<TbCity, DTOCity> ,ICity
    {
        private readonly IViewRepository<VwCities> ViewRepo ;
        private readonly IMapper Mapper;
        public CityServices(ITableRepository<TbCity> repo , IMapper Mapper , IUserService userService , IViewRepository<VwCities> ViewRepo) : base(repo , Mapper ,  userService)
        {
            this.Mapper = Mapper;
            this.ViewRepo = ViewRepo;
        }
        public List<DTOCity> GetAllCitites()
        {
            var cities = ViewRepo.GetAll().Where(a => a.CurrentState == 1).ToList();
            return  Mapper.Map<List<VwCities>, List<DTOCity>>(cities);
        }
    }
}
