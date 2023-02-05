using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithUnitTetsting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfromationController : ControllerBase
    {
        private readonly IUserInfromationUnitofWork _userInfromationUnitofWork;
        public UserInfromationController(IUserInfromationUnitofWork userInfromationUnitofWork) 
        {
            _userInfromationUnitofWork= userInfromationUnitofWork;
        }
        [HttpGet]
        public IActionResult GetUserInfromations()
        {
            var result= new List<UserInformation>();
            result= _userInfromationUnitofWork.UserInfromationRepo.Items(new QuerySpecification<UserInformation>(x => x.OrderBy(c => c.Name))).ToList<UserInformation>();
            if (result.Count == 0)
                return NoContent();
            return Ok(result);
        }
    }
}






