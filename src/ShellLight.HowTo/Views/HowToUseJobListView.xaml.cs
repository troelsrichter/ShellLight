using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ShellLight.Contract;
using ShellLight.Contract.Events;

namespace ShellLight.HowTo.Views
{
    public partial class HowToUseJobListView : UserControl
    {
         private readonly UICommandContext context;
        private DispatcherTimer timer1; //simulates that job 1 is running
        private DispatcherTimer timer2; //simulates that job 2 is running
        private DispatcherTimer timer3; //simulates that job 3 is running

        public HowToUseJobListView()
        {
            InitializeComponent();
        }

        public HowToUseJobListView(UICommandContext context)
            : this()
        {
            this.context = context;
            timer1 = new DispatcherTimer();
            timer2 = new DispatcherTimer();
            timer3 = new DispatcherTimer();
        }


        private void Button_Click_Job_1(object sender, RoutedEventArgs e)
        {
            context.Events.GetEvent<BeginJobEvent>().Publish(new JobEventArg("job1","Job 1"));
            timer1.Interval = new TimeSpan(0, 0, 0, 15);
            timer1.Tick += Job1Done;
            timer1.Start();
        }

        private void Job1Done(object sender, EventArgs e)
        {
            context.Events.GetEvent<EndJobEvent>().Publish(new JobEventArg("job1", "Job 1"));
            timer1.Stop();
        }

        private void Button_Click_Job_2(object sender, RoutedEventArgs e)
        {
            context.Events.GetEvent<BeginJobEvent>().Publish(new JobEventArg("job2", "Job 2"));
            timer2.Interval = new TimeSpan(0, 0, 0, 5);
            timer2.Tick += Job2Done;
            timer2.Start();
        }

        private void Job2Done(object sender, EventArgs e)
        {
            context.Events.GetEvent<EndJobEvent>().Publish(new JobEventArg("job2", "Job 2"));
            timer2.Stop();
        }

        private void Button_Click_Job_3(object sender, RoutedEventArgs e)
        {
            context.Events.GetEvent<BeginJobEvent>().Publish(new JobEventArg("job3", "Job 3"));
            timer3.Interval = new TimeSpan(0, 0, 0, 25);
            timer3.Tick += Job3Done;
            timer3.Start();
        }

        private void Job3Done(object sender, EventArgs e)
        {
            context.Events.GetEvent<EndJobEvent>().Publish(new JobEventArg("job3", "Job 3"));
            timer3.Stop();
        }
    }
}
