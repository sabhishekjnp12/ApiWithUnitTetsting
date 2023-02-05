using ApiWithUnitTetsting.EFCore;
using ApiWithUnitTetsting.Entities;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithUnitTetsting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicAuthenticationSampleController : ControllerBase
    {

        private readonly IEmployeeUnitofWork _employeeUnitofWork;

        public BasicAuthenticationSampleController(IEmployeeUnitofWork employeeUnitofWork)
        {
            _employeeUnitofWork = employeeUnitofWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployee()
        {
            try
            {
                var result = _employeeUnitofWork.EmployeeRepo.Items(new QuerySpecification<Employee>(x => x.OrderBy(c => c.Empid))).ToList<Employee>();
                if (result == null)
                    return NotFound();
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to get employee data " + ex.Message);
            }
        }

    }
}
