# opa-authz-middleware
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Usage

To use the middleware add this to your startup.cs
```
services.AddBuildAuthorization(options =>
{
    options.BaseAddress = "http://localhost:8181";
    options.PolicyPath = "/v1/data/policy";
    options.AllowOnFailure = false;
});
```

## Build Nuget package
compile and then
```
CONFIGURATION="Release" dotnet pack Source/OPA-AspDotNetCore-Middleware/OPA.AspNetCore.Middleware.csproj
```
