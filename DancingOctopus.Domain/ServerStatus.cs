namespace DancingOctopus.Domain
{
    public class ServerStatus
    {
        public string Server { get; private set; }
        public int NumberOfRunningTasks { get; private set; }

        public ServerStatus(string server, int numberOfRunningTasks)
        {
            this.Server = server;
            this.NumberOfRunningTasks = numberOfRunningTasks;
        }
    }
}
