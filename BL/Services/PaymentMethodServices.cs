using AutoMapper;
using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOConfiguration;
namespace BL.Services
{
    public class PaymentMethodServices : BaseServices<Domains.TbPaymentMethod , DTOPaymentMethod>, BL.Contracts.IPaymentMethodServices
    {
        public PaymentMethodServices(ITableRepository<TbPaymentMethod> repo , IMapper Mapper) : base(repo, Mapper)
        {
            
        }
    }
}
