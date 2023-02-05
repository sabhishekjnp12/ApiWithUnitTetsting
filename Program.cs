using ApiWithUnitTetsting.Context;
using ApiWithUnitTetsting.Handler;
using ApiWithUnitTetsting.Repositoryes;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions ,BasicAuthenticationHandler>("BasicAuthentication",null);
builder.Services.AddCors();
builder.Services.AddDbContext<UnitTestingContext>();
builder.Services.AddSingleton<IUserInfromationUnitofWork,UserInfromationUnitofWork>();
builder.Services.AddSingleton<IEmployeeUnitofWork, EmployeeUnitofWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
