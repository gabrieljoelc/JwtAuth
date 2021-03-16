# Azure Function JWT authorization

This has client and server examples for:

1. Client - Fetching an access token JWT, and sending it in request to "Server"
1. Server - Validating a JWT sent from a client

Note: This is likely mostly a Keycloak specific implementation but I'm not entirely sure.

## Getting started

Prerequisites:
- Azure Function runtime
- Visual Studio, Jetbrains Rider, or Azure Function CLI

1. Start the `JwtAuth.FunctionDemo` project.
2. Send a request with Postman or `curl` like:

```
curl http://localhost:7071/api/Client
```

3. You should see a JSON response body like this:

```
{
    "accessToken": "<JWT access token>",
    "serverResponseBody": {
        "result": "Accepted"
    }
}
```
