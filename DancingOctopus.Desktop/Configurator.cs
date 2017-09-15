using Formo;

namespace DancingOctopus.Desktop
{
    public class Configurator
    {
        dynamic config = new Configuration();

        public string GetServer() => config.Server;
        public string GetApiKey() => config.ApiKey;
    }
}
