using DAL.Contracts;
using Domains;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class PaymentMethodServices : BaseServices<Domains.TbPaymentMethod>, BL.Contracts.IPaymentMethodServices
    {
        public PaymentMethodServices(ITableRepository<TbPaymentMethod> repo) : base(repo)
        {
            
        }
    }
}
