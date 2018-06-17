
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Inquisition.Database.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DatabaseContext Context { get; set; }

        public Repository(DatabaseContext context)
        {
            Context = context;
        }

        public event Action<Message> ActionExecuted;

        public T SelectFirst(Expression<Func<T, bool>> expression)
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected first {typeof(T)}"));
            return Context.Set<T>().FirstOrDefault(expression);
        }

        public IEnumerable<T> SelectAll()
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected all {typeof(T)}"));
            return Context.Set<T>();
        }

        public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
        {
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Selected {typeof(T)} condition"));
            return Context.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            Context.Set<T>().Add(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Created new {typeof(T)}"));
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Updated {typeof(T)}"));
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), $"Deleted {typeof(T)}"));
        }

        public void Save()
        {
            Context.SaveChanges();
            ActionExecuted?.Invoke(new Message(nameof(Repository<T>), "Saved changes"));
        }        
    }
}
