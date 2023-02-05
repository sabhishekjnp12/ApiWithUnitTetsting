using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;

namespace ApiWithUnitTetsting.Repositoryes
{
    public class UserInfromationRepo : EFCoreRepository<UserInformation>, IUserInfromationRepo
    {
        public UserInfromationRepo(EFCoreUnitOfWorkBase unitOfWork) : base(unitOfWork)
        {
        }
    }
}
