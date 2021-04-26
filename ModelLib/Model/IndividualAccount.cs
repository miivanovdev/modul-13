using System;
using System.Collections.Generic;

namespace ModelLib
{
    public class IndividualAccount : BankAccount
    {
        public IndividualAccount(decimal amount, decimal interestRate, int ownerId, string ownerName, int departmentId, int minTerm, DateTime dateTime, int delay)
            : base(amount, interestRate, ownerId, ownerName, departmentId, minTerm, dateTime)
        {
            Delay = delay;
        }

        public override AccountType Type
        {
            get { return AccountType.IndividualAccount; }
        }

        public override string Name
        {
            get { return $"Индивидуальный счет {OwnerName} Id {OwnerId}"; }
        }

        private int delay;
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                if (value > MinTerm)
                    value = 1;

                delay = value;
            }
        }

        public override bool CanAdded
        {
            get
            {
                if (CreatedDate.AddMonths(Delay) < CurrentDate)
                    return true;

                return false;
            }
        }

        public override bool CanWithdrawed
        {
            get
            {
                if (CurrentDate > CreatedDate.AddMonths(MinTerm - Delay))
                    return true;

                return false;
            }
        }

        public override bool CanTransact
        {
            get { return CanWithdrawed; }
        }

        public override bool CanClose => CanWithdrawed;
    }
}
