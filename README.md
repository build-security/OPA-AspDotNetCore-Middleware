# opa-authz-middleware
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Usage

to use add this to your startup.cs
```
services.Configure<OpaAuthzConfiguration>(Configuration.GetSection("OpaAuthorizationMiddleware"));
services.AddMvc().AddMvcOptions(options => options.Filters.Add<OpaAuthorizationMiddleware>());
```

If you use some DI framework you'll have to register the types to their matching interfaces.
for example with Autofac:
```
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ...
            ...
            builder.RegisterType<OpaService>().As<IOpaService>();
            builder.RegisterType<OpaDecideBasic>().As<IOpaDecide>();
        }
```
It's also possible to implement your own version of the interfaces and register them to change the middleware's behaviour.

The configuration part in your appsettings.json
```
  "OpaAuthorizationMiddleware": {
    "BaseAddress": "http://localhost:8181/v1/data/",
    "Enable": true,
    "Timeout": 5,
    "PolicyPath": "build/access",
    "AllowOnFailure": true,
    "IncludeBody": true,
    "IncludeHeaders": true
  }
```

## Build Nuget package
compile and then
```
CONFIGURATION="Release" dotnet pack Source/OPA-AspDotNetCore-Middleware/OPA-AspDotNetCore-Middleware.csproj
```
