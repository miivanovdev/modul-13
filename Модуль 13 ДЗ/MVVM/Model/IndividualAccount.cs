using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class IndividualAccount : BankAccount
    {
        public IndividualAccount(decimal amount, decimal interestRate, int clientId, int departmentId, int minTerm, DateTime dateTime, int delay)
           : base(amount, interestRate, clientId, departmentId, minTerm, dateTime)
        {
            Delay = delay;
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

    }
}
