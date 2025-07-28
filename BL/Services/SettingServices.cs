using AutoMapper;
using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BL.Services
{
    public class SettingServices : BaseServices<Domains.TbSetting , DTOSetting>, BL.Contracts.ISetting
    {
        public SettingServices(ITableRepository<TbSetting> repo, IMapper Mapper, IUserService userService) : base(repo, Mapper, userService) 
        {
            
        }
    }
}
