using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.WPF
{
    /// <summary>
    /// An implementation of <see cref="INotifyPropertyChanged"/> that notifies clients that a property value has changed.
    /// </summary>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when changes occur that should notify clients that the value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Changes the value of the property and notifies clients that the value has changed.
        /// </summary>
        /// <param name="storage">Variable storing the value.</param>
        /// <param name="value">Value to store.</param>
        /// <param name="propertyName">(Optional) Name of the property that has changed.</param>
        /// <returns>True if the value was changed and clients were notified.</returns>
        protected virtual bool ChangeAndNotify<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;

            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }

    //Example
    //    private type _property;
    //    public type Property
    //    {
    //       get { return _property; }
    //       set { ChangeAndNotify(ref _property, value); }
    //    }
}
