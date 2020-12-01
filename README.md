# opa-authz-middleware
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Usage

to use add this to your startup.cs
```
services.Configure<OpaAuthzConfiguration>(Configuration.GetSection("OPAAuthorizationMiddleware"));
services.AddMvc().
    AddMvcOptions(options => { options.Filters.Add(typeof(OpaAuthorizationMiddleware)); });
```

if you use some DI framework like Autofac you might need to add this as well
```
builder.RegisterType<HttpClient>().As<HttpClient>();
```

and the configuration part in your appsettings.json
```
  "OPAAuthorizationMiddleware": {
    "BaseAddress": "http://localhost:8181/v1/data/",
    "PolicyPath": "Default/Policy",
    "AllowOnFailure": true,
    "ServiceId": "myService123"
  }
```

## Build Nuget package
```
dotnet pack Source/OPA-AspDotNetCore-Middleware/OPA-AspDotNetCore-Middleware.csproj
```
