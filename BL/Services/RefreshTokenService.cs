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
    public class RefreshTokenService : BaseServices<TbRefreshTokens,DTORefreshToken>, IRefreshTokens
    {
        ITableRepository<TbRefreshTokens> _repo;
        IMapper _mapper;
        public RefreshTokenService(ITableRepository<TbRefreshTokens> repo,IMapper mapper,
            IUserService userService) : base(repo,mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public DTORefreshToken GetByToken(string token)
        {
            var refreshToken = _repo.GetFirstOrDefault(a => a.Token == token);
            return _mapper.Map<TbRefreshTokens, DTORefreshToken>(refreshToken);
        }

        public bool RefreshTokenExists(DTORefreshToken token)
        {
            var tokenList = _repo.GetList(a => a.UserId == token.UserId && a.CurrentState == 1);
            foreach (var dbToken in tokenList)
            {
                _repo.ChangeStatus(dbToken.Id, Guid.Parse(token.UserId), 2);
            }

            var dbTokens = _mapper.Map<DTORefreshToken, TbRefreshTokens>(token);
            _repo.Add(dbTokens);
            return true;
        }

    }
}
