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
- Run the `ClientNetFx` console app, and test various options.