
namespace OPMLtools.Common.MVVM
{
    using System;

    using OPMLtools.Infrastructure.Models;

    public class App
    {
		#region Fields (1) 

        readonly static Messenger _messenger = new Messenger();

		#endregion Fields 

		#region Properties (1) 

        public static Messenger Messenger
        {
            get { return _messenger; }
        }

		#endregion Properties 

		#region Methods (1) 

		// Public Methods (1) 

        public static void AddMessage(string msg,LogModel.LogType logType)
        {
            App.Messenger.NotifyColleagues("DoLog", new LogModel
            {
                Message = msg,
                Type = logType,
                Time = DateTime.Now.ToLongTimeString()
            });
        }

		#endregion Methods 
    }


}
