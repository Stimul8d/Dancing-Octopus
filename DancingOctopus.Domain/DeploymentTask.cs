namespace DancingOctopus.Domain
{
    public class DeploymentTask
    {
        public string Id { get; }
        public string Duration { get; }
        public DeploymentStatus Status { get; }
        public bool IsCompleted { get; }

        public DeploymentTask(string id, string duration, DeploymentStatus status, bool isCompleted)
        {
            this.Id = id;
            this.Duration = duration;
            this.Status = status;
            this.IsCompleted = isCompleted;
        }

    }
}
