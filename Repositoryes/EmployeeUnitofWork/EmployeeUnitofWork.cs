using ApiWithUnitTetsting.Context;
using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;

namespace ApiWithUnitTetsting.Repositoryes
{
    public class EmployeeUnitofWork : EFCoreUnitOfWorkBase, IEmployeeUnitofWork
    {
        #region Private Fields

        private IEmployeeRepo _employeeRepo;

        #endregion

        #region Public Constructors

        public EmployeeUnitofWork() : base(new UnitTestingContext())
        {
        }

        #endregion

        #region Public Properties

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