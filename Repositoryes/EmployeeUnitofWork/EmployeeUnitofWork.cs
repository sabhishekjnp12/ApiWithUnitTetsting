using ApiWithUnitTetsting.Context;
using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiWithUnitTetsting.Repositoryes
{
    public class EmployeeUnitofWork : EFCoreUnitOfWorkBase, IEmployeeUnitofWork
    {
        #region Private Fields

        private UnitTestingContext _employeeDbContext;

        private IEmployeeRepo _employeeRepo;

        #endregion

        #region Public Constructors

        public EmployeeUnitofWork() : base(new UnitTestingContext())
        {
        }

        #endregion

        #region Public Properties

        public UnitTestingContext EmployeeDbContext
        {
            get
            {
                if (this._employeeDbContext == null)
                    this._employeeDbContext = new UnitTestingContext();
                return _employeeDbContext;
            }
        }

        public IEmployeeRepo EmployeeRepo
        {
            get
            {
                if (this._employeeRepo == null)
                    this._employeeRepo = new EmployeeRepo(this);
                return _employeeRepo;
            }
        }

        #endregion

        #region Public Methods

        public override IRepository<TEntity> GetRepository<TEntity>()
        {
            if (typeof(TEntity) == typeof(UserInformation))
                return (IRepository<TEntity>)EmployeeRepo;
            return base.GetRepository<TEntity>();
        }

        #endregion
    }
}