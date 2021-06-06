using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class PhysicalAccount : BankAccount
    {
        public PhysicalAccount() { }

        public PhysicalAccount(decimal amount, decimal interestRate, int ownerId, string ownerName, int departmentId, int minTerm, DateTime dateTime)
            : base(amount, interestRate, ownerId, ownerName, departmentId, minTerm, dateTime)
        {
            
        }
        
        public override AccountType Type { get; set; }

        public override string Name
        {
            get { return $"Физический счет {OwnerName} Id {OwnerId}"; }
        }

        public override bool CanAdded => true;
        
        public override bool CanWithdrawed
        {
            get
            {
                if (CreatedDate.AddMonths(MinTerm) <= CurrentDate)
                    return true;

                return false;
            }
        }

        public override bool CanClose
        {
            get { return Amount == 0; }
        }
        
        public override bool CanTransact
        {
            get { return CanWithdrawed;  }
        }
    }
}

