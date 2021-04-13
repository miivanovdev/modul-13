using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class PrivilegedAccount : BankAccount
    {
        public PrivilegedAccount(decimal amount, decimal interestRate, int clientId, int departmentId, int minTerm, DateTime dateTime)
           : base(amount, interestRate, clientId, departmentId, minTerm, dateTime)
        {

        }

        protected override decimal CountIncome()
        {
            if(MonthPassed >= 0)
                return Capitalized(Amount, MonthPassed);

            return 0;
        }

        private decimal Capitalized(decimal amount, int month)
        {
            if (month == 0)
                return amount;

            return Capitalized(amount + (amount * InterestRate) / 100, --month);
        }

        public override bool CanWithdrawed => true;
        public override bool CanAdded => true;
    }
}
