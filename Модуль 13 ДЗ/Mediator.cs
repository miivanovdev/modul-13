using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.MVVM.Model;

namespace Модуль_13_ДЗ
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
