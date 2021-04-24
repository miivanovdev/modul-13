using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Модуль_13_ДЗ.MVVM.Model
{
    public class BankAccount : ObservableObject, ITransactable
    {
        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm { get; set; }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Наименование счета
        /// </summary>
        public virtual string AccountName
        {
            get { return $"Базовый на имя {OwnerName}"; }
        }

        /// <summary>
        /// Тип счет
        /// </summary>
        public virtual AccountType Type
        {
            get { return AccountType.Basic; }
        }

        /// <summary>
        /// Доступно снятие со счета
        /// </summary>
        public virtual bool CanWithdrawed
        {
            get { return false; }
        }

        /// <summary>
        /// Доступно пополнение счета
        /// </summary>
        public virtual bool CanAdded
        {
            get { return false; }
        }

        /// <summary>
        /// Доступно закрытие счета
        /// </summary>
        public virtual bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// Доступен перевод со счета
        /// </summary>
        public virtual bool CanTransact
        {
            get { return false; }
        }

        private decimal amount;

        /// <summary>
        /// Начальная сумма счета
        /// </summary>
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

        /// <summary>
        /// Доступно средств
        /// </summary>
        public decimal AmountAvailable { get { return Amount; } }

        private DateTime currentDate;

        /// <summary>
        /// Текущая дата
        /// </summary>
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

        /// <summary>
        /// процент по вкладу
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Прошло месяцев
        /// </summary>
        public int MonthPassed
        {
            get
            {
                return Convert.ToInt32(CurrentDate.Subtract(CreatedDate).Days / (365.2425 / 12));
            }
        }

        /// <summary>
        /// Доход по вкладу
        /// </summary>
        public decimal Income
        {
            get
            {
                return Math.Round(CountIncome(), 2);
            }
        }

        /// <summary>
        /// Событие внесенния суммы на счет
        /// </summary>
        private event Action<object, decimal> amountAdded;
        public event Action<object, decimal> AmountAdded
        {
            add
            {
                if (amountAdded != null && amountAdded.GetInvocationList().Contains(value))
                    return;

                amountAdded += value;
            }
            remove { amountAdded -= value; }
        }

        /// <summary>
        /// Событие снятия суммы со счета
        /// </summary>
        private event Action<object, decimal> amountWithdrawed;
        public event Action<object, decimal> AmountWithdrawed
        {
            add
            {
                if (amountWithdrawed != null && amountWithdrawed.GetInvocationList().Contains(value))
                    return;

                amountWithdrawed += value;
            }
            remove { amountWithdrawed -= value; }
        }

        /// <summary>
        /// Событие перевода суммы со счета
        /// </summary>
        private event Action<object, BankAccount, decimal> amountTransact;
        public event Action<object, BankAccount, decimal> AmountTransact
        {
            add
            {
                if (amountTransact != null && amountTransact.GetInvocationList().Contains(value))
                    return;

                amountTransact += value;
            }
            remove { amountTransact -= value; }
        }
                                             
        /// <summary>
        /// Подсчет дохода
        /// </summary>
        /// <returns></returns>
        protected virtual decimal CountIncome()
        {
            if(MonthPassed == MinTerm || MonthPassed % MinTerm == 0)    
                return Amount + (MonthPassed / MinTerm) * (Amount * InterestRate) / 100;
            
            return 0;
        }

        /// <summary>
        /// Положить на счет
        /// </summary>
        /// <param name="amount"></param>
        public void Put(decimal amount)
        {
            Amount += amount;
            amountAdded?.Invoke(this, amount);
            NotifyPropertyChanged(nameof(Amount));
            NotifyPropertyChanged(nameof(Income));
        }

        /// <summary>
        /// Списать со счета
        /// </summary>
        /// <param name="amount"></param>
        public void Withdraw(decimal amount)
        {
            Amount -= amount;
            amountWithdrawed?.Invoke(this, amount);
            NotifyPropertyChanged(nameof(Amount));
            NotifyPropertyChanged(nameof(Income));
        }

        public BankAccount(decimal amount, decimal interestRate, int ownerId, string ownerName , int departmentId, int minTerm, DateTime dateTime)
        {
            Amount = amount;
            InterestRate = interestRate;
            OwnerId = ownerId;
            OwnerName = ownerName;
            DepartmentId = departmentId;
            CreatedDate = dateTime;
            MinTerm = minTerm;
            CurrentDate = DateTime.Now;
        }        
    }
}
