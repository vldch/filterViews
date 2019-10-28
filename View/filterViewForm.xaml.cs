using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace filtersView.View
{
    /// <summary>
    /// Interaction logic for filterViewForm.xaml
    /// </summary>
    public partial class filterViewForm : Window, IDisposable
    {
        public filterViewForm()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
