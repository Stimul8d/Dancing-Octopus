namespace DancingOctopus.Domain.Services
{
    public interface IGetServerStatus
    {
        ServerStatus GetStatus();
        DeploymentTask GetTaskStatus(string taskId);
    }
}
