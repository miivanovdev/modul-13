using System;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Посредник транзакции между клиентом и счетом
    /// </summary>
    class AccountToClientMediator : TransactionMediator
    {
        /// <summary>
        /// Флаг указывающий на операцию снятия со счета
        /// </summary>
        public bool IsWithdraw { get; private set; }

        public AccountToClientMediator(ITransactable client, ITransactable account, bool isWithdraw = false)
        {            
            IsWithdraw = isWithdraw;            
            Sender = client;
            Reciever = account;            
        }

        /// <summary>
        /// Метод проводящий транзакцию
        /// </summary>
        public override void Transaction()
        {
            if (Sender.BadHistory)
                throw new TransactionFailureException($"Клиент {Sender.Name} заблокирован!");

            if (Reciever.BadHistory)
                throw new TransactionFailureException($"Счет {Reciever.Name} заблокирован!");

            string label = IsWithdraw ? "Снятие со счета" : "Пополнение счета";

            DialogViewModel dialogVM = new DialogViewModel(label, Reciever.AmountAvailable, IsWithdraw);
            DialogWindow dialogWindow = new DialogWindow(dialogVM, label);

            if (dialogWindow.ShowDialog() == true)
            {
                if (IsWithdraw)
                {
                    Sender.Put(dialogVM.Amount);
                    Reciever.Withdraw(dialogVM.Amount);
                    LogMessage = new LogMessage($"{Sender.Name} снял со счета {Reciever.Name} сумму: {dialogVM.Amount}");
                }
                else
                {
                    Sender.Withdraw(dialogVM.Amount);
                    Reciever.Put(dialogVM.Amount);
                    LogMessage = new LogMessage($"{Sender.Name} пополнил счет {Reciever.Name} на сумму: {dialogVM.Amount}");
                }
            }          
        }
    }
}
