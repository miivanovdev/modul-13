using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    /// <summary>
    /// Поведение сущности участника транзакции
    /// </summary>
    public interface ITransactable
    {
        decimal AmountAvailable { get; }
        string Name { get; }
        void Put(decimal amount);
        void Withdraw(decimal amount, ITransactable sender = null);
    }
}
