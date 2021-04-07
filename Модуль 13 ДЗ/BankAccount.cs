using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ
{
    class BankAccount : INotifyPropertyChanged
    {        
        public bool IsCapitalized { get; protected set; }
        public int ClientId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DepartmentId { get; set; }

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

        public bool IsEditable
        {
            get
            {
                return CreatedDate == CurrentDate;
            }
        }

        public decimal InterestRate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public uint MonthPassed
        {
            get
            {
                return Convert.ToUInt32(CurrentDate.Subtract(CreatedDate).Days / (365.2425 / 12));
            }           
        }
        
        public decimal Income
        {
            get
            {
                return Math.Round(CountIncome(), 2);
            }
        }

        protected decimal CountIncome()
        {
            if(MonthPassed > 0)
            {
                if (Amount < 0)
                    return Amount + ((Amount * InterestRate) / 100) / MonthPassed;

                if (IsCapitalized)
                    return Capitalized(Amount, MonthPassed);
            }            

            return Amount + MonthPassed * (Amount * InterestRate) / 100;
        }

        protected decimal Capitalized(decimal amount, uint month)
        {
            if (month == 0)
                return amount;

            return Capitalized(amount + (amount * InterestRate) / 100, --month);
        }

        public BankAccount(decimal amount, decimal interestRate, int clientId, int departmentId, DateTime dateTime, bool isCapitalized = false)
        {
            Amount = amount;
            InterestRate = interestRate;
            ClientId = clientId;
            DepartmentId = departmentId;
            IsCapitalized = isCapitalized;
            CreatedDate = dateTime;
            CurrentDate = dateTime;
        }

        // <summary>
        /// Метод запуска события изменения свойства
        /// </summary>
        /// <param name="propertyName">Изменеяемое свойство</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
