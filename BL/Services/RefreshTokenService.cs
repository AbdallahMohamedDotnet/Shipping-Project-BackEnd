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
    public class RefreshTokenService : BaseServices<TbRefreshTokens,DTORefreshToken>,IRefreshTokens
    {
        ITableRepository<TbRefreshTokens> _repo;
        IMapper _mapper;
        public RefreshTokenService(ITableRepository<TbRefreshTokens> repo,IMapper mapper,
            IUserService userService) : base(repo,mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
        }


    }
}
