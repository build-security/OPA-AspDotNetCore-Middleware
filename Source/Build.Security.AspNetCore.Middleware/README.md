## Build Nuget package
compile and then
```
CONFIGURATION="Release" dotnet build Source/OPA.AspNetCore.Middleware/OPA.AspNetCore.Middleware.csproj \
    && CONFIGURATION="Release" dotnet pack -p:PackageID=OPA-AspDotNetCore-Middleware Source/OPA.AspNetCore.Middleware/OPA.AspNetCore.Middleware.csproj
```
