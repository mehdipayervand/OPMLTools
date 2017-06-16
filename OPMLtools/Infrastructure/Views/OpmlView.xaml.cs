using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OPMLtools.Infrastructure.ViewModels;

namespace OPMLtools.Infrastructure.Views
{
    /// <summary>
    /// Interaction logic for OpmlView.xaml
    /// </summary>
    public partial class OpmlView : UserControl
    {
        public OpmlView()
        {
            InitializeComponent();
            //OpmlViewModel vm=new OpmlViewModel();
            //this.DataContext = vm;
            //txtPath.Text = @"C:\Users\heKTor\Desktop\google-reader-subscriptions - Copy.xml";
            //Mouse.OverrideCursor = Cursors.Wait;
            //try
            //{
            //    MessageBox.Show(this.Cursor.ToString());
            //}
            //catch (Exception ex
            //    )
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnGetPath_Click(object sender, RoutedEventArgs e)
        {
            //todo: Refactor this part early
            var fileDialog = new OpenFileDialog { Filter = "XML Files (*.XML)|*.xml" };
            fileDialog.ShowDialog();
            txtPath.Text = fileDialog.FileName;

            //BindingExpression be = txtPath.GetBindingExpression(TextBox.TextProperty);
            //be.UpdateSource();

            //CommandManager.InvalidateRequerySuggested();
        }
    }
}
