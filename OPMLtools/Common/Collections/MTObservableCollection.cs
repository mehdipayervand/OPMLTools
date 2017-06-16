using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace OPMLtools.Common.Collections
{
    //from here http://julmar.com/blog/mark/?p=41
    public class MtObservableCollection<T> : ObservableCollection<T>
    {
        #region Delegates and Events (1)

        // Events (1) 

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion Delegates and Events

        #region Methods (1)

        // Protected Methods (1) 

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var eh = CollectionChanged;
            if (eh == null) return;

            var dispatcher = (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                              let dpo = nh.Target as DispatcherObject
                              where dpo != null
                              select dpo.Dispatcher).FirstOrDefault();

            if (dispatcher != null && dispatcher.CheckAccess() == false)
            {
                dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => OnCollectionChanged(e)));
            }
            else
            {
                foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                    nh.Invoke(this, e);
            }
        }

        #endregion Methods
    }
}
