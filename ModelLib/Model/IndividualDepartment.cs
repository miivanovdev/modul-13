using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelLib
{
    public class IndividualDepartment : BankDepartment<BankAccount>
    {
        public IndividualDepartment() { }

        public IndividualDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, int minTerm, decimal rate, bool isEmpty = false, int delay = 0)
           : base(log, name, minAmount, minTerm, rate, delay)
        {

        }

        public override AccountType Type { get; set; } = AccountType.IndividualAccount;
    }
}
