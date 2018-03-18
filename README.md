<img src="assets/postgres2go_logo_whitebg_250.png" height="120" width="120" align="left" />
<h1>&nbsp;Postgres2Go</h1>
<br/>
<hr/>

[![GitHub release][github-release-img]][github-release-url]&nbsp;[![Master build status][appveyor-master-status]][appveyor-project-url]&nbsp;[![Develop build status][appveyor-develop-status]][appveyor-project-url]&nbsp;[![GitHub license][license-img]][license-url]

[![Stable nuget][nuget-img]][nuget-url]&nbsp;[![Downloads][nuget-stats-img]][nuget-url]


## Description

>**Inspired by [Mongo2Go](https://github.com/Mongo2Go/Mongo2Go)**

Easily spin up PostgreSQL instances for integration tests. It is .NET library that makes easy to launch new instance of PostgreSQL server for duration of integration test suite. Each instance os PostgreSQL server run in an isolated envrionement side by side of another instance of PostgreSQL server. It does not require additional external dependencies. Allows test data access layer or application when state of database should be predictable.

It targets .NET Standard 2.0, .NET 4.6, .NET 4.7. 

This Nuget package contains bundled version of PostgreSQL for Windows, Linux.

## Installation

Postgres2Go can be installed 

using __nuget.exe__ and __powershell__: 
```powershell
Install-Package Postgres2Go
```

or using __dotnet cli__: 
```bash
dotnet add package Postgres2Go
```

or using __paket.exe__: 
```
paket add Postgres2Go
```

## Usage

To start new instance of PostgreSQL server within integration test suite use `PostgresRunner` like this:

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

When test come into tear down phase then execute method __`Dispose()`__ to cleanup environment. This will remove database created for test and PostgreSQL cluster.

Other examples can be found under [src/Postgres2Go.Samples](src/Postgres2Go.Samples)

## Credits
__Copyright (c) 2017 Johannes Hoppe__

__Copyright (c) 2018 [Skyrise](http://skyrise.tech)__

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
