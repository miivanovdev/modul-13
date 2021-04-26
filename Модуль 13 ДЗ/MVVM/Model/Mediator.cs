using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    /// <summary>
    /// Класс посредник операции транзакции над банковскими счетами
    /// </summary>
    abstract class Mediator
    {        
        protected ITransactable Reciever { get; set; }
        protected ITransactable Sender { get; set; }

        /// <summary>
        /// Осуществить транзакцию
        /// </summary>
        public abstract void Transaction();        
    }
}
