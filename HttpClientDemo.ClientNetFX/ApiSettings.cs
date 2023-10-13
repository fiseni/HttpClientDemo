namespace HttpClientDemo
{
    public class ApiSettings
    {
        public static ApiSettings Instance { get; } = new ApiSettings();

        //public string ApiUrl { get; } = "https://localhost:7216";
        public string ApiUrl { get; } = "http://localhost:7217";
        public string ApiGetEndpoint { get; } = "api/test";

        //public string ApiUrl { get; } = "https://api.publicapis.org";
        //public string ApiGetEndpoint { get; } = "/entries";
    }
}
