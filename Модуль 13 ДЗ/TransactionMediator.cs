using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Класс посредник операции транзакции над банковскими счетами
    /// </summary>
    abstract class TransactionMediator
    {        
        protected ITransactable Reciever { get; set; }
        protected ITransactable Sender { get; set; }
        public Log Log { get; set; }

        /// <summary>
        /// Осуществить транзакцию
        /// </summary>
        public abstract void Transaction();        
    }
}
