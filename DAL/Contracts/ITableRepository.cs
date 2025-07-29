using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        bool ChangeStatus(Guid id, Guid userId, int status = 1);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        public List<T> GetList(Expression<Func<T, bool>> filter);

    }

}
