# HttpClient Demos

Playground for testing HttpClient and RestSharp in .NET Framework 4.8 projects. Demo of various options and how to avoid socket exhaustion.

## Running the app
- Run the `Server` app from terminal
```
cd HttpClientDemo.Server
dotnet run -c Release
```
- Open powershell. Run the following command to watch the connections
```
while ($true) {netstat -ano | findstr :7217; sleep 1; cls}
```
- Run the clients and test various options.
1. Run the `ClientNetFx` project. It's a .NET Framework 4.8 console app.
2. Run the `WebForms` project. It's a ASP.NET WebForms 4.8 app.