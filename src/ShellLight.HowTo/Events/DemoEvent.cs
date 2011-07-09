using Microsoft.Practices.Composite.Presentation.Events;

namespace ShellLight.HowTo.Events
{
    public class DemoEvent : CompositePresentationEvent<Demo> {}

    public class Demo
    {
        public string Text { get; set; }
    }
}