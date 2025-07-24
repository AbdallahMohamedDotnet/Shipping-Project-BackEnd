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
{    public class UserSebderServices : BaseServices<Domains.TbUserSebder , DTOUserSebder>, Contracts.IUserSebder
    {
        public UserSebderServices(ITableRepository<TbUserSebder> repo , IMapper mapper) : base(repo , mapper)
        {
            
        }
    }

}
