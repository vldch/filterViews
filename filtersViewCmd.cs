using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using filtersView.View;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace filtersView
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class filtersViewCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            if (commandData.Application.ActiveUIDocument.Document.IsFamilyDocument)
            {
                MessageBox.Show("Вы открыли семейство", "Ошибка");               
                return Result.Cancelled;
            }

            //Создаем экземпляр модели
            Model model = new Model(uiapp);
            //Создаем экземпляр презентации-модели
            ViewModelItems viewmodel = new ViewModelItems();
            //Создаем экземпляр внешней транзакции и передаем в него созданную модель
            ExternalEventHandler externalEventHandlerButton = new ExternalEventHandler(model);
            //Создаем внешнее событие и добавляем внешнюю транзакцию
            ExternalEvent ExEvent = ExternalEvent.Create(externalEventHandlerButton);
            //Передаем созданную модель во ViewModel
            viewmodel.RevitModel = model;
            //Добавляем во ViewModel внешнее событие
            viewmodel.ApplyEvent = ExEvent;
            //Создаем экземпляр формы
            var mainFilterView = new filterViewForm();
            mainFilterView.DataContext = viewmodel;
            mainFilterView.ShowDialog();
            return Result.Succeeded;
        }
    }
}
