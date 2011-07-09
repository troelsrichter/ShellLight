using Microsoft.Practices.Composite.Presentation.Events;

namespace ShellLight.Contract.Events
{
    public class BeginJobEvent : CompositePresentationEvent<JobEventArg>
    {
        
    }

    public class JobEventArg
    {
        public JobEventArg(string jobId, string jobName)
        {
            JobId = jobId;
            JobName = jobName;
        }

        public string JobId { get; set; }
        public string JobName { get; set; }
    }
}