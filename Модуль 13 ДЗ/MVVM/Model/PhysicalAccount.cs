using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class PhysicalAccount : BankAccount
    {
        public PhysicalAccount(decimal amount, decimal interestRate, int clientId, int departmentId, int minTerm, DateTime dateTime)
            : base(amount, interestRate, clientId, departmentId, minTerm, dateTime)
        {

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
    }
}

