using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Autodesk.Revit.UI;
using filtersView.ViewModel;


namespace filtersView
{
    public class ViewModelItems : INotifyPropertyChanged
    {
        private ObservableCollection<ElementItem> views;
        private ObservableCollection<ElementItem> filtersView;
        private string _searchText;
        private Model revitmodel;
        public ExternalEvent ApplyEvent;
        private ICommand _filtersViewCommand;
        private ICommand _viewsCommand;
        internal Model RevitModel
        {
            get
            {
                return revitmodel;
            }
            set
            {
                revitmodel = value;
            }
        }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand ApplyWindowCommand { get; set; }
        public ICommand FiltersViewCommand => _filtersViewCommand ?? (_filtersViewCommand = new RelayCommandViews(param => this.SelectUnselectAllFiltersView(Convert.ToBoolean(param.ToString()))));
        public ICommand ViewsCommand => _viewsCommand ?? (_viewsCommand = new RelayCommandViews(param => this.SelectUnselectAllViews(Convert.ToBoolean(param.ToString()))));
        /// <summary>
        /// Метод для выделения и снятия выделения в для FilterView
        /// </summary>
        /// <param name="isSelected"></param>
        private void SelectUnselectAllFiltersView(bool isSelected)
        {
            for (int i = 0; i < this.FiltersView.Count; i++)
            {
                this.FiltersView[i].IsSelected = isSelected;
            }
        }
        private void SelectUnselectAllViews(bool isSelected)
        {
            for (int i = 0; i < this.Views.Count; i++)
            {
                this.Views[i].IsSelected = isSelected;
            }
        }
        public ObservableCollection<ElementItem> FiltersView
        {
            get
            {
                filtersView = RevitModel.FiltersView;
                if (SearchTextFiltersView == null)
                    return filtersView;
                return filtersView.Where(x => x.ItemName.ToUpper().StartsWith(SearchTextFiltersView.ToUpper())).Convert();
            }
        }
        public ObservableCollection<ElementItem> Views
        {
            get
            {
                views = RevitModel.Views;
                if (SearchTextViews == null)
                    return views;
                return views.Where(x => x.ItemName.ToUpper().StartsWith(SearchTextViews.ToUpper())).Convert();
            }
        }
        public string SearchTextFiltersView
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchTextFiltersView");
                OnPropertyChanged("FiltersView");
            }
        }
        public string SearchTextViews
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchTextViews");
                OnPropertyChanged("Views");
            }
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        private void ApplyWindow(Window window)
        {
            ApplyEvent.Raise();
            if (window != null)
            {
                window.Close();
            }
        }
        public ViewModelItems()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            this.ApplyWindowCommand = new RelayCommand<Window>(this.ApplyWindow);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}
