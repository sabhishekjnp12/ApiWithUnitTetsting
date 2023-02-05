using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;

namespace ApiWithUnitTetsting.Repositoryes
{
    public interface IEmployeeRepo : IRepository<Employee>
    {
    }
}