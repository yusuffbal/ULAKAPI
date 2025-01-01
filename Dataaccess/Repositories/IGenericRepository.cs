using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.Repositories
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);

        IList<T> GetList(Expression<Func<T, bool>> filter = null);

        int AllCount(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] include);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        T GetFirstOrDefault(
            Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] include);

        List<T> GetListByExp(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            string sortField = "",
            string sortOrder = "",
            int pageNumber = -1,
            int pageSize = -1);
    }
}
