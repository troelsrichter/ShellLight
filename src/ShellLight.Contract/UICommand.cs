using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using ShellLight.Contract.Attributes;

namespace ShellLight.Contract
{
    public enum UICommandState
    {
        Created,
        Initialized,
        Active,
    } ;

    /// <summary>
    /// This is the entrypoint to all functionality in the system
    /// All functionality starts by creating a Command that derives from this class
    /// </summary>
    [InheritedExport]
    public abstract class UICommand : ICommand
    {
        private string iconSource;
        private string name;
       
        public UICommandContext Context { get; set; }
        public int Score { get; set; }

        public event EventHandler CanExecuteChanged;
        public event EventHandler<EventArgs> BeforeShow;
        public event EventHandler<UICommandEventArgs> AfterShow;
        public event EventHandler<EventArgs> Closing;
        
        protected UICommand()
        {
            Id = this.GetType().FullName;
            State = UICommandState.Created;
            ParameterDiscription = string.Empty;
        }

        public void Execute(object parameter)
        {
            var context = parameter as UICommandContext;
            if (context != null)
            {
                this.Context = context;
            }
            Show();
        }

        public void Show()
        {
            OnBeforeShow();
            View = DoShow();
            OnAfterShow(View);
            State = UICommandState.Active;
        }

        public void Close()
        {
            State = UICommandState.Initialized;
            OnClosing();
        }

        public UICommandState State { get; set; }

        //todo parameter discription is not used to discribe the paramter!
        /// <summary>
        /// Is shown in parenthesis after the name in the launcher
        /// </summary>
        public string ParameterDiscription {get; private set;}

        public virtual bool CanExecute(object parameter)
        {
            if (this.HasAttribute<RequiresElevatedTrustAttribute>())
            {
                if (!Application.Current.HasElevatedPermissions)
                {
                    AddParameterDiscription("Out-Of-Browser only");
                    return false;
                }
            }
            return true;
        }

        public void AddParameterDiscription(string discription)
        {
            if (!ParameterDiscription.Contains(discription))
            {
                if (!string.IsNullOrEmpty(ParameterDiscription))
                {
                    ParameterDiscription = ParameterDiscription.Insert(0, ", ");
                }
                ParameterDiscription = ParameterDiscription.Insert(0, discription);
            }
        }


        public abstract UIElement DoShow();

        /// <summary>
        /// Default: FullName of the type
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Default: Class name of the command is used to extract name
        /// Convention: upper case letters becomes spaces and command is removed. 
        /// Convetion example: EditUserCommand -> Edit User
        /// </summary>
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = this.ConventionalName();
                }
                return name;
            }
        }

        public UIElement View { get; private set; }

        protected virtual void OnAfterShow(UIElement view)
        {
            if (AfterShow != null)
            {
                AfterShow(this, new UICommandEventArgs(this));
            }
        }

        protected virtual void OnBeforeShow()
        {
            if (BeforeShow != null)
            {
                BeforeShow(this, new EventArgs());
            }
        }

        protected virtual void OnClosing()
        {
            if (Closing != null)
            {
                Closing(this, new EventArgs());
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(ParameterDiscription) ? Name + " (" + ParameterDiscription + ")" : Name;
        }

        /// <summary>
        /// Relative uri of the command icon
        /// Default: "/ShellLight;component/Images/default.png"
        /// Convention: if you place an icon under /Icons/<name of command>.png this will be used
        /// Convention example: /Commands/EditUserCommand
        ///                     /Icons/EditUser.png
        /// </summary>
        public virtual string IconSource
        {
            get
            {
                if (string.IsNullOrEmpty(iconSource))
                {
                    iconSource = this.ConventionalIconSource();
                }
                return iconSource;
            }
        }
    }

    public class UICommandEventArgs : EventArgs
    {
        public UICommandEventArgs(UICommand command)
        {
            Command = command;
        }

        public UICommand Command { get; private set; }
    }


}