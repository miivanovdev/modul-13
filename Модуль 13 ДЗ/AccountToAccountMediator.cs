using System;
using System.Collections.Generic;
using System.Windows;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;


namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Посдердник операции транзакции со счета на счет 
    /// </summary>
    class AccountToAccountMediator : TransactionMediator
    {        
        private readonly TransactionViewModel transactionViewModel;

        /// <summary>
        /// Счет получателя
        /// </summary>
        public AccountsViewModel RecieverAccount { get; set; }

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal TransactionAmount { get; private set; }

        public AccountToAccountMediator(List<DepartmentsViewModel> departments, List<AccountsViewModel> accounts, ITransactable sender)
        {
            Sender = sender;
            transactionViewModel = new TransactionViewModel(departments, accounts, sender.AmountAvailable);
        }
                
        /// <summary>
        /// Метод проводящий транзакцию
        /// </summary>
        public override void Transaction()
        {            
            

            DialogTransaction dialogTransaction = new DialogTransaction(transactionViewModel);

            if (transactionViewModel == null || dialogTransaction == null)
                throw new TransactionFailureException("Ошибка при создании диалога транзакции!");

            if (dialogTransaction.ShowDialog() == true)
            {
                TransactionAmount = transactionViewModel.Amount;
                Reciever = transactionViewModel.SelectedAccount;
                RecieverAccount = transactionViewModel.SelectedAccount;

                Sender.Withdraw(TransactionAmount);
                Reciever.Put(TransactionAmount);

                Log = $"Перевод со счета {Sender.Name} на счет {Reciever.Name} на сумму: {transactionViewModel.Amount}";
            }
        }

        public void Rollback()
        {
            Sender.Put(TransactionAmount);
            Reciever?.Withdraw(TransactionAmount);
        }
    }
}
