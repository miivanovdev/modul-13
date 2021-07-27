

namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Класс посредник операции транзакции над банковскими счетами
    /// </summary>
    public abstract class TransactionMediator
    {        
        protected ITransactable Reciever { get; set; }
        protected ITransactable Sender { get; set; }
        public string Log { get; set; }

        protected readonly IWindow window;

        public TransactionMediator(IWindow window)
        {
            this.window = window;
        }

        public abstract  object GetReciever();

        /// <summary>
        /// Осуществить транзакцию
        /// </summary>
        public abstract void Transaction();        
    }
}
