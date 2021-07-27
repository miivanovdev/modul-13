

namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Класс посредник операции транзакции над банковскими счетами
    /// </summary>
    public abstract class TransactionMediator
    {        
        /// <summary>
        /// Получатель транзакции
        /// </summary>
        protected ITransactable Reciever { get; set; }

        /// <summary>
        /// Отправитель транзакции
        /// </summary>
        protected ITransactable Sender { get; set; }

        /// <summary>
        /// Сообщение о проведенной транзакции
        /// </summary>
        public string Log { get; set; }
               
        /// <summary>
        /// Окно владелец диалога
        /// </summary>
        protected readonly IWindow window;

        public TransactionMediator(IWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Метод получения отправителя
        /// </summary>
        /// <returns></returns>
        public abstract  object GetReciever();

        /// <summary>
        /// Осуществить транзакцию
        /// </summary>
        public abstract void Transaction();        
    }
}
