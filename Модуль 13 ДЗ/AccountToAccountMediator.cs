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
        public List<BankAccount> Accounts { get; private set; }

        public AccountToAccountMediator(List<BankAccount> accounts, ITransactable sender)
        {
            Accounts = accounts;
            Sender = sender;
        }
                
        public override void Transaction()
        {
            try
            {
                if (Accounts == null || Sender == null || Accounts.Count == 0)
                    throw new TransactionFailureException("Неверно переданы счета или отсутствует счет отправитель!");

                TransactionViewModel transactionViewModel = new TransactionViewModel(Accounts, Sender.AmountAvailable);
                DialogTransaction dialogTransaction = new DialogTransaction(transactionViewModel);

                if (transactionViewModel == null || dialogTransaction == null)
                    throw new TransactionFailureException("Ошибка при создании диалога транзакции!");

                if (dialogTransaction.ShowDialog() == true)
                {
                    Reciever = transactionViewModel.SelectedAccount;

                    Sender.Withdraw(transactionViewModel.Amount, Reciever);
                    Reciever.Put(transactionViewModel.Amount);
                }
            }
            catch (TransactionFailureException ex)
            {
                MessageBox.Show(ex.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
