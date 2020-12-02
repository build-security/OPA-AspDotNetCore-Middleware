# opa-authz-middleware
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Usage

to use add this to your startup.cs
```
services.Configure<OpaAuthzConfiguration>(Configuration.GetSection("OPAAuthorizationMiddleware"));
services.AddMvc().
    AddMvcOptions(options => { options.Filters.Add(typeof(OpaAuthorizationMiddleware)); });
```

If you use some DI framework you'll have to register the types to their matching interfaces.
for example with Autofac:
```
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ...
            ...
            builder.RegisterType<HttpClient>().As<HttpClient>();
            builder.RegisterType<OpaService>().As<IOpaService>();
            builder.RegisterType<OpaDecideBasic>().As<IOpaDecide>();
        }
```
It's also possible to implement your own version of the interfaces and register them to change the middleware's behaviour.

The configuration part in your appsettings.json
```
  "OPAAuthorizationMiddleware": {
    "BaseAddress": "http://localhost:8181/v1/data/",
    "PolicyPath": "Default/Policy",
    "AllowOnFailure": true,
    "ServiceId": "myService123"
  }
```

## Build Nuget package
compile and then
```
CONFIGURATION="Release" dotnet pack Source/OPA-AspDotNetCore-Middleware/OPA-AspDotNetCore-Middleware.csproj
```
