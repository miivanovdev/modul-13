using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;


namespace Модуль_13_ДЗ.Dialogs
{
    public class DialogAccountToClientViewModel : ObservableObject
    {
        public DialogAccountToClientViewModel(AccountsViewModel selectedAccount, ClientsViewModel selectedClient, bool isWithdraw = false)
        {
            Data = new DialogAccountToClientModel(selectedAccount.AmountAvailable, isWithdraw);
            SelectedAccount = selectedAccount;
            SelectedClient = selectedClient;
        }

        public AccountsViewModel SelectedAccount { get; private set; }

        public ClientsViewModel SelectedClient { get; private set; }

        public DialogAccountToClientModel Data { get; set; }

        public decimal Amount
        {
            get
            {
                return Data.Amount;
            }
        }

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
            return Data.IsValid; 
        }
    }
}
