using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class TransactionViewModel : ObservableObject
    {
        public TransactionViewModel(List<BankAccountViewModel> accounts, decimal CurrentAmount)
        {
            this.accounts = new List<BankAccountViewModel>(accounts);
            Data = new DialogDataModel("Перевод", CurrentAmount, true);
        }

        public decimal Amount { get { return Data.Amount; } }

        public DialogDataModel Data { get; set; }

        /// <summary>
        /// Все счета
        /// </summary>
        private List<BankAccountViewModel> accounts;

        /// <summary>
        /// Отфильтрованные счета
        /// </summary>
        public ReadOnlyCollection<BankAccountViewModel> Accounts
        {
            get { return new ReadOnlyCollection<BankAccountViewModel>(GetSubset()); }
        }

        /// <summary>
        /// Фильтр по типу счета
        /// </summary>
        private AccountType accountType;
        public AccountType AccountType
        {
            get { return accountType; }
            set
            {
                if (accountType != value)
                {
                    accountType = value;
                    NotifyPropertyChanged(nameof(AccountType));
                    NotifyPropertyChanged(nameof(Accounts));
                }
            }
        }

        private List<BankAccountViewModel> GetSubset()
        {
            return accounts.Where(x => x.AccountType == AccountType).ToList();
        }

        /// <summary>
        /// Результат диалога
        /// </summary>
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

        private BankAccountViewModel selectedAccount;
        /// <summary>
        /// Выбранный счет получатель
        /// </summary>
        public BankAccountViewModel SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                if (selectedAccount == value)
                    return;

                selectedAccount = value;
                Data.TotalAmount = selectedAccount.Amount;
                NotifyPropertyChanged(nameof(SelectedAccount));
            }
        }

        private RelayCommand okCommand;
        /// <summary>
        /// Команда закрытия при положительном результате диалога
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
        /// Положительное закрытие диалога
        /// </summary>
        /// <param name="o"></param>
        private void OkClose(object o)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Проверка допустимости положительного исхода диалога
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClose(object o)
        {
            return SelectedAccount != null &&
                   Data.IsValid;
        }
    }
}
