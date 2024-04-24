# Configuration

## Default Values

Technically you don't need to add any configuration to use the library, if your Rqlite client instance is running on `localhost` on the default port `4001`.  This is like including the following in your `appsettings.json` file:

```json
{
    "Rqlite": {
        "DefaultClientName": "localhost",
        "Clients": {
            "localhost": {
                "BaseAddress": "http://localhost:4001",
                "IncludeTimings": false,
                "TimeoutInSeconds": 30
            }
        }
    }
}
```

With this config the following are functionally equivalent:

```csharp
using var client = factory.CreateClient();
using var client = factory.CreateClient("localhost");
```

## Global Options

You can set global values for the base address, whether or not to include timings with each request, and the network timeout, which will affect all clients, e.g.

```json
{
    "Rqlite": {
        "BaseAddress": "http://db:4001",
        "IncludeTimings": true,
        "TimeoutInSeconds": 5
    }
}
```

With this config (i.e. no named clients) you would need to use:

```csharp
using var client = factory.CreateClient();
```

## Defining Clients

You can define named clients with different options:

```json
{
    "Rqlite": {
        "DefaultClientName": "default",
        "Clients": {
            "default": { },
            "auth": {
                "BaseAddress": "http://auth:4001",
                "TimeoutInSeconds": 10
            },
            "with_timings": {
                "IncludeTimings": true
            },
            "short_timeout": {
                "TimeoutInSeconds": 5,
            }
        }
    }
}
```

Hopefully by now it is fairly obvious what values these clients will have:

```csharp
using var clientDefault = factory.CreateClient();
// BaseAddress: http://localhost:4001
// IncludeTimings: false
// TimeoutInSeconds: 30

using var clientAuth = factory.CreateClient("auth");
// BaseAddress: http://auth:4001
// IncludeTimings: false
// TimeoutInSeconds: 10

using var clientAuth = factory.CreateClient("with_timings");
// BaseAddress: http://localhost:4001
// IncludeTimings: true
// TimeoutInSeconds: 30

using var clientAuth = factory.CreateClient("short_timeout");
// BaseAddress: http://localhost:4001
// IncludeTimings: false
// TimeoutInSeconds: 5
```
