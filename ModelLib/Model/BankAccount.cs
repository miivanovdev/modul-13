using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;

namespace ModelLib
{
    [Table(Name = "Accounts")]
    [InheritanceMapping(Code = AccountType.Basic, Type = typeof(BankAccount),
    IsDefault = true)]
    [InheritanceMapping(Code = AccountType.IndividualAccount, Type = typeof(IndividualAccount))]
    [InheritanceMapping(Code = AccountType.PhysicalAccount, Type = typeof(PhysicalAccount))]
    [InheritanceMapping(Code = AccountType.PrivilegedAccount, Type = typeof(PrivilegedAccount))]
    public class BankAccount : ObservableObject, ITransactable
    {
        [Column(Name = "AccountId", IsDbGenerated = true, IsPrimaryKey = true)]
        public int AccountId { get; set; }
        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        [Column(Name = "OwnerId")]
        public int OwnerId { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        [Column(Name = "OwnerName")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [Column(Name = "CreationDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        [Column(Name = "MinTerm")]
        public int MinTerm { get; set; }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        [Column(Name = "DepartmentId")]
        public int DepartmentId { get; set; }

        [Column(Name = "Delay")]
        public int Delay { get; set; }

        /// <summary>
        /// Наименование счета
        /// </summary>
        public virtual string Name
        {
            get { return $"Базовый на имя {OwnerName} - Id {OwnerId}"; }
        }

        /// <summary>
        /// Тип счет
        /// </summary>
        [Column(Name = "AccountType", IsDiscriminator = true)]
        public virtual AccountType Type { get; set; } 

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
        [Column(Name = "Amount")]
        public decimal Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (value < 0)
                    value = Math.Abs(value);

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
        [Column(Name = "CurrentDate")]
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
        [Column(Name = "InterestRate")]
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
        /// Счет заблокирован
        /// </summary>
        [Column(Name = "BadHistory")]
        public bool BadHistory { get; set; }

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
        private event Action<object, ITransactable, decimal> amountTransact;
        public event Action<object, ITransactable, decimal> AmountTransact
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
        public void Withdraw(decimal amount, ITransactable reciever = null)
        {
            Amount -= amount;

            if(reciever != null)
            {
                amountTransact?.Invoke(this, reciever, amount);
            }
            else
            {
                amountWithdrawed?.Invoke(this, amount);
            }                

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

        public BankAccount() { }
    }
}
