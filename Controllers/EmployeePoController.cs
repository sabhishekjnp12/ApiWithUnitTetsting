using ApiWithUnitTetsting.Entities;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithUnitTetsting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePoController : ControllerBase
    {
        private readonly IEmployeeUnitofWork _employeeUnitofWork;

        public EmployeePoController(IEmployeeUnitofWork employeeUnitofWork)
        {
            _employeeUnitofWork = employeeUnitofWork;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result =  _employeeUnitofWork.EmployeeRepo.Find(id);
                if (result == null)
                    return NotFound();
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to get employee data " + ex.Message);
            }
        }



        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try

            {
                if (employee == null)
                    return BadRequest();
                 var createdEMployee =_employeeUnitofWork.EmployeeRepo.Add(employee);
                 _employeeUnitofWork.SaveChanges();
                return await GetEmployee(createdEMployee.Empid);  // CreatedAtAction(nameof(GetEmployee), new { createdEMployee.Empid },createdEMployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to insert the employee data " + ex.Message);
            }
        }

    }


}
