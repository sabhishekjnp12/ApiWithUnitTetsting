using ApiWithUnitTetsting.Entities;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
                var employeeToDelete = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x => x.Empid == id);
                var employeeToDelete = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x=>x.Empid==id);
                if (employeeToDelete == null)
                    NotFound($"Record not found with this id={id}");
                if (employeeToDelete != null)
                    _employeeUnitofWork.EmployeeRepo.Remove(employeeToDelete);
                await _employeeUnitofWork.SaveChangesAsync();
               await _employeeUnitofWork.SaveChangesAsync();
                return Ok(employeeToDelete);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting data " + ex.Message);
            }

        }


        //https://jsonpatch.com/  to learn how to send the data in json patch format.
        [HttpPatch]
        [Route(("{id:int}/UpdatePartial"))]
        public async Task<ActionResult<Employee>> UpdatePartial(int id, [FromBody] JsonPatchDocument<Employee> employeePatch)
        {
            try
            {
                if (id <= 0 || employeePatch == null)
                    return BadRequest($"Please check your Data  and id ={id}");
                var employeeExisting = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x => x.Empid == id);
                if (employeeExisting == null)
                    return NotFound($"Employee with id ={id} not foound");
                var employeeNewpatch = new Employee
                {
                    Empid= employeeExisting.Empid,
                    FirstName = employeeExisting.FirstName,
                    LastName = employeeExisting.LastName,
                    Address = employeeExisting.Address,
                    CreatedDate = employeeExisting.CreatedDate,
                    CreatedBy = employeeExisting.CreatedBy,
                    Alive = employeeExisting.Alive
                    // employeePatch.ap
                };

                employeePatch.ApplyTo(employeeNewpatch, ModelState);
                if(!ModelState.IsValid)
                    return BadRequest($"Please check your ModelState={ModelState}");

                if (employeeExisting != null)
                {
                    employeeExisting.FirstName = employeeNewpatch.FirstName;
                    employeeExisting.LastName = employeeNewpatch.LastName;
                    employeeExisting.Address = employeeNewpatch.Address;
                    employeeExisting.CreatedDate = employeeNewpatch.CreatedDate;
                    employeeExisting.CreatedBy = employeeNewpatch.CreatedBy;
                    employeeExisting.Alive = employeeNewpatch.Alive;
                    _employeeUnitofWork.EmployeeDbContext.SaveChanges();
                    return Ok(employeeExisting);
                }

                /* Pass string in this json fromat [
                    {
                    "path": "/createdBy",
                     "op": "replace",
                     "value": "Admin"
                    },
                    {
                    "path": "/Alive",
                    "op": "replace",
                    "value": "false"
                    }
                ]   */  

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unabble to update employee data " + ex.Message);
            }

            return Ok();
        }

    }


}
