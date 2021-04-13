using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class BankAccount : ObservableObject
    {        
        public int ClientId { get; set; }
        public DateTime CreatedDate { get; set; }
        protected int MinTerm { get; set; }

        public int DepartmentId { get; set; }

        public virtual bool CanWithdrawed
        {
            get { return false; }
        }

        public virtual bool CanAdded
        {
            get { return false; }
        }
       
        public virtual bool CanClose
        {
            get { return false; }
        }

        private decimal amount;
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (value < 0)
                    value *= (-1);

                amount = value;
                NotifyPropertyChanged(nameof(Amount));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                if (currentDate == value)
                    return;

                if (value < CreatedDate)
                    return;

                currentDate = value;
                NotifyPropertyChanged(nameof(MonthPassed));
                NotifyPropertyChanged(nameof(Income));
            }
        }
                
        public decimal InterestRate { get; set; }

        public int MonthPassed
        {
            get
            {
                return Convert.ToInt32(CurrentDate.Subtract(CreatedDate).Days / (365.2425 / 12));
            }           
        }
        
        public decimal Income
        {
            get
            {
                return Math.Round(CountIncome(), 2);
            }
        }

        protected virtual decimal CountIncome()
        {
            if(MonthPassed > 0)    
                return Amount + MonthPassed * (Amount * InterestRate) / 100;

            if (MonthPassed == 0)
                return Amount + 1 * (Amount * InterestRate) / 100;

            return 0;
        }        

        public BankAccount(decimal amount, decimal interestRate, int clientId, int departmentId, int minTerm, DateTime dateTime)
        {
            Amount = amount;
            InterestRate = interestRate;
            ClientId = clientId;
            DepartmentId = departmentId;
            CreatedDate = dateTime;
            MinTerm = minTerm;
            CurrentDate = DateTime.Now;
        }        
    }
}
