using System.Collections.Generic;

namespace Common.Repository
{
    /// <summary>
    /// Represents a repository for interacting with a database instance.
    /// </summary>
    /// <typeparam name="T">Type of object to return.</typeparam>
    public interface IRepository<T>
    {
        IEnumerable<T> List { get; }
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T FindById(int id);
    }
}
