using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiWithUnitTetsting.Handler;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace ApiWithUnitTetsting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeJwtController : ControllerBase
    {
        private readonly IEmployeeUnitofWork _employeeUnitofWork;
        private readonly JwtSettings _jwtSettings;

        public EmployeeJwtController(IEmployeeUnitofWork employeeUnitofWork,IOptions<JwtSettings> options)
        {
            _employeeUnitofWork = employeeUnitofWork;
            _jwtSettings = options.Value;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] EmployeeJwtCredentials employeeJwt)
        {
            var employee = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x => x.FirstName == employeeJwt.FirstName && x.LastName == employeeJwt.LastName);
            if (employee == null)
                return Unauthorized();

            // generate Token

            var tokenHandler=new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new Claim[] { new Claim( ClaimTypes.Name, employee.FirstName) }) ,
                Expires= DateTime.Now.AddSeconds(20),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(tokenkey),SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokendesc);
            string finalTken = tokenHandler.WriteToken(token);
            return Ok(finalTken);
        }

    }
}
