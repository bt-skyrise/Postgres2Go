Postgres2Go
===========
> Postgres2Go - PostgreSQL for integration tests

[![GitHub release][github-release-img]][github-release-url]&nbsp;[![Master build status][appveyor-master-status]][appveyor-project-url]&nbsp;[![Develop build status][appveyor-develop-status]][appveyor-project-url]&nbsp;[![GitHub license][license-img]][license-url]

[![Stable nuget][nuget-img]][nuget-url]&nbsp;[![Downloads][nuget-stats-img]][nuget-url]

### Description
>Inspired by __[https://github.com/Mongo2Go/Mongo2Go](https://github.com/Mongo2Go/Mongo2Go)__

Easily spin up PostgreSQL instances for integration tests. It targets .NET Standard 2.0, .NET 4.6, .NET 4.7. This Nuget package contains the executables of PostgreSQL for Windows, Linux.

## Installation

Using __nuget.exe__ and __powershell__: 
```
Install-Package Postgres2Go
```

or using __dotnet cli__: 
```
dotnet add package Postgres2Go
```

or using __paket.exe__: 
```
paket add Postgres2Go
```

## Usage

```csharp
public class Program
{
    public static void Main(params string[] args) 
    {
        using(var runner = PostgresRunner.Start()) 
        using(var conn = new NpgsqlConnection(runner.GetConnectionString()))
        using(var cmd = new NpgsqlCommand("select version()", conn))
        {
            conn.Open();
            var version = cmd.ExecuteScalar() as string;

            Console.WriteLine($"PostgreSQL version: {version}");
        }
    }
}
```

`PostgresRunner.Start()` accepts parameters:
- `DataDirectory` directory where new instance of PostgreSQL cluster will keep its data; if not provided then special directory will be created inside of `TEMP` folder
- `BinariesSearchPattern` path part where PostgreSQL distribution should be located
- `Port` TCP port used by PostgreSQL instance; if not provided then first free TCP port above 15433 will be used

To cleanup environment execute method __`Dispose()`__

Example usage can be found under [src/Postgres2Go.Samples](src/Postgres2Go.Samples)

## Credits
Copyright (c) 2017 Johannes Hoppe
Copyright (c) 2018 [Skyrise](http://skyrise.tech)

Special thanks to all [Contributors](CREDITS.md)

# License

This software is distributed under [MIT License](LICENSE.md).
It contains third-party files located in the pg-dist folder that are distributed under [PostgreSQL License](tools/LICENSE.md)

[appveyor-master-status]: https://ci.appveyor.com/api/projects/status/github/bt-skyrise/Postgres2Go?svg=true&branch=master&passingText=master%20pass&failingText=master%20failed
[appveyor-develop-status]: https://ci.appveyor.com/api/projects/status/github/bt-skyrise/Postgres2Go?svg=true&branch=develop&passingText=develop%20pass&failingText=develop%20failed
[appveyor-project-url]: https://ci.appveyor.com/project/skyrisetech/postgres2go
[github-release-img]: https://img.shields.io/github/release/bt-skyrise/Postgres2Go.svg
[github-release-url]: https://github.com/bt-skyrise/Postgres2Go/releases
[license-img]: https://img.shields.io/badge/License-MIT-green.svg
[license-url]: https://raw.githubusercontent.com/bt-skyrise/Postgres2Go/master/LICENSE.md
[nuget-img]: https://img.shields.io/nuget/v/Postgres2Go.svg?label=stable%20nuget
[nuget-stats-img]: https://img.shields.io/nuget/dt/Postgres2Go.svg?label=downloads
[nuget-url]:https://www.nuget.org/packages/Postgres2Go/
