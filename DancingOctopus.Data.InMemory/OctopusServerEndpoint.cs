namespace DancingOctopus.Data.InMemory
{
    internal class OctopusServerEndpoint
    {
        private string server;
        private string apiKey;

        public OctopusServerEndpoint(string server, string apiKey)
        {
            this.server = server;
            this.apiKey = apiKey;
        }
    }
}