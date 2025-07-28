using BL.DTOConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IUserService
    {
        Task<DTOUserResult> RegisterAsync(DTOUser registerDto);
        Task<DTOUserResult> LoginAsync(DTOUser loginDto);
        Task LogoutAsync();
        Task<DTOUser> GetUserByIdAsync(string userId);
        Task<IEnumerable<DTOUser>> GetAllUsersAsync();
        Guid GetLoggedInUser();
    }
}
