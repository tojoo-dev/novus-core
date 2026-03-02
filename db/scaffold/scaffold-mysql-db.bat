@REM make sure that EF CLI is installed. if not, then install with: dotnet tool install --global dotnet-ef
@REM add reference to the Microsoft.EntityFrameworkCore.Design within Novus.Api csproj
dotnet ef dbcontext scaffold "server=localhost;Database=employees;Uid=user;Pwd=simplepwd" MySql.EntityFrameworkCore --project ..\..\src\Novus.Core\Novus.Core.csproj --startup-project ..\..\src\Novus.Api\Novus.Api.csproj --output-dir Models --context EmployeesContext --context-dir ./ --data-annotations --verbose
