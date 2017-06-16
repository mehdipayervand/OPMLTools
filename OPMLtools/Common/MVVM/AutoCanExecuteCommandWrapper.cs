// -----------------------------------------------------------------------
// <copyright file="AutoCanExecuteCommandWrapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OPMLtools.Common.MVVM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoCanExecuteCommandWrapper : ICommand
    {
        public ICommand WrappedCommand { get; private set; }

        public AutoCanExecuteCommandWrapper(ICommand wrappedCommand)
        {
            if (wrappedCommand == null)
            {
                throw new ArgumentNullException("wrappedCommand");
            }

            WrappedCommand = wrappedCommand;
        }

        public void Execute(object parameter)
        {
            WrappedCommand.Execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return WrappedCommand.CanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

}
