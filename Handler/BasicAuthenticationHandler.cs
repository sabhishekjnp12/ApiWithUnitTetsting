
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using System.Text;
using ApiWithUnitTetsting.Repositoryes;
using System.Security.Claims;
namespace ApiWithUnitTetsting.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IEmployeeUnitofWork _employeeUnitofWork;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IEmployeeUnitofWork employeeUnitofWork) : base(options, logger, encoder, clock)
        {
            _employeeUnitofWork= employeeUnitofWork;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail(" No Header found"));

            var headervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(headervalue.Parameter);
            string credentials=Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(':');
                string userName = array[0];
                string password = array[1];
                var user = _employeeUnitofWork.EmployeeDbContext.Employees.FirstOrDefault(x => x.FirstName == userName && x.LastName == array[1]);
                if (user == null) 
                    return Task.FromResult(AuthenticateResult.Fail("Un authorized"));

                var claims = new[] { new Claim(ClaimTypes.Name, userName) };
                var identity=new ClaimsIdentity(claims,Scheme.Name);
                var prinicpal=new ClaimsPrincipal(identity);
                var token=new AuthenticationTicket(prinicpal,Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(token));
            }
            return Task.FromResult(AuthenticateResult.Fail(" no user found"));

        }
    }
}
