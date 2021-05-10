# opa-authz-middleware
## Abstract
OPA-AspDotNetCore-Middleware is a .net middleware meant for authorizing API requests using a 3rd party policy engine (OPA) as the Policy Decision Point (PDP).
If you're not familiar with OPA, please [learn more](https://www.openpolicyagent.org/).
ASP.NET Core Authorization Middleware that consults with an Open Policy Agent

## Data Flow
![enter image description here](https://github.com/build-security/opa-express-middleware/blob/main/Data%20flow.png)

## Usage
### Prerequisites 
- Finish our "onboarding" tutorial
- Run a pdp instance
---

**Important note**
In the following example we used our aws managed pdp instance to ease your first setup, but if you feel comfortable you are recommended to use your own pdp instance instead.
In that case, don't forget to change the **hostname** and the **port** in your code.

### Simple usage
To use the middleware add this to your startup.cs
```js
services.AddBuildAuthorization(options =>
{
    options.Enable = true;
    options.BaseAddress = "http://localhost:8181";
    options.PolicyPath = "/v1/data/build";
    options.AllowOnFailure = false;
    options.Timeout = 5;
});
```

### Mandatory configuration

 1. `BaseAddress`: The address of the Policy Decision Point (PDP)
 2. `PolicyPath`: Full path to the policy (including the rule) that decides whether requests should be authorized

### Optional configuration
 1. `AllowOnFailure`: Boolean. "Fail open" mechanism to allow access to the API in case the policy engine is not reachable. **Default is false**.
 2. `IncludeBody`: Boolean. Whether or not to pass the request body to the policy engine. **Default is true**.
 3. `IncludeHeaders`: Boolean. Whether or not to pass the request headers to the policy engine. **Default is true**
 4. `Timeout`: Boolean. Amount of time to wait before request is abandoned and request is declared as failed. **Default is 1000ms**.
 5. `Enable`: Boolean. Whether or not to consult with the policy engine for the specific request. **Default is true**
### PDP Request example

This is what the input received by the PDP would look like.

```
{
    "input": {
        "request": {
            "method": "GET",
            "query": {
                "querykey": "queryvalue"
            },
            "path": "/some/path",
            "scheme": "http",
            "host": "localhost",
            "body": {
                "bodykey": "bodyvalue"
            },
            "headers": {
                "content-type": "application/json",
                "user-agent": "PostmanRuntime/7.26.5",
                "accept": "*/*",
                "cache-control": "no-cache",
                "host": "localhost:3000",
                "accept-encoding": "gzip, deflate, br",
                "connection": "keep-alive",
                "content-length": "24"
            }
        },
        "source": {
            "port": 63405,
            "address": "::1"
        },
        "destination": {
            "port": 3000,
            "address": "::1"
        },
        "resources": {
            "attributes": {
                "region": "israel",
                "userId": "buildsec"
            },
            "permissions": [
                "user.read"
            ]
        },
        "serviceId": 1
    }
}
```
