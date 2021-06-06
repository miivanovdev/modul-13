﻿using System;
using System.Data.Linq.Mapping;

namespace ModelLib
{
    public class IndividualAccount : BankAccount
    {
        public IndividualAccount() { }

        public IndividualAccount(decimal amount, decimal interestRate, int ownerId, string ownerName, int departmentId, int minTerm, DateTime dateTime, int delay)
            : base(amount, interestRate, ownerId, ownerName, departmentId, minTerm, dateTime)
        {
            Delay = delay;
        }

        public override AccountType Type { get; set; } 
        
        public override string Name
        {
            get { return $"Индивидуальный счет {OwnerName} Id {OwnerId}"; }
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
