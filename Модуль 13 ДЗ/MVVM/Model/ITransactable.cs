using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    interface ITransactable
    {
        decimal AmountAvailable { get; }
        void Put(decimal amount);
        void Withdraw(decimal amount);
    }
}
