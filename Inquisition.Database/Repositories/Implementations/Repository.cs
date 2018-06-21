using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Inquisition.Database.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DatabaseContext _dbContext { get; set; }
        public event Action<Message> ActionExecuted;

        public Repository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T SelectFirst(Expression<Func<T, bool>> expression)
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected first {typeof(T)}"));
            return _dbContext.Set<T>().FirstOrDefault(expression);
        }

        public IEnumerable<T> SelectAll()
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected all {typeof(T)}"));
            return _dbContext.Set<T>();
        }

        public IEnumerable<T> SelectWhere(Expression<Func<T, bool>> expression)
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected {typeof(T)} where condition"));
            return _dbContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Created new {typeof(T)}"));
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Updated {typeof(T)}"));
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Deleted {typeof(T)}"));
        }

        public void Save()
        {
            _dbContext.SaveChanges();
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), "Saved changes"));
        }        
    }
}
