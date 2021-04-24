using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    abstract class Mediator
    {        
        protected ITransactable Reciever { get; set; }
        protected ITransactable Sender { get; set; }

        public abstract void Transaction();        
    }
}
