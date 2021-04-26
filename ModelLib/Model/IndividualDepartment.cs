using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ModelLib
{
    public class IndividualDepartment : BankDepartment<BankAccount>
    {
        public IndividualDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false, uint delay = 0)
           : base(log, name, minAmount, minTerm, rate, isEmpty, delay)
        {

        }

        public override AccountType AccountType => AccountType.IndividualAccount;
    }
}
