using ApiWithUnitTetsting.Context;
using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;

namespace ApiWithUnitTetsting.Repositoryes
{
    public class UserInfromationUnitofWork : EFCoreUnitOfWorkBase, IUserInfromationUnitofWork
    {
        #region Private Fields

        private IUserInfromationRepo _userInfromationRepo;

        #endregion

        #region Public Constructors

        public UserInfromationUnitofWork() : base(new UnitTestingContext())
        {
        }

        #endregion

        #region Public Properties

        public IUserInfromationRepo UserInfromationRepo
        {
            get
            {
                if (this._userInfromationRepo == null)
                    this._userInfromationRepo = new UserInfromationRepo(this);
                return _userInfromationRepo;
            }
        }

        #endregion

        #region Public Methods

        public override IRepository<TEntity> GetRepository<TEntity>()
        {
            if (typeof(TEntity) == typeof(UserInformation))
                return (IRepository<TEntity>)UserInfromationRepo;
            return base.GetRepository<TEntity>();
        }

        #endregion
    }
}