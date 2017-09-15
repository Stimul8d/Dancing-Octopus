using System;
using DancingOctopus.Domain;
using DancingOctopus.Domain.Services;

namespace DancingOctopus.Data.InMemory
{
    public class GetInMemoryServerStatus : IGetServerStatus
    {
        public ServerStatus GetStatus()
        {
            return new ServerStatus("http://somethingverylong.anotherthing.co.uk", 12);
        }

        public DeploymentTask GetTaskStatus(string taskId)
        {
            throw new NotImplementedException();
        }
    }
}
