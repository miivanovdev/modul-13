
using Модуль_13_ДЗ.Dialogs;


namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Посдердник операции транзакции со счета на счет 
    /// </summary>
    public class AccountToAccountMediator : TransactionMediator
    {        
        private readonly DialogTransactionViewModel transactionViewModel;

        /// <summary>
        /// Счет получателя
        /// </summary>
        public object RecieverAccount { get { return transactionViewModel.RecieverAccount; } }
        
        public AccountToAccountMediator(IWindow window, DialogTransactionViewModel transactionViewModel)
            :base(window)
        {
            this.transactionViewModel = transactionViewModel;
        }
                
        /// <summary>
        /// Метод проводящий транзакцию
        /// </summary>
        public override void Transaction()
        {
            if (window.ShowDialog() == true)
            {
                Sender = transactionViewModel.SenderAccount;
                Reciever = transactionViewModel.RecieverAccount;

                Sender.Withdraw(transactionViewModel.Amount);
                Reciever.Put(transactionViewModel.Amount);

                Log = $"Перевод со счета {Sender.Name} на счет {Reciever.Name} на сумму: {transactionViewModel.Amount}";
            }
        }

        public void Rollback()
        {
            Sender.Put(transactionViewModel.Amount);
            Reciever.Withdraw(transactionViewModel.Amount);
        }

        public override object GetReciever()
        {
            return transactionViewModel.RecieverAccount;
        }
    }
}
