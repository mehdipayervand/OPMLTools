using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OPMLtools.Common.Collections;
using OPMLtools.Infrastructure.Models;
using System.Reflection;
using log4net.Config;
using log4net;

namespace OPMLtools.Infrastructure.Core
{
    class Opml
    {
		#region Fields (2) 

        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static XDocument XDocument;

		#endregion Fields 

		#region Constructors (1) 

        public Opml()
        {
            XmlConfigurator.Configure();
        }

		#endregion Constructors 

		#region Methods (3) 

		// Public Methods (2) 

        public static void CreateOpml(MtObservableCollection<OpmlModel> opmlCollection, string headTitle, string savePath)
        {
            Init(headTitle);
            var body = XDocument.Document.Element("opml").Element("body");

            foreach (var opmlModel in opmlCollection)
            {
                if (opmlModel.Ancestors.Count == 0)
                {
                    body.Add(new XElement("outline", new XAttribute("text", opmlModel.Text),
                                          new XAttribute("title", opmlModel.Title),
                                          new XAttribute("type", opmlModel.Type),
                                          new XAttribute("xmlUrl", opmlModel.XmlUrl),
                                          new XAttribute("htmlUrl", opmlModel.HtmlUrl)));
                }
                else
                {
                    AddAncestors(opmlModel.Ancestors).Add(new XElement("outline", new XAttribute("text", opmlModel.Text),
                                                                       new XAttribute("title", opmlModel.Title),
                                                                       new XAttribute("type", opmlModel.Type),
                                                                       new XAttribute("xmlUrl", opmlModel.XmlUrl),
                                                                       new XAttribute("htmlUrl", opmlModel.HtmlUrl)));
                }

            }

            var fileInfo = new FileInfo(savePath);
            var bakSavePath = fileInfo.FullName + ".bak";
            try
            {

                File.Copy(savePath, bakSavePath);
                Common.MVVM.App.AddMessage("Make backup copies of your OPML file", LogModel.LogType.Info);

                XDocument.Save(savePath);
                Common.MVVM.App.AddMessage("Your OPML file has been updated successfully", LogModel.LogType.Info);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                Common.MVVM.App.AddMessage("Error in saving file on disk", LogModel.LogType.Error);
            }
        }

        public static void Init(string title)
        {
            XDocument = new XDocument();
            XDocument = new XDocument(new XDeclaration("1.0", "utf-8", ""),
                                       new XElement("opml", new XAttribute("version", "1.0"),
                                                    new XElement("head", new XElement("title", title.Trim())),
                                                    new XElement("body", "")));

        }
		// Private Methods (1) 

        private static XElement AddAncestors(IEnumerable<string> ancList)
        {

            XElement bodyXElement = XDocument.Document.Element("opml").Element("body");

            XElement lastInsertedXElemet = null;
            foreach (var anc in ancList.Reverse())
            {
                XElement desireXElement = null;
                var existXElement = (from item in bodyXElement.Descendants("outline")
                                     let tag = item.Attribute("htmlUrl")
                                     where tag == null
                                     select item).ToList();

                if (existXElement.Count > 0)
                {

                    desireXElement = existXElement.Where(w => (string)w.Attribute("title") == anc).FirstOrDefault();
                }

                if (desireXElement == null)
                {
                    lastInsertedXElemet = new XElement("outline", new XAttribute("title", anc), new XAttribute("text", anc));
                    bodyXElement.Add(lastInsertedXElemet);
                }
                else
                {
                    lastInsertedXElemet = desireXElement;
                    bodyXElement = desireXElement;

                }
            }

            return lastInsertedXElemet;
        }

		#endregion Methods 
    }
}
