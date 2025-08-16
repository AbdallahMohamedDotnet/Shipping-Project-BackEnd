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
{    public class UserSenderServices : BaseServices<Domains.TbUserSender, DTOUserSender>, Contracts.IUserSender
    {
        public UserSenderServices(ITableRepository<TbUserSender> repo , IMapper mapper , IUserService userService) : base(repo , mapper, userService)
        {
            
        }
    }

}
