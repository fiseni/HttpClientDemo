namespace HttpClientDemo
{
    public class ApiSettings
    {
        public static ApiSettings Instance = new ApiSettings();

        //public string ApiUrl { get; } = "https://localhost:7216";
        public string ApiUrl { get; } = "http://localhost:7217";
        public string ApiGetEndpoint { get; } = "api/test1";
        public string ApiGetEndpointProtected { get; } = "api/test2";
    }

    public class PublicApisSetting
    {
        public static PublicApisSetting Instance = new PublicApisSetting();

        public string ApiUrl { get; } = "https://api.publicapis.org";
        public string ApiGetEndpoint { get; } = "entries";
    }
}
