using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelLib
{
    public class PrivilegedAccount : BankAccount
    {
        public PrivilegedAccount() { }

        public PrivilegedAccount(decimal amount, decimal interestRate, int ownerId, string ownerName, int departmentId, int minTerm, DateTime dateTime)
            : base(amount, interestRate, ownerId, ownerName, departmentId, minTerm, dateTime)
        {

        }

        public override AccountType Type { get; set; }

        public override string Name
        {
            get { return $"Привелигированный счет {OwnerName} Id {OwnerId}"; }
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
        public override bool CanTransact => true;
        public override bool CanClose => true;
    }
}
