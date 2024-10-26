# Rqlite .NET Client

This is a community client for accessing [Rqlite](https://rqlite.io) from .NET applications, **currently in pre-release**.

## Contents

* [Installation](#installation)
* [Usage](#usage)
* [Readme App](#readme-app)
* [Licence / Copyright](#licence)

## Installation

You can either download and build the source yourself in your own project, or install the [NuGet Package](https://nuget.org/packages/rqlite).

## Usage

The recommended way to use `Rqlite.Client` is via dependency injection.  Support for `Microsoft.Extensions.DependencyInjection` is included, e.g.

```csharp
using Rqlite.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRqlite();

// ...
```

You can then inject `IRqliteClientFactory` into a class, which you can then use to create a client, e.g.

```csharp
public class Foo(IRqliteClientFactory factory)
{
    public async Task<string> BarAsync()
    {
        using var client = factory.CreateClient();
        return await client.GetVersionAsync().UnwrapAsync(() => "Unknown version.");
    }
}
```

`IRqliteClient` objects implement `IDisposable` so it is recommended you create them with the `using` keyword.  This will safely dispose of your client object once you are done with it.  (You could create a client in your constructor, but this is not recommended.)

Once you have a client you can start executing queries, e.g.

```csharp
var id = 10;
var items = await client.QueryAsync<Item>("SELECT * FROM item WHERE id = :id", new { id });
```

NB: This client is **async only** so you **must** use it within `async` methods so you can make use of the `await` keyword.

## Readme App

To see the library in action, the source contains a Readme App.  Simply checkout the repository, spin up a Rqlite instance (Docker required), and run the project:

```bash
# in a temporary directory of your choice
$ git checkout https://github.com/bfren/rqlite .
$ chmod +x run.sh
$ ./run.sh

# in a new terminal
$ dotnet run --project apps/ReadmeApp
```

## Licence

> [MIT](https://mit.bfren.dev/2023)

## Copyright

> Copyright (c) 2023-2024 [bfren](https://bfren.dev) (unless otherwise stated)
