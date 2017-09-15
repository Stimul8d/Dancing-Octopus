using System.Diagnostics;

namespace DancingOctopus.Domain
{
    [DebuggerDisplay("{Tenant.Name}-{Project.Name}:{Source.Name}->{Destination.Name}")]
    public class Deployment
    {
        public DeploymentEnvironment Destination { get; }
        public DeploymentStatus Status { get; set; }
        public string Duration { get; set; }
        public string Url { get; set; }
        public bool Complete { get; set; }
        public Release Release { get; }
        public Tenant Tenant { get; }

        public Deployment(Tenant tenant, Release release)
        {
            this.Release = release;
            this.Destination = release.Environment;
            this.Status = DeploymentStatus.NotStarted;
            this.Tenant = tenant;
        }
    }
}
