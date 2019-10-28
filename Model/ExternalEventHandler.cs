using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace filtersView
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalEventHandler : IExternalEventHandler
    {
        public Model RevitModel;
        private Document _document;
        public ExternalEventHandler(Model model)
        {
            this.RevitModel = model;
        }
        public void Execute(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            _document = uidoc.Document;
            using (var transact = new Transaction(_document, "Apply new filters to selected Views"))
            {
                transact.Start();
                SetFiltersToView(RevitModel.FiltersView.Where(x=>x.IsSelected), RevitModel.Views.Where(x=>x.IsSelected));
                transact.Commit();
            }
        }
        public string GetName()
        {
            return "Set new Filters to selected Views from ActiveView";
        }
        public void SetFiltersToView(IEnumerable<ElementItem> filters,IEnumerable<ElementItem> views)
        {
            foreach (var viewId in views.Where(x => x.IsSelected))
            {
                foreach (var jfilter in filters)
                {
                    var viewset = _document.GetElement(viewId.ModelId) as Autodesk.Revit.DB.View;
                    var existfilviewset = viewset.GetFilters();
                    if (existfilviewset.Count != 0)
                    {
                        foreach (var setviewfil in existfilviewset)
                        {
                            if (setviewfil == jfilter.ModelId)
                            {
                                try
                                {
                                    viewset.RemoveFilter(setviewfil);
                                    viewset.AddFilter(jfilter.ModelId);
                                    viewset.SetFilterOverrides(jfilter.ModelId, _document.ActiveView.GetFilterOverrides(jfilter.ModelId));
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    viewset.AddFilter(jfilter.ModelId);
                                    viewset.SetFilterOverrides(jfilter.ModelId, _document.ActiveView.GetFilterOverrides(jfilter.ModelId));
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            viewset.AddFilter(jfilter.ModelId);
                            viewset.SetFilterOverrides(jfilter.ModelId, _document.ActiveView.GetFilterOverrides(jfilter.ModelId));
                        }
                        catch { }
                    }
                }
            }
        }
    }
}