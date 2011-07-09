using System.ComponentModel;
using System.Windows;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;
using ShellLight.Contract.Events;

namespace ShellLight.Framework.Commands
{
    [AttachToTray]
    public class JobListCommand: UICommand, INotifyPropertyChanged
    {
        private JobListView listView;
        private int activeJobCount = 0;

        private const string IconSourceInactive = "/ShellLight.Framework;component/Icons/job_inactive.png";
        private const string IconSourceActive = "/ShellLight.Framework;component/Icons/job.png";
        private string iconSource = IconSourceInactive;

        public override UIElement DoShow()
        {
            if (listView == null)
            {
                listView = new JobListView(Context);
                Context.Events.GetEvent<BeginJobEvent>().Subscribe(BeginJobHandler);
                Context.Events.GetEvent<EndJobEvent>().Subscribe(EndJobHandler);
            }
            return listView;
        }

        public void EndJobHandler(JobEventArg obj)
        {
            --activeJobCount;
            if (activeJobCount <= 0)
            {
                iconSource = IconSourceInactive;
                PropertyChanged(this, new PropertyChangedEventArgs("IconSource"));
            }
        }

        public void BeginJobHandler(JobEventArg obj)
        {
            ++activeJobCount;
            if (activeJobCount > 0)
            {
                iconSource = IconSourceActive;
                PropertyChanged(this, new PropertyChangedEventArgs("IconSource"));
            }
        }

        public override string IconSource
        {
            get
            {
                return iconSource;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}