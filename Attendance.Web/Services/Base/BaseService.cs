using Attendance.Models;
using Attendance.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Attendance.Web.Services.Base
{
    public class Repository<T> where T : BaseEntity
    {
        private readonly DatabaseContext context;
        private IDbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(DatabaseContext context)
        {
            this.context = context;
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public IEnumerable<T> Get()
        {
            return this.Entities.Where(e => !e.IsDeleted);
        }
        
        public IEnumerable<T> Get(Expression<Func<T,bool>> expression)
        {
            return this.Entities.Where(e => !e.IsDeleted).Where(expression);
        }

        public IEnumerable<T> Get(Expression<Func<T,bool>> expression,string include)
        {
            IQueryable<T> query=this.Entities;
            foreach (var item in include.Split(','))
            {
                query.Include(item);
            }
            return query.Where(e => !e.IsDeleted).Where(expression);
        }
        
        public IEnumerable<T> GetSorted<TKey>(Expression<Func<T, bool>> expression,Expression<Func<T,TKey>> sort,bool desc=true)
        {
            if(desc) return this.Entities.Where(e => !e.IsDeleted).Where(expression).OrderByDescending(sort);
            else return this.Entities.Where(e => !e.IsDeleted).Where(expression).OrderBy(sort);
        }
        public IEnumerable<T> GetSorted<TKey>(Expression<Func<T, TKey>> sort, bool desc=true)
        {
            if(desc) return this.Entities.Where(e => !e.IsDeleted).OrderByDescending(sort);
            else return this.Entities.Where(e => !e.IsDeleted).OrderBy(sort);
        }
        
        public IEnumerable<T> Get(Expression<Func<T,bool>> expression,int pageSize,int pageIndex)
        {
            return this.Entities.Where(e => !e.IsDeleted).Where(expression).Take(pageSize*pageIndex).Skip(pageSize);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity.Id = Guid.NewGuid();
                entity.CreationDate = DateTime.Now;
                entity.IsDeleted = false;
                this.Entities.Add(entity);
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity.LastModifiedDate = DateTime.Now;
                this.context.Entry(entity).State = EntityState.Modified;
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                throw new Exception(errorMessage, dbEx);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                entity.DeletionDate = DateTime.Now;
                entity.IsDeleted = true; 
                this.context.Entry(entity).State = EntityState.Modified;
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                } 
            } 
        }
        public void Delete(object id)
        {
            Delete(GetById(id));
        }
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        private IDbSet<T> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = context.Set<T>();
                }
                return entities;
            }
        }
    }

}