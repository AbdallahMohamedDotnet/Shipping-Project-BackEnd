
using DAL.Exceptions;
using DAL.Repositories;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains;
using BL.Contracts;
using DAL.Contracts;
namespace BL.Repositories
{
    public class ShippingType : BaseServices<TbShippingType> , IShippingType
    {

        public ShippingType(ITableRepository<TbShippingType> repo) : base(repo)
        {

        }
    }


}
