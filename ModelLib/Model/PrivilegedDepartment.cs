using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelLib
{
    public class PrivilegedDepartment : BankDepartment<BankAccount>
    {
        public PrivilegedDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false, uint delay = 0)
           : base(log, name, minAmount, minTerm, rate, isEmpty, delay)
        {

        }

        public override AccountType AccountType => AccountType.PrivilegedAccount;   
    }
}
