using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.MVVM.Model;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class PhysicalDepartment : BankDepartment<BankAccount>
    {
        public PhysicalDepartment(string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false, uint delay = 0)
            : base(name, minAmount, minTerm, rate, isEmpty, delay)
        {

        }

        public override AccountType AccountType => AccountType.PhysicalAccount;        
    }
}
