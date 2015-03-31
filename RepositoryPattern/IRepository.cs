using System.Collections.Generic;

namespace RepositoryPattern
{
    /// <summary>
    /// Represents a repository for interacting with a database instance.
    /// </summary>
    /// <typeparam name="T">Type of object to return.</typeparam>
    public interface IRepository<T>
    {
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        T GetById(int id);
    }
}
