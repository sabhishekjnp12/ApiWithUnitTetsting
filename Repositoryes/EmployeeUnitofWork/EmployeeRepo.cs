using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;

namespace ApiWithUnitTetsting.Repositoryes
{
    public class EmployeeRepo : EFCoreRepository<Employee>, IEmployeeRepo
    {
        public EmployeeRepo(EFCoreUnitOfWorkBase unitOfWork) : base(unitOfWork)
        {
        }
    }
}
