# opa-authz-middleware
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Usage

To use the middleware add this to your startup.cs
```
services.AddBuildAuthorization(options =>
{
    options.Enable = true;
    options.BaseAddress = "http://localhost:8182";
    options.PolicyPath = "/v1/data/build";
    options.AllowOnFailure = false;
    options.Timeout = 5;
});
```

## Build Nuget package
compile and then
```
CONFIGURATION="Release" dotnet pack Source/OPA-AspDotNetCore-Middleware/OPA.AspNetCore.Middleware.csproj
```
