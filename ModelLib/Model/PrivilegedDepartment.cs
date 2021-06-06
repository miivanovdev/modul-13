using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelLib
{
    public class PrivilegedDepartment : BankDepartment<BankAccount>
    {
        public PrivilegedDepartment() { } 

        public PrivilegedDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, int minTerm, decimal rate, bool isEmpty = false, int delay = 0)
           : base(log, name, minAmount, minTerm, rate, delay)
        {

        }

        public override AccountType Type { get; set; } = AccountType.PrivilegedAccount;
    }
}
