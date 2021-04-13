using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    interface IAccountable
    {
        void OpenAccount(IList<BankAccount> accounts, Client client);
        void CloseAccount(IList<BankAccount> accounts, Client client);
        void Transact(IList<BankAccount> accounts, Client client);
        void Put(IList<BankAccount> accounts, Client client);
        void Withdraw(IList<BankAccount> accounts, Client client);
    }
}
