namespace DancingOctopus.Domain
{
    public class ConnectionDetails
    {
        public string Server { get; set; }
        public string ApiKey { get; set; }

        public ConnectionDetails(string server, string apiKey)
        {
            this.Server = server;
            this.ApiKey = apiKey;
        }
    }
}
