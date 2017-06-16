using System.Collections.Generic;
using System.ComponentModel;

namespace OPMLtools.Infrastructure.Models
{
    class OpmlModel : INotifyPropertyChanged
    {
		#region Fields (5) 

        private string _htmlUrl;
        private string _text;
        private string _title;
        private string _type;
        private string _xmlUrl;

		#endregion Fields 

		#region Properties (7) 

        //public IEnumerable<XElement> Ancestors { get; set; }
        public List<string> Ancestors { get; set; }

        public int Count { get; set; }

        public string HtmlUrl
        {
            get { return _htmlUrl; }
            set
            {
                if (value == null)
                    return;
                _htmlUrl = value;
                RaisePropertyChanged("HtmlUrl");
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null)
                    return;
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == null)
                    return;
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                if (value == null)
                    return;
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string XmlUrl
        {
            get { return _xmlUrl; }
            set
            {
                if (value == null)
                    return;
                _xmlUrl = value;
                RaisePropertyChanged("XmlUrl");
            }
        }

		#endregion Properties 

		#region Delegates and Events (1) 

		// Events (1) 

        public event PropertyChangedEventHandler PropertyChanged;

		#endregion Delegates and Events 

		#region Methods (1) 

		// Private Methods (1) 

        void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

		#endregion Methods 
    }
}

