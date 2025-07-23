using DAL.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class SettingServices : BaseServices<Domains.TbSetting>, BL.Contracts.ISetting
    {
        public SettingServices(ITableRepository<TbSetting> repo) : base(repo) 
        {
            
        }
    }
}
