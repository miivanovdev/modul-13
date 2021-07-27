using System;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.Dialogs
{
    public class ClientInputViewModel : ObservableObject
    {
        public ClientInputViewModel()
        {
            Client = new ClientsViewModel(new Clients());
        }

        public ClientsViewModel Client { get; set; }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult == value)
                    return;

                dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));
            }
        }

        private RelayCommand okCommand;

        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                (okCommand = new RelayCommand(new Action<object>(OkClose),
                                              new Func<object, bool>(CanClose)
                ));
            }
        }

        private void OkClose(object o)
        {
            DialogResult = true;
        }

        private bool CanClose(object o)
        {
            return Client.Error == string.Empty;
        }
    }
}
