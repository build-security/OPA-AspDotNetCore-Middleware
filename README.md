# OPA-AspDotNetCore-Middleware
<p align="center"><img src="Logo-build.png" class="center" alt="build-logo" width="30%"/></p>

## Abstract
[build.security](https://docs.build.security/) provides simple development and management for your organization's authorization policy.
OPA-AspDotNetCore-Middleware is a .Net middleware intended for performing authorization requests against build.security PDP(Policy Decision Point)/[OPA](https://www.openpolicyagent.org/).

## Data Flow
<p align="center"> <img src="Data%20flow.png" alt="drawing" width="60%"/></p>

## Usage

Before you start we recommend completing the onboarding tutorial.

---

**Important note**

To simplify the setup process, the following example uses a local [build.security PDP instance](https://docs.build.security/policy-decision-points-pdp/pdp-deployments/standalone-docker-1).
If you are already familiar with how to run your PDP, You can also run a PDP on you environment (Dev/Prod, etc).

In that case, don't forget to change the **hostname** and the **port** in your code.
### Simple usage
To use the middleware add this to your startup.cs
```c#
services.AddBuildAuthorization(options =>
{
    options.Enable = true;
    options.BaseAddress = "http://localhost:8181";
    options.PolicyPath = "/v1/data/authz/allow";
    options.AllowOnFailure = false;
    options.Timeout = 5;
});
```

Then add this attributes to your middlewares
### Mandatory configuration

 1. `BaseAddress`: String. The address of the Policy Decision Point (PDP)

### Optional configuration
 1. `PolicyPath`: String. Full path to the policy (including the rule) that decides whether requests should be authorized. **/v1/data/authz/allow**
 2. `AllowOnFailure`: Boolean. "Fail open" mechanism to allow access to the API in case the policy engine is not reachable. **Default is false**.
 3. `IncludeBody`: Boolean. Whether or not to pass the request body to the policy engine. **Default is true**.
 4. `IncludeHeaders`: Boolean. Whether or not to pass the request headers to the policy engine. **Default is true**
 5. `Timeout`: Boolean. Amount of time to wait before request is abandoned and request is declared as failed. **Default is 1000ms**.
 6. `Enable`: Boolean. Whether or not to consult with the policy engine for the specific request. **Default is true**
 7. `IgnoreEndpoints`- String array. Determines what endpoint should be excluded from authorization.
 8. `IgnoreRegex` - String array. Determines what endpoints should be excluded from authorization using regex.  

### PDP Request example

This is what the input received by the PDP would look like:

```
{
   "input":{
      "request":{
         "method":"GET",
         "query":[
            
         ],
         "path":"/static/js/0.chunk.js",
         "scheme":"http",
         "host":{
            "value":"localhost:5000",
            "hasValue":true,
            "host":"localhost",
            "port":5000
         }
      },
      "source":{
         "ipAddress":"::1",
         "port":64288
      },
      "destination":{
         "ipAddress":"::1",
         "port":5000
      },
      "resources":{
         "requirements":[
            
         ],
         "attributes":{
            
         }
      },
      "sample":"application"
   }
}
```

If everything works well you should receive the following response:

```
{
    "decision_id":"ef414180-05bd-4817-9634-7d1537d5a657",
    "result":true
}
```
