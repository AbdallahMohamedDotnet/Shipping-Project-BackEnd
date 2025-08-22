using DAL.Models;
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
        public bool Add(T entity, out Guid id);
        bool Update(T entity);
        bool Delete(Guid id);
        bool ChangeStatus(Guid id, Guid userId, int status = 1);
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        public List<T> GetList(Expression<Func<T, bool>> filter);

        Task<PagedResult<TResult>> GetPagedList<TResult>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, TResult>>? selector = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        params Expression<Func<T, object>>[] includers);

    }

}
