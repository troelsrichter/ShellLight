using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace ShellLight.Framework.ViewModels
{
    public class JobListViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public JobListViewModel()
        {
            Jobs = new ObservableCollection<JobViewModel>();
        }

        public ObservableCollection<JobViewModel> Jobs { get; private set;}
    }

    public class JobViewModel: INotifyPropertyChanged
    {
        private DispatcherTimer timer;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    }
                }
            }
        }

        public JobViewModel(string id, string name)
        {
            Id = id;
            this.name = name;
            IsIndeterminate = true;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Duration"));
            }
        }

        private bool isIndeterminate = true;
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            set
            {
                if (value != isIndeterminate)
                {
                    isIndeterminate = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsIndeterminate"));
                    }
                }
            }
        }

        public void Start()
        {
            StartTime = DateTime.Now.Ticks;
            if (timer != null)
            {
                timer.Start();
            }
        }

        public void Stop()
        {
            EndTime = DateTime.Now.Ticks;
            if (timer != null)
            {
                timer.Stop();
            }
        }

        public long StartTime { get; private set; }

        public long EndTime { get; private set; }

        /// <summary>
        /// Previous duration in milliseconds
        /// </summary>
        /// <returns></returns>
        public long PreviousDuration
        {
            get { return EndTime - StartTime; }
        }

        private long avarageDuration;
        /// <summary>
        /// Avarage duration in milliseconds
        /// </summary>
        public long AvarageDuration
        {
            get { return avarageDuration; }
            set
            {
                avarageDuration = value;
                if (avarageDuration != 0)
                {
                    IsIndeterminate = false;
                }
            }
        }

        public long Duration
        {
            get { return DateTime.Now.Ticks - StartTime; }
        }
    }
}