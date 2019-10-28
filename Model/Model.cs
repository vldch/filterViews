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
        private static UIApplication UIAPP = null;
        private static Application APP = null;
        private static UIDocument UIDOC = null;
        private static Document DOC = null;
        private ObservableCollection<ElementItem> _filtersView;
        private ObservableCollection<ElementItem> _views;

        public ObservableCollection<ElementItem> FiltersView { get { return _filtersView; } set { _filtersView = value; } }
        public ObservableCollection<ElementItem> Views { get { return _views; } set { _views = value; } }
        public Model(UIApplication uiapp)
        {
            UIAPP = uiapp;
            APP = UIAPP.Application;
            UIDOC = UIAPP.ActiveUIDocument;
            DOC = UIDOC.Document;
            FiltersView = GetFiltersView();
            Views = GetViews();
        }
        private ObservableCollection<ElementItem> GetViews()
        {
            ObservableCollection<ElementItem> elementItems = new ObservableCollection<ElementItem>();
            List<BuiltInCategory> builtInCategory = new List<BuiltInCategory>();
            builtInCategory.Add(BuiltInCategory.OST_Views);

            ElementMulticategoryFilter multuFilter = new ElementMulticategoryFilter(builtInCategory);
            IList<Element> elements = new FilteredElementCollector(DOC).WherePasses(multuFilter).WhereElementIsNotElementType().ToElements();
            foreach (var element in elements)
            {
                ElementItem elementItem = new ElementItem();
                elementItem.ModelId = element.Id;
                elementItem.ItemName = element.Name;
                elementItems.Add(elementItem);
            }
            return elementItems;
        }
        private ObservableCollection<ElementItem> GetFiltersView()
        {
            ObservableCollection<ElementItem> elementItems = new ObservableCollection<ElementItem>();
            var filterViews = DOC.ActiveView.GetFilters().ToList().ConvertAll(x => DOC.GetElement(x));
            foreach (var filterView in filterViews)
            {
                var elementItem = new ElementItem();
                elementItem.ItemName = filterView.Name;
                elementItem.ModelId = filterView.Id;
                elementItems.Add(elementItem);
            }
            return elementItems;
        }

    }

}
