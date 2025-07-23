using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IBaseService<T>
    {
        List<T> GetAll();
        T GetById(Guid id);
        bool Add(T entity, Guid UserID);
        bool Update(T entity, Guid UserID);
        bool ChangeState(Guid id, int state, Guid UserID);
    }
}
