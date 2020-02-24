# matcher-chief

This C# solution uses ASP.NET Core and Web Sockets to emulate a Halo matchmaking server and client. It is 100% fan-inspired and does not interface with a real Halo client in any way. It instead serves as a study on video game matchmaking algorithms.

Solution projects:
 - `MatcherChief.Server` - The matchmaking server; must run as a .NET Core 3.1 application
 - `MatcherChief.Client` - A .NET standard 2.0 library for connecting to the server
 - `MatcherChief.Shared` - A .NET standard 2.0 library used to share code between the client and server

Each project has an associated unit test project (e.g. `MatcherChief.Server` has `MatcherChief.Server.Tests`).

`MatcherChief.ClientRunner` is an additional tool included to load test clients against the server.

# Run

Run the server:
```
cd src/MatcherChief.Server
dotnet run
```

Run clients:
```
cd tools/MatcherChief.ClientRunner
dotnet run
```

When running in Development mode, the server will write an `audit.json` file to its working directory. One line is written per successful match.

The server also has an endpoint `GET /api/system/status` that returns a server status overview.

# License

See LICENSE.

I have never worked at Bungie, 343 Industries, or Microsoft, nor been given access to their source code in any way. The code and algorithms within this repository are my own.