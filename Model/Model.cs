using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace filtersView
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Model
    {
        private static UIApplication _uiapp;
        private static Application _app;
        private static UIDocument _uidoc;
        private static Document _doc;
        private ObservableCollection<ElementItem> _filtersView;
        private ObservableCollection<ElementItem> _views;

        public ObservableCollection<ElementItem> FiltersView { get { return _filtersView; } set { _filtersView = value; } }
        public ObservableCollection<ElementItem> Views { get { return _views; } set { _views = value; } }
        public Model(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _app = _uiapp.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _doc = _uidoc.Document;
            FiltersView = GetFiltersView();
            Views = GetViews();
        }
        private ObservableCollection<ElementItem> GetViews()
        {
            List<BuiltInCategory> builtInCategory = new List<BuiltInCategory>();
            builtInCategory.Add(BuiltInCategory.OST_Views);

            ElementMulticategoryFilter multuFilter = new ElementMulticategoryFilter(builtInCategory);
            var elements = new FilteredElementCollector(_doc)
                .WherePasses(multuFilter)
                .WhereElementIsNotElementType()
                .ToElements()
                .Select(x => new ElementItem(x.Id, x.Name))
                .Convert<ElementItem>();
            return elements;
        }
        private ObservableCollection<ElementItem> GetFiltersView()
        {
            return _doc.ActiveView.GetFilters().ToList().ConvertAll(x => _doc.GetElement(x)).Select(x => new ElementItem(x.Id, x.Name)).Convert<ElementItem>();
        }
    }

}
