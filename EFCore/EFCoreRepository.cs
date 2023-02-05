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
        //  UnitOfWork = unitOfWork;
        dbContext = unitOfWork.Context;
        //dbContext.ChangeTracker.HasChanges();
        dbSet = unitOfWork.Context.Set<TEntity>();

        //dbContext.ChangeTracker.HasChanges();
        ////unitOfWork.Context.ChangeTracker.Entries();
        ////unitOfWork.Context.ChangeTracker.DetectChanges();// Entries();
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

        //         IEnumerable<TEntity> entitiess = dbContext.ChangeTracker.Entries()
        //.Where(x => x.State != EntityState.Deleted && x.Entity is TEntity).Select(x => x.Entity as TEntity);

        //         IEnumerable<TEntity> entitiess = dbContext.ChangeTracker.Entries()
        //.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted
        //|| x.State == EntityState.Unchanged && x.Entity is TEntity).Select(x => x.Entity as TEntity);

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
        //int indexOfPreviousEntity = IndexOfSearchMatch(previousEntity, querySpecification);
        //newIndex = indexOfPreviousEntity + 1;
        //return GetQuery(querySpecification, true, false, false).AsEnumerable().ElementAt(newIndex);
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

    public virtual void Remove(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        //if (dbSet.Attach(entity).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
        //{
        dbSet.Attach(entity);
        //}
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

            //dbSet = dbContext.Set<TEntity>().AsNoTracking();
            //dbSet = dbContext.
            //   ObjectStateManager.
            //   GetObjectStateEntries(EntityState.Added |
            //                         EntityState.Deleted |
            //                         EntityState.Modified
            //                        );
            //dbContext.ChangeTracker.
            //dbContext.ChangeTracker.Entries().

            //return dbSet.Local.AsQueryable();
            return dbSet;
        }

        // return querySpecification.GetQuery(dbSet.Local, sorted, grouped, searchmatches);
        return querySpecification.GetQuery(dbSet, sorted, grouped, searchmatches);
    }

    #endregion

    //++++++++++++++++++++++++++

    //        #region Constructors

    //        /// <summary>
    //        /// Creates a new instance of the <see cref="EntityRepository{T_context}"/> class
    //        /// with a specified type of <see cref="Microsoft.EntityFrameworkCore._context"/>.
    //        /// </summary>
    //#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    //        public EntityRepository(DbContext context)
    //#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    //        {
    //            this.UnitOfWork = unitOfWork;
    //            this.context = context;
    //            this.dbSet = this.context.Set<TEntity>();
    //        }

    //        #endregion Constructors

    //        public IEnumerable<TEntity> All()
    //        {
    //            DebugHelper.SimpleMessage();
    //            return dbSet.ToList();
    //        }

    //        public async Task<IEnumerable<TEntity>> AllAsync()
    //        {
    //            DebugHelper.SimpleMessage();
    //            return await dbSet.ToListAsync();
    //        }

    //        public virtual TEntity Get(params object[] primaryKeyValues)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return Find(primaryKeyValues);
    //        }

    //        public virtual async Task<TEntity> GetAsync(params object[] primaryKeyValues)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return await FindAsync(primaryKeyValues);
    //        }

    //        public virtual EntityState GetEntityState(TEntity entity)
    //        {
    //            DebugHelper.SimpleMessage();
    //            if (entity == null)
    //                throw new ArgumentNullException(nameof(entity));

    //            object[]? primaryKeyValues = GetPrimaryKeyValues(entity);
    //            if (primaryKeyValues == null)
    //                throw new ArgumentNullException(nameof(primaryKeyValues));

    //            TEntity entityFromDatabase = Find(primaryKeyValues);
    //            if (entityFromDatabase == null)
    //                return EntityState.Deleted;
    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            PropertyInfo timeStampPropertyInfo = entityFromDatabase.GetType().GetProperty("RowVersion");
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    //            if (timeStampPropertyInfo != null)
    //            {
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //                if (timeStampPropertyInfo.GetValue(entity).Equals(timeStampPropertyInfo.GetValue(entityFromDatabase)))
    //                    return EntityState.Unchanged;
    //                else
    //                    return EntityState.Modified;
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //            }
    //            PropertyInfo[] propertyInfos = entityFromDatabase.GetType().GetProperties();
    //            foreach (PropertyInfo propertyInfo in propertyInfos)
    //            {
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //                if (propertyInfo.GetValue(entity).Equals(propertyInfo.GetValue(entityFromDatabase)) == false)
    //                    return EntityState.Modified;
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //            }
    //            return EntityState.Unchanged;
    //        }

    //        public virtual async Task<EntityState> GetEntityStateAsync(TEntity entity)
    //        {
    //            DebugHelper.SimpleMessage();

    //            if (entity == null)
    //                throw new ArgumentNullException(nameof(entity));

    //            object[]? primaryKeyValues = GetPrimaryKeyValues(entity);
    //            if (primaryKeyValues == null)
    //                throw new ArgumentNullException(nameof(primaryKeyValues));
    //#pragma warning disable CS8604 // Possible null reference argument.
    //            TEntity entityFromDatabase = await FindAsync(primaryKeyValues);
    //#pragma warning restore CS8604 // Possible null reference argument.

    //            if (entityFromDatabase == null)
    //                return await Task.FromResult(EntityState.Deleted);

    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            PropertyInfo timeStampPropertyInfo = entityFromDatabase.GetType().GetProperty("RowVersion");
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    //            if (timeStampPropertyInfo != null)
    //            {
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //                if (timeStampPropertyInfo.GetValue(entity).Equals(timeStampPropertyInfo.GetValue(entityFromDatabase)))
    //                    return await Task.FromResult(EntityState.Unchanged);
    //                else
    //                    return await Task.FromResult(EntityState.Modified);
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //            }

    //            PropertyInfo[] propertyInfos = entityFromDatabase.GetType().GetProperties();

    //            foreach (PropertyInfo propertyInfo in propertyInfos)
    //            {
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //                if (propertyInfo.GetValue(entity).Equals(propertyInfo.GetValue(entityFromDatabase)) == false)
    //                    return await Task.FromResult(EntityState.Modified);
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //            }

    //            return await Task.FromResult(EntityState.Unchanged);
    //        }

    //        public virtual bool HasFieldChanged(TEntity entity, string fieldName)
    //        {
    //            DebugHelper.SimpleMessage();
    //            if (entity == null)
    //                throw new ArgumentNullException(nameof(entity));
    //            if (string.IsNullOrWhiteSpace(fieldName))
    //                throw new ArgumentNullException(nameof(fieldName));
    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            object[] primaryKeyValues = GetPrimaryKeyValues(entity);
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(fieldName);
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    //            if (propertyInfo == null)
    //                throw new ArgumentException("The property is not existing.", nameof(fieldName));
    //#pragma warning disable CS8604 // Possible null reference argument.
    //            TEntity entityFromDatabase = Get(primaryKeyValues);
    //#pragma warning restore CS8604 // Possible null reference argument.
    //            if (entityFromDatabase == null)
    //                throw new ArgumentException("An entity with the specified primarykey is not existing.", nameof(entity));
    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //            return !propertyInfo.GetValue(entity).Equals(propertyInfo.GetValue(entityFromDatabase));
    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //        }

    //        public virtual async Task<bool> HasFieldChangedAsync(TEntity entity, string fieldName)
    //        {
    //            DebugHelper.SimpleMessage();

    //            if (entity == null)
    //                throw new ArgumentNullException(nameof(entity));

    //            if (string.IsNullOrWhiteSpace(fieldName))
    //                throw new ArgumentNullException(nameof(fieldName));

    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            object[] primaryKeyValues = GetPrimaryKeyValues(entity);
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    //            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(fieldName);
    //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    //            if (propertyInfo == null)
    //                throw new ArgumentException("The property is not existing.", nameof(fieldName));

    //#pragma warning disable CS8604 // Possible null reference argument.
    //            TEntity entityFromDatabase = await GetAsync(primaryKeyValues);
    //#pragma warning restore CS8604 // Possible null reference argument.

    //            if (entityFromDatabase == null)
    //                throw new ArgumentException("An entity with the specified primarykey is not existing.", nameof(entity));

    //#pragma warning disable CS8602 // Dereference of a possibly null reference.
    //            return await Task.FromResult(result: !propertyInfo.GetValue(entity).Equals(propertyInfo.GetValue(entityFromDatabase)));

    //#pragma warning restore CS8602 // Dereference of a possibly null reference.
    //        }

    //        public virtual Task<TEntity> NextAsync(TEntity previousEntity, out int newIndex, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, bool>>? predicate = null)
    //        {
    //            int? indexOfParam = IndexOf(previousEntity, orderBy, predicate);

    //            //int? indexOfParam =await IndexOfAsync(previousEntity, orderBy, predicate);
    //            if (indexOfParam == null)
    //                throw new ArgumentException(nameof(previousEntity) + " is not contained in the collecion");
    //            Debug.WriteLine("Next of " + indexOfParam.Value);

    //            newIndex = indexOfParam.Value + 1;

    //            //IQueryable<TEntity> LastEntities =await Task.FromResult(GetQueryAsync(orderBy, predicate)).Result;

    //            IQueryable<TEntity> LastEntities = Task.FromResult(GetQuery(orderBy, predicate)).Result;
    //            return Task.FromResult(LastEntities
    //                .AsEnumerable()
    //                .ElementAt(newIndex));

    //            //return await Task.FromResult(LastEntities
    //            //   .AsEnumerable()
    //            //   .ElementAt(index));

    //        }

    //        /// <summary>
    //        /// Returns a page of entities from a specified sequence with a specified order, defined by the index of the page and the paging-size.
    //        /// </summary>
    //        /// <param name="pageIndex">The index of the page.</param>
    //        /// <param name="pageSize">The size of the paging.</param>
    //        /// <param name="orderBy">The order of the sequence.</param>
    //        /// <param name="predicate">The predicate to specify the sequence of entities.</param>
    //        /// <returns>The page of entities.</returns>
    //        public virtual IEnumerable<TEntity> Page(int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, bool>>? predicate)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return GetQuery(orderBy, predicate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
    //        }

    //        public virtual IEnumerable<TEntity> Select(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, Expression<Func<TEntity, bool>>? predicate = null)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return GetQuery(orderBy, predicate).ToList();
    //        }
    //        public virtual async Task<IEnumerable<TEntity>> SelectAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, Expression<Func<TEntity, bool>>? predicate = null)
    //        {
    //            DebugHelper.SimpleMessage();

    //            IQueryable<TEntity> LastEntities = await GetQueryAsync(orderBy, predicate);
    //            return await LastEntities.ToListAsync();// Task.FromResult(LastEntities.ToList());
    //        }

    //        public virtual IEnumerable<TEntity> Select(string sql, params object[] parameters)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return dbSet.FromSqlRaw(sql, parameters).ToList();
    //        }

    //        public virtual async Task<IEnumerable<TEntity>> SelectAsync(string sql, params object[] parameters)
    //        {
    //            DebugHelper.SimpleMessage();

    //            return await dbSet.FromSqlRaw(sql, parameters).ToListAsync();
    //            // Task.FromResult(dbSet.FromSqlRaw(sql, parameters).ToList());
    //        }

    //        public virtual TEntity Find(params object[] primaryKeyValues)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return dbSet.Find(primaryKeyValues);
    //        }

    //        public virtual async Task<TEntity> FindAsync(params object[] primaryKeyValues)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return await dbSet.FindAsync(primaryKeyValues);
    //        }

    //        public virtual TEntity Find(TEntity entity)
    //        {
    //            DebugHelper.SimpleMessage();
    //            return dbSet.Find(GetPrimaryKeyValues(entity));
    //        }

    //        public virtual async Task<TEntity> FindAsync(TEntity entity)
    //        {
    //            DebugHelper.SimpleMessage();

    //#pragma warning disable CS8603 // Possible null reference return.
    //            return await dbSet.FindAsync(GetPrimaryKeyValues(entity));
    //            // Task.FromResult(dbSet.Find(GetPrimaryKeyValues(entity)));
    //#pragma warning restore CS8603 // Possible null reference return.

    //        }

    //        protected async Task<IQueryable<TEntity>> GetQueryAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, Expression<Func<TEntity, bool>>? predicate)
    //        {
    //            DebugHelper.SimpleMessage();

    //            IQueryable<TEntity> result;
    //            if (predicate == null)
    //            {
    //                if (orderBy == null)
    //                    result = await Task.FromResult(dbSet);
    //                else
    //                    result = await Task.FromResult(orderBy(dbSet));
    //            }
    //            else
    //            {
    //                if (orderBy == null)
    //                    result = await Task.FromResult(dbSet.Where(predicate));

    //                else
    //                    result = await Task.FromResult(orderBy(dbSet.Where(predicate)));
    //            }

    //            DebugHelper.EndMessage();

    //            return result;
    //        }

    //        public object[]? GetPrimaryKeyValues(TEntity entity)
    //        {
    //            DebugHelper.SimpleMessage();
    //            if (entity == null)
    //                throw new ArgumentNullException(nameof(entity));
    //            Debug.WriteLine("Entity is of type" + entity.GetType().Name);
    //            var entry = context.Entry(entity);
    //            if (entry == null)
    //            {
    //                DebugHelper.EndMessage("Entry not found");
    //                return null;
    //            }
    //            else
    //            {
    //                Debug.WriteLine("Entry found");
    //            }

    //            IKey primaryKey = entry.Metadata.FindPrimaryKey();
    //            if (primaryKey == null)
    //            {
    //                DebugHelper.EndMessage("primaryKey not found");
    //                return null;
    //            }
    //            else
    //            {
    //                Debug.WriteLine("primaryKey found");

    //            }
    //            DebugHelper.EndMessage();
    //            return primaryKey.Properties.Select(x => x.PropertyInfo.GetValue(entity)).ToArray();
    //        }
}