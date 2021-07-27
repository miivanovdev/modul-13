using System;
using Модуль_13_ДЗ.Dialogs;

namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Посредник транзакции между клиентом и счетом
    /// </summary>
    public class AccountToClientMediator : TransactionMediator
    {
        public AccountToClientMediator(IWindow window, DialogAccountToClientViewModel dialogVM, bool isWithdraw = false)
           : base(window)
        {
            if (dialogVM == null)
                throw new ArgumentNullException("dialogVM");

            this.dialogVM = dialogVM;

            Sender = dialogVM.SelectedClient;
            Reciever = dialogVM.SelectedAccount;
        }

        private readonly DialogAccountToClientViewModel dialogVM;

        public bool IsWithdraw { get { return dialogVM.Data.IsWithdraw; } }

        /// <summary>
        /// Метод проводящий транзакцию
        /// </summary>
        public override void Transaction()
        {
            if (Sender.BadHistory)
                throw new TransactionFailureException($"Клиент {Sender.Name} заблокирован!");

            if (Reciever.BadHistory)
                throw new TransactionFailureException($"Счет {Reciever.Name} заблокирован!");
                       

            if (window.ShowDialog() == true)
            {
                if (IsWithdraw)
                {
                    Sender.Put(dialogVM.Amount);
                    Reciever.Withdraw(dialogVM.Amount);
                    Log = $"{Sender.Name} снял со счета {Reciever.Name} сумму: {dialogVM.Amount}";
                }
                else
                {
                    Sender.Withdraw(dialogVM.Amount);
                    Reciever.Put(dialogVM.Amount);
                    Log = $"{Sender.Name} пополнил счет {Reciever.Name} на сумму: {dialogVM.Amount}";
                }
            }          
        }

        public override object GetReciever()
        {
            return dialogVM.SelectedAccount;
        }
    }
}
