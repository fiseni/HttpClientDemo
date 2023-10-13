# HttpClient Demos

Playground for testing HttpClient and RestSharp in .NET Framework 4.8 projects. Demo of various options and how to avoid some common pitfalls.

## Running the app
- Run the `Server` app from terminal (ASP.NET Core 7 app)
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

## Scenarios

### Option 1

Not recommended. It instantiates a new HttpClient on each call; that means new HttpMessageHandler instance and new TCP connection. The least we can do is dispose the HttpClient instance (enclose it in using statement). This will force the connection to TIME_WAIT state (and OS will close it after ~2 minutes). It's not efficient and can lead to socket exhaustion. Also, creating a new connection is expensive.

### Option 11

Same as Option1. But, we're initiating 4 requests to demonstrate the effect of concurrent calls, and how quickly the open connections can pile up. On each call 4 new connections will be created.

### Option 2

Much better approach. In ASP.NET Core we have well established practices for HttpClient usage (e.g. IHttpClientFactory, typed clients). In older platforms wiring up the IHttpClientFactory is not that straightforward, especially if you're utilizing old technologies like web services (asmx). We can use a static HttpClient instance instead. This will reuse the same HttpMessageHandler instance and TCP connection. The downside is that the client will remain oblivious to DNS changes. Also, the HttpClient 4.2.0.0 is not based on SocketsHttpHandler, therefore we can't utilize the PooledConnectionLifetime option. Luckily, we still can use the ServicePointManager to control the connection lifetime. We can set the ConnectionLeaseTimeout to a lower value (by default is indefinite); and we can control the dns refrsh through the DnsRefreshTimeout setting (by defautlt is 120 seconds).

### Option 21

Same as option 2. We're initiating 4 requests to demonstrate the effect of concurrent calls. Since we're using the same HttpClient instance, we might expect that a single connection will be created and used for all calls. But, that's not the case for concurrent calls. It will try to create new connections so it can serve the requests concurrently. So, how many connections? For a single client instance, the max number of connections is defined by ServicePoint.ConnectionLimit setting (by default is 2, or 10 for web apps). In our case, 2 connections will be created and used for all 4 calls. On the next method calls, the same 2 connections are reused. Note: the limit is per client instance, that's why we end up with way more connections in Option11.

### Option 3

Not recommended. We're using RestSharp library, and instantiating a new RestClient on each call. Internally, it uses HttpClient; and that means new HttpMessageHandler instance and new TCP connection. Same issues as Option1. It serves to illustrate the point that just because we're using a 3rd party library, it doesn't mean that we're not prone to same mistakes.

### Option 4

RestClient offers the option to reuse the internal HttpClient instance by setting useClientFactory to true. It holds a dictionary of HttpClient instances, one per BaseUrl. This is a compelling approach, since no longer we have to capture it as a static state. Also it offers more flexibility and customization per call. It's prone to the same DNS issues. Follow the recommendations from Option2.

### Option 5

We've defined a factory that creates a RestClient instance (returns the same client instance). Also, we've defined a wrapper around the RestClient that configures the client, handles the authentication and exposes a singleton instance. Note that we're not utilizing the useClientFactory option, since we hold a single instance anyway. It's prone to the same DNS issues. Follow the recommendations from Option2.

### Option 6

It's an evolved version of Option5. I'm quite fond of this approach. We're utilizing the useClientFactory setting to cache the HttpClient instances, but consumers have flexibility of further customization per RestClient instance. Also tried to mimic the IHttpClientFactory behavior, the named clients feature in particular. It's prone to the same DNS issues. Follow the recommendations from Option2.

### Option 61

It's based on Option6, just exploring the effect of concurrent calls.

### Option 62

Same as Option61, but we're creating a new RestClient instance through the factory on each call. No effect on the number of open connections.

### Option 63

It's based on Option6, just exploring the sync API of RestSharp. It uses AsyncHelper to run sync over async. That's something that we should avoid at all costs. But, some old technologies like web services (asmx) don't have support for TAP. That's one of the reasons I tend to use RestSharp for older apps, the sync APIs. It's quite compelling implementation. Unlike many implementation that tend to create new thread to avoid deadlock `Task.Run(async () => {await ...}).Result`, this implementation is using a custom SynchronizationContext to avoid deadlock while remaining on the same thread. Quite convenient, you can even access HttpContext.Current.

### Option 64

Same as option 63, just running multiple calls. The first impression is that this will run fully synchronously. But, the results are quite surprising.