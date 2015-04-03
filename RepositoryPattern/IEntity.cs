namespace RepositoryPattern
{
    /// <summary>
    /// Represents an entity in the database.
    /// </summary>
    public interface IEntity<T>: IWriteTo, IReadFrom
    {
        T Id { get; set; }
    }
}
