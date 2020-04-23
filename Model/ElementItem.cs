using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Autodesk.Revit.DB;

namespace filtersView
{
    public class ElementItem : INotifyPropertyChanged
    {
        private ElementId _modelId;
        private string _itemName;
        private bool _isSelected;
        public ElementId ModelId
        {
            get => _modelId;
            set
            {
                _modelId = value;
                OnPropertyChanged(nameof(ModelId));
            }
        }
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        public ElementItem(ElementId id,string name)
        {
            ModelId = id;
            ItemName = name;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
