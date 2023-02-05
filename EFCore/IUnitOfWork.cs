namespace ApiWithUnitTetsting.EFCore;

/// <summary>
/// Defines a Unit of Work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region Public Methods

    /// <summary>
    /// Returns the generic repository of a specified entities type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities to be handled by the repository.</typeparam>
    /// <returns>The generic repository of the specified entities type.</returns>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    /// <summary>
    /// Saves all changes made in the Unit of Work.
    /// </summary>
    /// <returns>The number of changed entities in the database.</returns>
    int SaveChanges();

    /// <summary>
    /// Saves all changes made in the Unit of Work.
    /// </summary>
    /// <returns>The number of changed entities in the database.</returns>
    Task<int> SaveChangesAsync();

    #endregion
}