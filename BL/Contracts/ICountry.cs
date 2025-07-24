using BL.DTOConfiguration;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface ICountry : IBaseService<TbCountry, DTOCountry>
    {

    }
}
