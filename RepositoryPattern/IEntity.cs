namespace RepositoryPattern
{
    /// <summary>
    /// Represents an entity in the database.
    /// </summary>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
