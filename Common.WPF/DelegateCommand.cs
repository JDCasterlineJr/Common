using System;
using System.Windows.Input;

namespace Common.WPF
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for Execute(T) and CanExecute(T).
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Fields

        /// <summary>
        /// Action to be taken when this command is executed
        /// </summary>
        private readonly Action<object> _execute;
        /// <summary>
        /// Predicate that evaluates if this command can be executed
        /// </summary>
        private readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate that evaluates if this command can be executed.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    //Example
    //    private ICommand _command;
    //    private bool _canExecuteCommand = true;
    //    public ICommand ResetTimerCommand
    //    {
    //        get { return _command ?? (_command = new DelegateCommand(param => CommandAction(), param => _canExecuteCommand)); }
    //    }

    //    public bool CanExecuteCommand
    //    {
    //        get { return _canExecuteCommand; }
    //        set { _canExecuteCommand = value; }
    //    }
    //    private void CommandAction()
    //    {
    //        code here
    //    }
}
