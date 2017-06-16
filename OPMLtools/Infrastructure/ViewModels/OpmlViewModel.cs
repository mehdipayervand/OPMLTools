using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using OPMLtools.Common.Collections;
using OPMLtools.Infrastructure.Models;
using OPMLtools.Common.MVVM;
using System.Xml.Linq;
using OPMLtools.Common.ExtensionMethods;
using System.Reflection;
using log4net.Config;
using log4net;

namespace OPMLtools.Infrastructure.ViewModels
{
    class OpmlViewModel : INotifyPropertyChanged
    {
        #region Fields (5)

        private string _filePath;
        private string _headTitle;
        private bool _isBusy;
        private MtObservableCollection<OpmlModel> _opmls;
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Fields

        #region Constructors (1)

        public OpmlViewModel()
        {
            GetOpmlCommand = new DelegateCommand<string>(doGetOpml, canDoGetOpml);
            RemoveDuplicateCommand = new DelegateCommand<string>(doRemoveDuplicate, canDoRemoveDuplicate);
            Opmls = new MtObservableCollection<OpmlModel>();

            XmlConfigurator.Configure();
        }

        #endregion Constructors

        #region Properties (6)

        public string FilePath
        {
            get { return this._filePath; }
            set
            {
                if (value == null)
                    return;
                this._filePath = value;
                RaisePropertyChanged("FilePath");

                //enable Button
                if (!string.IsNullOrWhiteSpace(_filePath))
                {
                    GetOpmlCommand.CanExecute(FilePath);
                }
            }
        }

        public DelegateCommand<string> GetOpmlCommand { get; set; }

        public DelegateCommand<string> RemoveDuplicateCommand { get; set; }

        public string HeadTitle
        {
            get
            {
                return _headTitle;
            }
            set
            {
                if (value == _headTitle)
                    return;
                _headTitle = value;
                //this.RaisePropertyChanged("HeadTitle");

                //enable Button
                if (!string.IsNullOrWhiteSpace(_headTitle))
                {
                    //Perform cross thread operation
                    Application.Current.Dispatcher.Invoke(new Action(() => this.RemoveDuplicateCommand.CanExecute(this.HeadTitle)));
                }
            }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                this._isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public MtObservableCollection<OpmlModel> Opmls
        {
            get { return this._opmls; }
            set
            {
                if (value == null)
                    return;

                this._opmls = value;
                RaisePropertyChanged("Opmls");

                //todo:Not Work
                //if (Opmls != null &&Opmls.Count>0)
                //{
                //    RemoveDuplicateCommand.CanExecute(Opmls.Count);
                //}

            }
        }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Delegates and Events

        #region Methods (7)

        // Private Methods (7) 

        private bool canDoGetOpml(string path)
        {
            return !string.IsNullOrWhiteSpace(FilePath);
        }

        private bool canDoRemoveDuplicate(string arg)
        {
            return !string.IsNullOrWhiteSpace(_headTitle);

            //todo: Verify why this part not work
            //return Opmls.Count > 0;
        }

        private void doGetOpml(string path)
        {
            IsBusy = true;
            new Thread(GetOpmlAsync).Start();
        }

        private void doRemoveDuplicate(string obj)
        {
            IsBusy = true;
            new Thread(RemoveDuplicate).Start();

        }

        private void GetOpmlAsync()
        {
            Common.MVVM.App.AddMessage("Start try get OPML file", LogModel.LogType.Info);
            Opmls.Clear();
            try
            {
                var xDocument = XDocument.Load(FilePath);
                if (!xDocument.Descendants("outline").Any())
                {
                    Common.MVVM.App.AddMessage("There is no outline element in OPML file.", LogModel.LogType.Error);
                    return;
                }
                var xmlResults = from item in xDocument.Descendants("outline").AsParallel()
                                 where item.Attribute("htmlUrl") != null
                                 select
                                     new OpmlModel
                                         {
                                             Text = (string)item.Attribute("text"),
                                             Title = (string)item.Attribute("title"),
                                             Type = (string)item.Attribute("type"),
                                             HtmlUrl = (string)item.Attribute("htmlUrl"),
                                             XmlUrl = (string)item.Attribute("xmlUrl"),
                                             Ancestors =
                                                 (item.Ancestors().TakeWhile(tw => tw.Name != "body").Select(
                                                     s => s.Attribute("text").Value)).ToList()
                                         };

                var opmlGroup = from xmlResult in xmlResults
                                select
                                    new OpmlModel
                                        {
                                            Count =
                                                xmlResults.GroupBy(i => i.HtmlUrl).Where(
                                                    w => w.Key == xmlResult.HtmlUrl).Select(g => g.Count()).
                                                FirstOrDefault(),
                                            Text = xmlResult.Text,
                                            Title = xmlResult.Title,
                                            Type = xmlResult.Type,
                                            HtmlUrl = xmlResult.HtmlUrl,
                                            XmlUrl = xmlResult.XmlUrl,
                                            Ancestors = xmlResult.Ancestors
                                        };


                foreach (var xmlOpml in opmlGroup.DistinctBy(d => d.HtmlUrl).OrderByDescending(o => o.Count))
                {
                    Opmls.Add(xmlOpml);
                }

                IsBusy = false;
                Common.MVVM.App.AddMessage("End try get OPML file", LogModel.LogType.Info);

                //get OPMl head title
                if (Opmls.Count <= 0) return;
                var xElement = xDocument.Element("opml");
                if (xElement != null)
                    HeadTitle = (string)xElement.Element("head").Element("title");
                else
                {
                    HeadTitle = "subscriptions in Google Reader";
                    Common.MVVM.App.AddMessage("Head element not exists.default head used instead.", LogModel.LogType.Warn);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                Common.MVVM.App.AddMessage("Error in loading OPML file.", LogModel.LogType.Error);
                IsBusy = false;
                //Inactive RemoveDuplicate button
                HeadTitle = string.Empty;
                return;
            }
            finally
            {
                IsBusy = false;
            }
        }

        void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RemoveDuplicate()
        {
            if (Opmls.Count <= 0) return;
            if (string.IsNullOrWhiteSpace(FilePath.Trim())) return;

            Core.Opml.CreateOpml(Opmls, HeadTitle, FilePath);
            IsBusy = false;

        }

        #endregion Methods
    }
}
