Scaffold-DbContext "Server=DESKTOP-SFE42UM;Database=UnitTestingDemo;User ID=sa;Password=DBpassword;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -context UnitTestingContext -contextdir Context -force

No on confgire Method 
Scaffold-DbContext "Server=DESKTOP-SFE42UM;Database=UnitTestingDemo;User ID=sa;Password=DBpassword;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -NoOnConfiguring -OutputDir Entities -context UnitTestingContext -contextdir Context -force


Pathc method we can deply to Nuget package manager
1- Microsoft.AspNetCore.Mvc.NewtonsoftJson
2- Microsoft.AspNetCore.JsonPatch

Add Add Program.cs file

a- builder.Services.AddControllers().AddNewtonsoftJson();
b- builder.Services.AddCors();

//https://jsonpatch.com/ how to send the data 
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