# Azure Function JWT authorization

This has client and server examples for:

1. Client - Fetching an access token JWT, and sending it in request to "Server"
1. Server - Validating a JWT sent from a client

Note: This is likely mostly a Keycloak specific implementation but I'm not entirely sure.

## Highlights

As you explore the code, start with these classes in mostly this order:

1. [JwtAuth.FunctionDemo.ClientServer](https://github.com/gabrieljoelc/JwtAuth/blob/main/JwtAuth.FunctionDemo/ClientServer.cs)
2. [JwtAuth.FunctionDemo.Startup](https://github.com/gabrieljoelc/JwtAuth/blob/main/JwtAuth.FunctionDemo/Startup.cs)
3. [JwtAuth.Core.DefaultJwtFetcher](https://github.com/gabrieljoelc/JwtAuth/blob/main/JwtAuth.Core/DefaultJwtFetcher.cs)
4. [JwtAuth.Http.NetHttpAuthorizer](https://github.com/gabrieljoelc/JwtAuth/blob/main/JwtAuth.Http/NetHttpAuthorizer.cs)
5. [JwtAuth.Core.DefaultJwtValidator](https://github.com/gabrieljoelc/JwtAuth/blob/main/JwtAuth.Core/DefaultJwtValidator.cs)

## Getting started

Prerequisites:
- Azure Function runtime
- Visual Studio, Jetbrains Rider, or Azure Function CLI

1. Copy `JwtAuth.FunctionDemo/local.settings.dist.json` to `JwtAuth.FunctionDemo/local.settings.json`
2. Fill-out your identity server values in `JwtAuth.FunctionDemo/local.settings.json`
3. Start the `JwtAuth.FunctionDemo` project.
4. Send a request with Postman or `curl` like:

```
curl http://localhost:7071/api/Client
```

5. You should see a JSON response body like this:

```
{
    "accessToken": "<JWT access token>",
    "serverResponseBody": {
        "result": "Accepted"
    }
}
```
