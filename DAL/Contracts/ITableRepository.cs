using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface ITableRepository<T> where T : BaseTable
    { 
        List<T> GetAll();
        T GetById(Guid id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(Guid id);
        bool ChangeState(Guid id, int state);

    }

}
