using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Controls;
using ShellLight.Contract;
using ShellLight.Contract.Events;
using ShellLight.Framework.ViewModels;

namespace ShellLight.Framework
{
    public partial class JobListView : UserControl
    {
        public JobListView()
        {
            InitializeComponent();
            DataContext = new JobListViewModel();
        }

        public JobListView(UICommandContext context):this()
        {
            context.Events.GetEvent<BeginJobEvent>().Subscribe(BeginJobHandler);
            context.Events.GetEvent<EndJobEvent>().Subscribe(EndJobHandler);
        }

        public void EndJobHandler(JobEventArg obj)
        {
            var datacontext = DataContext as JobListViewModel;
            var result = from j in datacontext.Jobs where j.Id == obj.JobId select j;
            if (result.Count() == 1)
            {
                var jobViewModel = result.First();
                jobViewModel.Stop();
                StoreNewAvarageDuration(obj, jobViewModel);
                datacontext.Jobs.Remove(jobViewModel);
            }
        }

        private void StoreNewAvarageDuration(JobEventArg obj, JobViewModel jobViewModel)
        {
            var newAvarageDuration = jobViewModel.AvarageDuration != 0
                                         ? (jobViewModel.AvarageDuration + jobViewModel.PreviousDuration)/2
                                         : jobViewModel.PreviousDuration;
            IsolatedStorageSettings.ApplicationSettings[obj.JobId] = newAvarageDuration;
        }

        public void BeginJobHandler(JobEventArg obj)
        {
            var datacontext = DataContext as JobListViewModel;
            var result = from j in datacontext.Jobs where j.Id == obj.JobId select j;
            if (result.Count() == 0)
            {
                var jobViewModel = new JobViewModel(obj.JobId, obj.JobName);
                
                LoadAvarageDurationFromStore(obj, jobViewModel);

                datacontext.Jobs.Add(jobViewModel);
                jobViewModel.Start();
            }
        }

        private void LoadAvarageDurationFromStore(JobEventArg obj, JobViewModel jobViewModel)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(obj.JobId))
            {
                jobViewModel.AvarageDuration = (long) IsolatedStorageSettings.ApplicationSettings[obj.JobId];
            }
        }
    }
}
