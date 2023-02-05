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
                var result = _employeeUnitofWork.EmployeeRepo.Find(id);
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
                var createdEMployee = _employeeUnitofWork.EmployeeRepo.Add(employee);
                _employeeUnitofWork.SaveChanges();
                return await GetEmployee(createdEMployee.Empid);
                // CreatedAtAction(nameof(GetEmployee), new { createdEMployee.Empid },createdEMployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to insert the employee data " + ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpDateEpmloyee(int id, Employee employee)
        {
            try
            {
                if (id != employee.Empid)
                    return BadRequest("Employee Id Mismatch");
                var result = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x => x.Empid == id);
                if (result == null)
                    return NotFound($"Employee with id ={id} not foound");
                if (result != null)
                {
                    result.FirstName = employee.FirstName;
                    result.LastName = employee.LastName;
                    result.Address = employee.Address;
                    result.CreatedDate = employee.CreatedDate;
                    result.CreatedBy = employee.CreatedBy;
                    result.Alive = employee.Alive;
                    _employeeUnitofWork.EmployeeDbContext.SaveChanges();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to update employee data " + ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {

            try
            {
                var employeeToDelete = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x=>x.Empid==id);
                if (employeeToDelete == null)
                    NotFound($"Record not found with this id={id}");
                if (employeeToDelete != null)
                    _employeeUnitofWork.EmployeeRepo.Remove(employeeToDelete);
               await _employeeUnitofWork.SaveChangesAsync();
                return Ok(employeeToDelete);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting data " + ex.Message);
            }

        }

    }


}
