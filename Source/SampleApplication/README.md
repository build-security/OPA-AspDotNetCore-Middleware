# OPA-AspDotNetCore-Middleware

### How to use the sample app
- Run the `Sample Application` 
- Run a build.security [PDP](https://docs.build.security/documentation/policy-decision-points-pdp/pdp-deployments/standalone-docker-1) / [OPA](https://www.openpolicyagent.org/) with the following rego policy :


```
package authz

default allow = false

allowedOpeartion = ["GET", "POST"]
allowedScheme = ["http", "https"]
allowedPermissions = ["overrideAuthController.read", "overrideAuthController.create"]

allow  {
  is_schema_allowed
  is_authorized
  is_operation_allowed
} 

# allowing only http and https
is_schema_allowed {
  some i 
  input.request.scheme == allowedScheme[i]
}

# allowing only certain actions
is_authorized {
  some k, j
  input.resources.requirements[j] == allowedPermissions[k]
}

# allowing only POST and GET operations
is_operation_allowed {
  some j
  input.request.method == allowedOpeartion[j]
}
```

This policy will allow only POST and GET request to get processes by the `Sample Application` .
## Testing your policy - 
Now we will test your policy with two different requests, one should fail and the other should succeed.

### # First Pdp request (Should succeed)

Send a POST request to the `Sample Application`  overrideAuthController.

``` 
curl -X POST -d '{"input":{
  "email": "richard@email.com"
}}' http://localhost:5000/overrideAuth
```

### The Pdp request
After middleware enrichment the request input recieved by the pdp would look like:

```
{
  "destination": {
    "ipAddress": "::1",
    "port": 5000
  },
  "request": {
    "body": {
      "input": {
        "email": "richard@email.com"
      }
    },
    "host": {
      "hasValue": true,
      "host": "localhost",
      "port": 5000,
      "value": "localhost:5000"
    },
    "method": "POST",
    "path": "/overrideAuth",
    "query": [],
    "scheme": "http"
  },
  "resources": {
    "attributes": {
      "action": "Create",
      "controller": "OverrideAuth"
    },
    "requirements": [
      "overrideAuthController.create"
    ]
  },
  "sample": "application",
  "source": {
    "ipAddress": "::1",
    "port": 63175
  }
}
```

Now, If everything works well your service recieve the following result :

```
{
    "decision_id":"ef414180-05bd-4817-9634-7d1537d5a657",
    "result":true
}
```

And your curl will return

``` 
You created a new object successfully 
```

### # Second Pdp request (Should fail)

Send a PUT request to the `Sample Application`  overrideAuthController.

``` 
curl -X PUT -d '{"input":{
  "email": "richard@email.com"
}}' http://localhost:5000/overrideAuth 
```

### The Pdp request
After the middleware enrichment the request input recieved by the pdp would look like :

```
{
  "destination": {
    "ipAddress": "::1",
    "port": 5000
  },
  "request": {
    "body": {
      "input": {
        "email": "richard@email.com"
      }
    },
    "host": {
      "hasValue": true,
      "host": "localhost",
      "port": 5000,
      "value": "localhost:5000"
    },
    "method": "PUT",
    "path": "/overrideAuth",
    "query": [],
    "scheme": "http"
  },
  "resources": {
    "attributes": {
      "action": "Put",
      "controller": "OverrideAuth"
    },
    "requirements": [
      "overrideAuthController.put"
    ]
  },
  "sample": "application",
  "source": {
    "ipAddress": "::1",
    "port": 63387
  }
}
```
Now, If everything works well your service recieve the following result :

```
{
    "decision_id":"6df9779a-1776-46e1-b1c8-2e4861b840c4",
    "result":false
}
```

And your curl will return:

``` 
Forbidden
```
