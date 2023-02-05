using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ApiWithUnitTetsting.EFCore;

public abstract class EFCoreUnitOfWorkBase : IUnitOfWork, IDisposable
{
    #region Public Fields

    public readonly DbContext Context;

    #endregion

    #region Private Fields

    private readonly Hashtable repositories;

    private bool _disposed;

    #endregion

    #region Protected Constructors

    protected EFCoreUnitOfWorkBase(DbContext context)
    {
        Context = context;
        repositories = new Hashtable();
    }

    #endregion

    #region Private Destructors

    ~EFCoreUnitOfWorkBase()
    {
        Dispose(disposing: false);
    }

    #endregion

    #region Public Methods

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        string entityTypeName = typeof(TEntity).Name;

        if (!repositories.ContainsKey(entityTypeName))
        {
            Type repositoryType = typeof(EFCoreRepository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), this);

            repositories.Add(entityTypeName, repositoryInstance);
        }
        return repositories[entityTypeName] as IRepository<TEntity>;
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    #endregion

    #region Protected Methods

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Context.Dispose();
                // TODO: dispose (!) managed state (managed objects).
            }
            // free unmanaged resources (unmanaged objects, like open files, open network connections) and override a finalizer below.
            // Mostly nothing to do

            // then set large fields to null.

            _disposed = true;
        }
        // Call the base class implementation if this class is inherited.
        // base.Dispose(disposing);
    }

    #endregion
}