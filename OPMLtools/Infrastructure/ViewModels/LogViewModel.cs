// -----------------------------------------------------------------------
// <copyright file="LogViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using OPMLtools.Common.Collections;
using OPMLtools.Infrastructure.Models;

namespace OPMLtools.Infrastructure.ViewModels
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogViewModel : INotifyPropertyChanged
    {
        #region Fields (1)

        private MtObservableCollection<LogModel> logs;

        #endregion Fields

        #region Constructors (1)

        public LogViewModel()
        {

            Logs = new MtObservableCollection<LogModel>();
            //var logModel1 = new LogModel { Message = "Just ERROR Test", Time = DateTime.Now ,Type = LogModel.LogType.Error};
            //var logModel2 = new LogModel { Message = "Just INFO Test", Time = DateTime.Now, Type = LogModel.LogType.Info };
            //logs.Add(logModel1);
            //logs.Add(logModel2);
            Common.MVVM.App.Messenger.Register<LogModel>("DoLog", DoLog);
        }

        #endregion Constructors

        #region Properties (1)

        public MtObservableCollection<LogModel> Logs
        {
            get
            {
                return this.logs;
            }
            set
            {
                if (value == this.logs)

                    return;
                this.logs = value;
                this.RaisePropertyChanged("Logs");
            }
        }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Delegates and Events

        #region Methods (2)

        // Private Methods (2) 

        private void DoLog(LogModel logData)
        {
            logs.Add(logData);
        }

        void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
