using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class PhysicalAccount : BankAccount
    {
        public PhysicalAccount(decimal amount, decimal interestRate, int ownerId, string ownerName, int departmentId, int minTerm, DateTime dateTime)
            : base(amount, interestRate, ownerId, ownerName, departmentId, minTerm, dateTime)
        {

        }

        public override AccountType Type
        {
            get { return AccountType.PhysicalAccount; }
        }

        public override string AccountName
        {
            get { return $"Физический счет {OwnerName}"; }
        }

        public override bool CanAdded => true;
        
        public override bool CanWithdrawed
        {
            get
            {
                if (CurrentDate == DateTime.Now)
                    return true;

                return false;
            }
        }

        public override bool CanClose
        {
            get
            {
                if (CreatedDate.AddMonths(MinTerm) == CurrentDate)
                    return true;

                return false;
            }
        }

        public override bool CanTransact
        {
            get { return CanWithdrawed;  }
        }
    }
}

