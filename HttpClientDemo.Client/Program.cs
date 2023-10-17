
while (true)
{
    Console.Write("Choose option: ");
    var option = Console.ReadLine();

    if (option is null) break;

    var response = await GetResponseAsync(option);

    Console.WriteLine($"{Environment.NewLine}Response: {response}");
}

static async Task<string> GetResponseAsync(string option)
{
    return option switch
    {
        "1" => await new HttpClientDemo.Client1.Service1().GetStatusCodeAsync(),
        "11" => await new HttpClientDemo.Client1.Service11().GetStatusCodeAsync(),
        "2" => await new HttpClientDemo.Client2.Service2().GetStatusCodeAsync(),
        "21" => await new HttpClientDemo.Client2.Service21().GetStatusCodeAsync(),
        _ => "Not valid option!",
    };
}
