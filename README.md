# dotnet-captcha
dotnet 5 | captcha | drawing

```
# .\src\Application
dotnet new classlib -o .\src\Core\Application -f net5.0
dotnet add .\src\Core\Application package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add .\src\Core\Application package System.Drawing.Common
rm .\src\Core\Application\Class1.cs
mkdir .\src\Core\Application\Common
mkdir .\src\Core\Application\Repositories
mkdir .\src\Core\Application\Services
echo 'namespace Application.Common { }' > .\src\Core\Application\Common\Exceptions.cs
echo 'namespace Application.Common { }' > .\src\Core\Application\Common\IDbContext.cs
echo 'namespace Application.Common { }' > .\src\Core\Application\Common\IRepository.cs
echo 'namespace Application.Services { }' > .\src\Core\Application\Services\CaptchaService.cs
echo 'namespace Application { }' > .\src\Core\Application\DependencyInjection.cs

# .\src\Presentation
dotnet new mvc -o .\src\Presentation\WebApp -f net5.0
dotnet add .\src\Presentation\WebApp reference .\src\Core\Application

dotnet new sln
dotnet sln add (ls -r .\**\*.csproj)
```