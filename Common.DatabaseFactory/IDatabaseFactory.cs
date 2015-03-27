namespace Common.DatabaseFactory
{
    /// <summary>
    /// Represents a database factory used to create an instance of an <see cref="IDatabase"/>.
    /// </summary>
    public interface IDatabaseFactory
    {
        DatabaseFactoryConfigurationSection ConfigurationSection { get; }
        IDatabase CreateInstance();
    }
}