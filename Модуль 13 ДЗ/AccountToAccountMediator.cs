using System;
using System.Collections.Generic;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Посдердник операции транзакции со счета на счет 
    /// </summary>
    class AccountToAccountMediator : TransactionMediator
    {
        /// <summary>
        /// Коллекция счетов на которые возможен перевод
        /// </summary>
        public List<BankAccountViewModel> Accounts { get; private set; }

        /// <summary>
        /// Счет получателя
        /// </summary>
        public BankAccountViewModel RecieverAccount { get; set; }

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal TransactionAmount { get; private set; }

        public AccountToAccountMediator(List<BankAccountViewModel> accounts, ITransactable sender)
        {
            Accounts = accounts;
            Sender = sender;
        }
                
        /// <summary>
        /// Метод проводящий транзакцию
        /// </summary>
        public override void Transaction()
        {            
            if (Accounts == null || Sender == null || Accounts.Count == 0)
                throw new TransactionFailureException("Неверно переданы счета или отсутствует счет отправитель!");

            TransactionViewModel transactionViewModel = new TransactionViewModel(Accounts, Sender.AmountAvailable);
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

                Log = new Log($"Перевод со счета {Sender.Name} на счет {Reciever.Name} на сумму: {transactionViewModel.Amount}");
            }
        }

        public void Rollback()
        {
            Sender.Put(TransactionAmount);
            Reciever?.Withdraw(TransactionAmount);
        }
    }
}
