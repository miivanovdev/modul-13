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
    /// <summary>
    /// Модель представление для диалога
    /// взаимодействия между клиентом и счетом
    /// </summary>
    public class DialogAccountToClientViewModel : ObservableObject
    {
        public DialogAccountToClientViewModel(AccountsViewModel selectedAccount, ClientsViewModel selectedClient, bool isWithdraw = false)
        {
            Data = new DialogAccountToClientModel(selectedAccount.AmountAvailable, isWithdraw);
            SelectedAccount = selectedAccount;
            SelectedClient = selectedClient;
        }

        /// <summary>
        /// Выбранный счет
        /// </summary>
        public AccountsViewModel SelectedAccount { get; private set; }

        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public ClientsViewModel SelectedClient { get; private set; }

        /// <summary>
        /// Модель данных диалогового окна
        /// </summary>
        public DialogAccountToClientModel Data { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal Amount
        {
            get
            {
                return Data.Amount;
            }
        }

        private bool? dialogResult;
        /// <summary>
        /// Флаг закрытия диалога
        /// </summary>
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
        /// <summary>
        /// Команда закрытия дилога
        /// </summary>
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

        /// <summary>
        /// Метод при закрытии диалога
        /// </summary>
        /// <param name="o"></param>
        private void OkClose(object o)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Доступность команды закрытия
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClose(object o)
        {
            return Data.IsValid; 
        }
    }
}
