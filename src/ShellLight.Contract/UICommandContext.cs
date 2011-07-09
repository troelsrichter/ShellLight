using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Composite.Events;

namespace ShellLight.Contract
{
    /// <summary>
    /// When a command is executed this context is given to the command as a paramter.
    /// When you have to communicate with the rest of the system from within a command,
    /// you can do this through the commmand context.
    /// </summary>
    public class UICommandContext
    {
        public List<UICommand> RegiseredCommands { get; set; }
        public string Parameter { get; set; }
        public object Data { get; set; }
        public IEventAggregator Events { get; set; }

        //TODO can this be done in a more elegant way through MEF injection?
        public T LookUpCommand<T>()
        {
            object command = null;

            var result = from c in RegiseredCommands where c.GetType() == typeof(T) select c;
            if (result.Count() > 0)
            {
                command = result.First();
            }
            return (T)command;
        }
    }
}