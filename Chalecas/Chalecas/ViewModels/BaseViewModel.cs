using System;
using System.ComponentModel;

namespace Chalecas.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged // interfaces implemented
    {
        #region props
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public BaseViewModel()
        {
        }

    }
}
