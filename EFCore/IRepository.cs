namespace ApiWithUnitTetsting.EFCore;

/// <summary>
/// Defines the members of a repository of entities containing crud-methods and tasks.
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    #region Public Methods

    /// <summary>
    /// Adds a specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    TEntity Add(TEntity entity);

    /// <summary>
    /// Asynchronously adds a specified entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds specified entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to be added.</param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Asynchronously adds specified entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to be added.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns whether any entity is existing in a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns><see langword="true"/> if the sequence contains at least an entity; otherwise <see langword="false"/>.</returns>
    bool Any(IQuerySpecification<TEntity>? querySpecification = null);

    /// <summary>
    /// Asynchronously returns whether any entity is existing in a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> if the sequence contains at least an entity; otherwise <see langword="false"/>.</returns>
    Task<bool> AnyAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns whether any entity is matching the <see cref="IQuerySpecification{T}.CustomSearch"/> in a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <returns><see langword="true"/> if the sequence contains at least an entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/>; otherwise <see langword="false"/>.</returns>
    bool AnySearchMatch(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Asynchronously returns whether any entity is matching the <see cref="IQuerySpecification{T}.CustomSearch"/> in a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> if the sequence contains at least an entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/>; otherwise <see langword="false"/>.</returns>
    Task<bool> AnySearchMatchAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of entities from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The number of entities in the specified sequence.</returns>
    int Count(IQuerySpecification<TEntity>? querySpecification = null);

    /// <summary>
    /// Asynchronously returns the number of entities from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The number of entities in the specified sequence.</returns>
    Task<int> CountAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of entities matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <returns>The number of entities in the specified sequence matching the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    int CountSearchMatches(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Asynchronously returns the number of entities matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The number of entities in the specified sequence matching the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    Task<int> CountSearchMatchesAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default);

    TEntity? Find(object primaryKey);

    /// <summary>
    /// Returns the first entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The first entity from the ordered sequence.</returns>
    TEntity First(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the first entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence with a specified order.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <returns>The first entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from the ordered sequence.</returns>
    TEntity FirstSearchMatch(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters);

    /// <summary>
    /// Returns the index of an entity with a specified primarykey from a specified sequence with a specified order.
    /// </summary>
    /// <param name="entity">The entity to get its index from.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The index of the entity from the ordered sequence; -1, if the entity is not found in the sequence.</returns>
    int IndexOf(TEntity entity, IQuerySpecification<TEntity> querySpecification);

    int IndexOfSearchMatch(TEntity entity, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the entity at a specified zero-based index from the repository with a specified order.
    /// </summary>
    /// <param name="index">The zero-based index of the entity in the sequence.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The entity at the specified index from the ordered sequence.</returns>
    TEntity ItemAt(int index, IQuerySpecification<TEntity> querySpecification);

    TEntity ItemAtSearchMatchIndex(int index, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns all entities matching a specified <see cref="IQuerySpecification{T}"/> without regarding the <see cref="IQuerySpecification{T}.CustomSearch"/>.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/>.</param>
    /// <returns>All entities matching a specified <see cref="IQuerySpecification{T}"/> without regarding the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    IEnumerable<TEntity> Items(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns all entities matching a specified <see cref="IQuerySpecification{T}"/> regarding the <see cref="IQuerySpecification{T}.CustomSearch"/>.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/>.</param>
    /// <returns>All entities matching a specified <see cref="IQuerySpecification{T}"/> regarding the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    IEnumerable<TEntity> ItemsSearchMatching(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the last entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The last entity from the ordered sequence.</returns>
    TEntity Last(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the last entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence with a specified order.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <returns>The last entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from the ordered sequence.</returns>
    TEntity LastSearchMatch(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the number of entities from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The number of entities in the specified sequence.</returns>
    long LongCount(IQuerySpecification<TEntity>? querySpecification = null);

    /// <summary>
    /// Asynchronously returns the number of entities from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The number of entities in the specified sequence.</returns>
    Task<long> LongCountAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the number of entities matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <returns>The number of entities in the specified sequence matching the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    long LongCountSearchMatches(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Asynchronously returns the number of entities matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from a specified sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities and the <see cref="IQuerySpecification{T}.CustomSearch"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The number of entities in the specified sequence matching the <see cref="IQuerySpecification{T}.CustomSearch"/>.</returns>
    Task<long> LongCountSearchMatchesAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the next entity after a specified entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="previousEntity">The entity that is before the result.</param>
    /// <param name="newIndex">Returns the new index of the next entity.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The next entity from the ordered sequence.</returns>
    TEntity Next(TEntity previousEntity, out int newIndex, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the next entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> after a specified entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="previousEntity">The entity that is before the result.</param>
    /// <param name="newSearchMatchIndex">Returns the new search match index of the next entity.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The next entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from the ordered sequence.</returns>
    TEntity NextSearchMatch(TEntity previousEntity, out int newSearchMatchIndex, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the previous entity before a specified entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="nextEntity">The entity that is after the result.</param>
    /// <param name="newIndex">Returns the new index of the next entity.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The previous entity from the ordered sequence.</returns>
    TEntity Previous(TEntity nextEntity, out int newIndex, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the previous entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> before a specified entity from a specified sequence with a specified order.
    /// </summary>
    /// <param name="nextEntity">The entity that is after the result.</param>
    /// <param name="newSearchMatchIndex">Returns the new search match index of the next entity.</param>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The previous entity matching the <see cref="IQuerySpecification{T}.CustomSearch"/> from the ordered sequence.</returns>
    TEntity PreviousSearchMatch(TEntity nextEntity, out int newSearchMatchIndex, IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Removes an existing specified entity.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    void Remove(TEntity? entity);

    /// <summary>
    /// Removes all specified entities in this repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entities">The entities to be deleted.</param>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes all entities matching an <see cref="IQuerySpecification{T}"/>.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the entities to be removed.</param>
    void RemoveRange(IQuerySpecification<TEntity> querySpecification);

    /// <summary>
    /// Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.
    /// </summary>
    /// <param name="querySpecification">The <see cref="IQuerySpecification{T}"/> to specify the sequence of entities.</param>
    /// <returns>The single element of the input sequence.</returns>
    TEntity Single(IQuerySpecification<TEntity>? querySpecification);

    /// <summary>
    /// Updates an existing, specified entity to the database.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <remarks>
    /// The entity on the database will be identified by the primarykey within the specified entity; so an entities primarykey can never be updated / changed with this method.
    /// </remarks>
    void Update(TEntity entity);

    /// <summary>
    /// Updates existing, specified entities to the database.
    /// </summary>
    /// <param name="entities">The entities to be updated.</param>
    void UpdateRange(IEnumerable<TEntity> entities);

    #endregion

    
}