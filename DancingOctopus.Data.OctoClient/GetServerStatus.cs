using DancingOctopus.Domain.Services;
using System.Linq;
using DancingOctopus.Domain;
using Octopus.Client;
using System.Collections.Generic;
using Octopus.Client.Model;
using System;

namespace DancingOctopus.Data.InMemory
{
    public class GetServerStatus : IGetServerStatus
    {
        private OctopusRepository repo;
        private ConnectionDetails connectionDetails;
        private IEnumerable<TaskResource> tasks = new List<TaskResource>();

        public GetServerStatus(ConnectionDetails conn)
        {
            this.connectionDetails = conn;
            this.repo = new OctopusRepository(new OctopusServerEndpoint(conn.Server, conn.ApiKey));
        }
        public ServerStatus GetStatus()
        {
            GetTasks();
            return new ServerStatus(connectionDetails.Server, tasks.Count());
        }

        private void GetTasks()
        {
            tasks = repo.Tasks.GetAllActive().ToList();
        }

        public DeploymentTask GetTaskStatus(string taskId)
        {
            if (tasks.Count() == 0) GetTasks();
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null) task = repo.Tasks.Get(taskId);
            return new DeploymentTask(task.Id, task.Duration, 
                task.State == TaskState.Success ? DeploymentStatus.Successful : DeploymentStatus.Failed, task.IsCompleted);
        }
    }
}
