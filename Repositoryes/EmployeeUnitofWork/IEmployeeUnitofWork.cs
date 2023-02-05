using ApiWithUnitTetsting.Context;
using ApiWithUnitTetsting.EFCore;
using Microsoft.EntityFrameworkCore;

namespace ApiWithUnitTetsting.Repositoryes
{
    public interface IEmployeeUnitofWork : IUnitOfWork
    {
        public IEmployeeRepo EmployeeRepo { get; }
        public UnitTestingContext EmployeeDbContext { get; }
    }
}
