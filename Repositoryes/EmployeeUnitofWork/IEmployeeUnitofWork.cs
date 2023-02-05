using ApiWithUnitTetsting.EFCore;

namespace ApiWithUnitTetsting.Repositoryes
{
    public interface IEmployeeUnitofWork : IUnitOfWork
    {
        public IEmployeeRepo EmployeeRepo { get; }
    }
}
