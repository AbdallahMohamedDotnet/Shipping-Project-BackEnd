using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{    public class UserSebderServices : BaseServices<Domains.TbUserSebder>, Contracts.IUserSebder
    {
        public UserSebderServices(ITableRepository<TbUserSebder> repo) : base(repo)
        {
            
        }
    }

}
