using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Core.Entities;

namespace Dataaccess.Repositories
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()
    {
        public TEntity Add(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            context.SaveChanges();

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            using var context = new TContext();
            var updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            context.SaveChanges();

            return entity;
        }

        public void Delete(TEntity entity)
        {
            using var context = new TContext();
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            return filter == null
                ? context.Set<TEntity>().ToList()
                : context.Set<TEntity>().Where(filter).ToList();
        }

        public int AllCount(Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] include)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = query.AsNoTracking();

            if (include != null)
                foreach (var inc in include)
                    query = query.Include(inc);

            if (predicate != null) query = query.Where(predicate);

            return query.Distinct().AsNoTracking().Count();
        }

        public TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] include)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            query = query.AsNoTracking();

            if (include != null)
                foreach (var inc in include)
                    query = query.Include(inc);

            if (predicate != null) query = query.Where(predicate);

            return query.FirstOrDefault();
        }

        public List<TEntity> GetListByExp(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            string sortField = "",
            string sortOrder = "asc",
            int pageNumber = -1,
            int pageSize = -1)
        {
            using var context = new TContext();
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(sortField))
                query = OrderBy(query, char.ToUpper(sortField[0]) + sortField.Substring(1), sortOrder != "asc");

            if (pageNumber != -1) query = query.Skip((pageNumber - 1) * pageSize);

            if (pageSize != -1) query = query.Take(pageSize);

            return query.ToList();
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            var command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty.Replace('İ', 'I'));
            if (property != null)
            {
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), command,
                    new[] { type, property.PropertyType },
                    source.AsQueryable().Expression,
                    Expression.Quote(orderByExpression));
                return source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);
            }

            return source;
        }
    }
}

