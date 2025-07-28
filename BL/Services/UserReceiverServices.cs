using AutoMapper;
using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class UserReceiverServices : BaseServices<TbUserReceiver, DTOUserReceiver>, IUserReceiver
    {
        public UserReceiverServices(ITableRepository<TbUserReceiver> repo, IMapper mapper , IUserService userService) : base(repo, mapper, userService)
        {
        }
    }
}