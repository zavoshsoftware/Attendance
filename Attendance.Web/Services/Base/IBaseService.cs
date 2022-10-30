using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Web.Services.Base
{

    public interface IBaseService<T> : IRepository where T : class
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void SoftDelete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> Get(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
            T GetWithoutDeleted(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where, List<string> includeList);
        bool IsExistAny(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        int Count(Expression<Func<T, bool>> exp);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, List<string> includeList);
        IQueryable<T> GetManyAsQueryable(Expression<Func<T, bool>> where);
        List<T> GetManyWithSortAsQueryable(Expression<Func<T, bool>> where);
        IQueryable<T> GetManyAsQueryable(Expression<Func<T, bool>> where, List<string> includList);
        IQueryable<T> GetAllAsQueryable();
        IQueryable<T> TableNoTracking { get; }
    }
    public interface IRepository
    {
    }
}