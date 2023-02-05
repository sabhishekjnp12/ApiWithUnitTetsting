using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiWithUnitTetsting.EFCore;

/// <summary>
/// Base class for a generic repository of entities containing crud-methods and tasks.
/// Implementation for connecting via entity framework core.
/// </summary>
public class EFCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    #region Protected Fields

    protected DbContext dbContext;

    protected DbSet<TEntity> dbSet;

    #endregion

    #region Public Constructors

    public EFCoreRepository(EFCoreUnitOfWorkBase unitOfWork)
    {
        dbContext = unitOfWork.Context;
        dbSet = unitOfWork.Context.Set<TEntity>();
    }

    #endregion

    //  public IUnitOfWork UnitOfWork { get; }

    #region Public Methods

    public virtual TEntity Add(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return dbSet.Add(entity).Entity;
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        dbSet.AddRange(entities);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual bool Any(IQuerySpecification<TEntity>? querySpecification = null)
    {
        if (querySpecification == null || querySpecification.Criteria == null)
            return dbSet.Any();
        return GetQuery(querySpecification, false, false, false).Any();
    }

    public virtual async Task<bool> AnyAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default)
    {
        if (querySpecification == null || querySpecification.Criteria == null)
            return await dbSet.AnyAsync(cancellationToken);
        return await GetQuery(querySpecification, false, false, false).AnyAsync(cancellationToken);
    }

    public bool AnySearchMatch(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, false, false, true).Any();
    }

    public async Task<bool> AnySearchMatchAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default)
    {
        return await GetQuery(querySpecification, false, false, true).AnyAsync(cancellationToken);
    }

    public virtual int Count(IQuerySpecification<TEntity>? querySpecification = null)
    {
        if (querySpecification == null)
            return dbSet.Count();
        return GetQuery(querySpecification, false, false, false).Count() + dbContext.ChangeTracker.Entries<TEntity>().Count(e => e.State == EntityState.Added);
        /// TODO Apply Specification (Critera and Filter if set) on Added count, too
    }

    public virtual async Task<int> CountAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default)
    {
        if (querySpecification == null)
            return await dbSet.CountAsync(cancellationToken);
        return await GetQuery(querySpecification, false, false, false).CountAsync(cancellationToken);
        /// TODO see above, count added, too.
    }

    public int CountSearchMatches(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, false, false, true).Count();
    }

    public async Task<int> CountSearchMatchesAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default)
    {
        return await GetQuery(querySpecification, false, false, true).CountAsync(cancellationToken);
    }

    public virtual TEntity Find(object primaryKey)
    {
        return dbSet.Find(primaryKey);
    }

    public virtual TEntity First(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, true, false, false).First();
    }

    public TEntity FirstSearchMatch(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, true, false, true).First();
    }

    public virtual IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters)
    {
        IQueryable<TEntity> entities = dbSet.FromSqlRaw(sql, parameters);
        return entities;
    }

    public virtual int IndexOf(TEntity entity, IQuerySpecification<TEntity> querySpecification)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return GetQuery(querySpecification, true, false, false).ToList().IndexOf(entity);
    }

    public int IndexOfSearchMatch(TEntity entity, IQuerySpecification<TEntity> querySpecification)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        return GetQuery(querySpecification, true, false, true).ToList().IndexOf(entity);
    }

    public virtual TEntity ItemAt(int index, IQuerySpecification<TEntity> querySpecification)
    {
        if (index == 0)
            return GetQuery(querySpecification, true, false, false).First();
        else
            return GetQuery(querySpecification, true, false, false).Skip(index).First();
    }

    public TEntity ItemAtSearchMatchIndex(int index, IQuerySpecification<TEntity> querySpecification)
    {
        if (index == 0)
            return GetQuery(querySpecification, true, false, true).First();
        else
            return GetQuery(querySpecification, true, false, true).Skip(index).First();
    }

    public IEnumerable<TEntity> Items(IQuerySpecification<TEntity> querySpecification)
    {

        return GetQuery(querySpecification, null, null, false).ToList();
    }

    public IEnumerable<TEntity> ItemsSearchMatching(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, null, null, true).ToList();
    }

    public virtual TEntity Last(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, true, false, false).Last();
    }

    public TEntity LastSearchMatch(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, true, false, true).Last();
    }

    public virtual long LongCount(IQuerySpecification<TEntity>? querySpecification = null)
    {
        if (querySpecification == null)
            return dbSet.LongCount();
        else
            return GetQuery(querySpecification, false, false, false).LongCount();
    }

    public virtual async Task<long> LongCountAsync(IQuerySpecification<TEntity>? querySpecification = null, CancellationToken cancellationToken = default)
    {
        if (querySpecification == null)
            return await dbSet.LongCountAsync(cancellationToken);
        else
            return await GetQuery(querySpecification, false, false, false).LongCountAsync(cancellationToken);
    }

    public long LongCountSearchMatches(IQuerySpecification<TEntity> querySpecification)
    {
        return GetQuery(querySpecification, false, false, true).LongCount();
    }

    public async Task<long> LongCountSearchMatchesAsync(IQuerySpecification<TEntity> querySpecification, CancellationToken cancellationToken = default)
    {
        return await GetQuery(querySpecification, false, false, true).LongCountAsync(cancellationToken);
    }

    public virtual TEntity Next(TEntity previousEntity, out int newIndex, IQuerySpecification<TEntity> querySpecification)
    {
        int indexOfPreviousEntity = IndexOf(previousEntity, querySpecification);
        newIndex = indexOfPreviousEntity + 1;
        return GetQuery(querySpecification, true, false, false).AsEnumerable().ElementAt(newIndex);
    }

    public TEntity NextSearchMatch(TEntity previousEntity, out int newIndex, IQuerySpecification<TEntity> querySpecification)
    {
        throw new NotImplementedException();
    }

    public virtual TEntity Previous(TEntity nextEntity, out int newIndex, IQuerySpecification<TEntity> querySpecification)
    {
        int indexOfNextEntity = IndexOf(nextEntity, querySpecification);
        if (indexOfNextEntity == 0)
            throw new ArgumentException(nameof(nextEntity) + " is the first item in the collecion");
        newIndex = indexOfNextEntity - 1;
        return GetQuery(querySpecification, true, false, false).AsEnumerable().ElementAt(newIndex);
    }

    public TEntity PreviousSearchMatch(TEntity nextEntity, out int newIndex, IQuerySpecification<TEntity> specification)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(TEntity? entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        dbSet.Attach(entity);
        dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));
        dbSet.RemoveRange(entities);
    }

    public virtual void RemoveRange(IQuerySpecification<TEntity> querySpecification)
    {
        if (querySpecification.Criteria == null)
            throw new ArgumentException("querySpecification.Criteria must be set");
        IEnumerable<TEntity> entitiesToRemove = dbSet.Where(querySpecification.Criteria).ToList();
        dbSet.RemoveRange(entitiesToRemove);
    }

    public TEntity Single(IQuerySpecification<TEntity>? querySpecification)
    {
        if (querySpecification == null)
            return dbSet.Single();
        else
            return GetQuery(querySpecification, false, false, false).Single();
    }

    public void Update(TEntity entity)
    {
        EntityEntry entry = dbSet.Attach(entity);
        entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        dbSet.UpdateRange(entities);
    }

    #endregion

    #region Protected Methods

    protected IQueryable<TEntity> GetQuery(IQuerySpecification<TEntity>? querySpecification, bool? sorted, bool? grouped, bool searchmatches)
    {
        //LocalView<TEntity> localDbSet = dbSet.Local;
        if (querySpecification == null)
        {
            if (sorted.HasValue && sorted == true)
                throw new InvalidOperationException();
            if (grouped.HasValue && grouped == true)
                throw new InvalidOperationException();
            if (searchmatches == true)
                throw new InvalidOperationException();

            return dbSet;
        }
        return querySpecification.GetQuery(dbSet, sorted, grouped, searchmatches);
    }

    #endregion

    
}