using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class AccountToAccountMediator : Mediator
    {
        public List<BankAccount> Accounts { get; private set; }

        public AccountToAccountMediator(List<BankAccount> accounts, ITransactable sender)
        {
            Accounts = accounts;
            Sender = sender;
        }

        public override void Transaction()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel(Accounts, Sender.AmountAvailable);
            DialogTransaction dialogTransaction = new DialogTransaction(transactionViewModel);

            if (dialogTransaction.ShowDialog() == true)
            {
                Reciever = transactionViewModel.SelectedAccount;

                Sender.Withdraw(transactionViewModel.Amount);
                Reciever.Put(transactionViewModel.Amount);
            }
        }
    }
}
