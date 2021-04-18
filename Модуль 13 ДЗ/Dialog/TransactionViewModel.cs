using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.MVVM.Model;

namespace Модуль_13_ДЗ
{
    public class TransactionViewModel : ObservableObject
    {
        public TransactionViewModel(List<BankAccount> accounts, decimal CurrentAmount)
        {
            this.accounts = new ReadOnlyCollection<BankAccount>(accounts);
            Data = new DialogDataModel("Перевод", CurrentAmount, true);
        }

        public decimal Amount { get { return Data.Amount; } }
        public DialogDataModel Data { get; set; }

        private ReadOnlyCollection<BankAccount> accounts;
        public ReadOnlyCollection<BankAccount> Accounts
        {
            get { return new ReadOnlyCollection<BankAccount>(accounts.Where(x => x.Type == this.AccountType).ToList()); ; }
        }

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

        private BankAccount selectedAccount;

        public BankAccount SelectedAccount
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
            return SelectedAccount != null &&
                   Data.IsValid;
        }
    }
}
