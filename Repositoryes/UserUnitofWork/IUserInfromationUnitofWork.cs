using ApiWithUnitTetsting.EFCore;

namespace ApiWithUnitTetsting.Repositoryes
{
    public interface IUserInfromationUnitofWork : IUnitOfWork
    {
        public IUserInfromationRepo UserInfromationRepo { get; }
    }
}
