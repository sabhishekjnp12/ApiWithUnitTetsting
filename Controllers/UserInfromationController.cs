using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithUnitTetsting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfromationController : ControllerBase
    {
        #region Private Fields

        private readonly IUserInfromationUnitofWork _userInfromationUnitofWork;

        #endregion

        #region Public Constructors

        public UserInfromationController(IUserInfromationUnitofWork userInfromationUnitofWork)
        {
            _userInfromationUnitofWork = userInfromationUnitofWork;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IActionResult GetUserInfromations()
        {
            var result = new List<UserInformation>();
            result = _userInfromationUnitofWork.UserInfromationRepo.Items(new QuerySpecification<UserInformation>(x => x.OrderBy(c => c.Name))).ToList<UserInformation>();
            if (result.Count == 0)
                return NoContent();
            return Ok(result);
        }

        #endregion
    }
}