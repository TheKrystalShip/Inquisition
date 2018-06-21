using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Inquisition.Database.Repositories
{
    public interface IRepository<T>
    {
        event Action<Message> ActionExecuted;
        T SelectFirst(Expression<Func<T, bool>> expression);
        IEnumerable<T> SelectAll();
        IEnumerable<T> SelectWhere(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
