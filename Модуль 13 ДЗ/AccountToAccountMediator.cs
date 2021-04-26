using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Модуль_13_ДЗ.MVVM.Model
{
    /// <summary>
    /// Посдердник операции транзакции со счета на счет 
    /// </summary>
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

                    Sender.Withdraw(transactionViewModel.Amount, true);
                    Reciever.Put(transactionViewModel.Amount, Sender);
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
