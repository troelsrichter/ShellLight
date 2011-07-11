using System;
using System.ComponentModel;


namespace ShellLight.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler pceh = PropertyChanged;
            if (pceh != null)
            {
                pceh(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Set's the value of the target parameter (replacing the reference) if the value has changed. Also fires
        /// a PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="target">The target to be swapped out, if different to the value parameter</param>
        /// <param name="value">The new value</param>
        /// <param name="changedProperties">A list of properties whose value may have been impacted by this change and whose PropertyChanged event should be raised</param>
        /// <returns>True if the value is changed, False otherwise</returns>
        protected virtual bool SetValue<T>(ref T target, T value, params string[] changedProperties)
        {
            if (Object.Equals(target, value))
            {
                return false;
            }
            target = value;
         
            OnPropertyChanged(changedProperties);
            IsDirty = true;

            return true;
        }

        protected virtual void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (string property in propertyNames)
            {
                OnPropertyChanged(property);
            }
        }

        private Guid id;
        public Guid Id
        {
            get { return id; }
            set { SetValue(ref id, value, "Id"); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                if (isDirty != value)
                {
                    isDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }
    }

}