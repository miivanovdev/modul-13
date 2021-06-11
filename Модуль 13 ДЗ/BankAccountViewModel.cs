using System;
using System.Linq;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class BankAccountViewModel : ObservableObject, ITransactable
    {
        public BankAccountViewModel(BankAccount bankAccount)
        {
            BankAccount = bankAccount;
            CurrentDate = DateTime.Now;
        }

        public BankAccount BankAccount { get; private set; }

        /// <summary>
        /// Идентификатор счета
        /// </summary>
        public int AccountId { get { return BankAccount.AccountId; }  }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int OwnerId { get { return BankAccount.OwnerId; } }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string OwnerName { get { return BankAccount.OwnerName; } }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get { return BankAccount.CreatedDate; } }
                
        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get
            {
                return BankAccount.MinTerm;
            }
            set
            {
                if (BankAccount.MinTerm == value)
                    return;

                BankAccount.MinTerm = value;
                NotifyPropertyChanged(nameof(MinTerm));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get { return BankAccount.DepartmentId; } }

        /// <summary>
        /// Наименование счета
        /// </summary>
        public virtual string Name
        {
            get { return $"Базовый на имя {BankAccount.OwnerName} - Id {BankAccount.OwnerId}"; }
        }

        /// <summary>
        /// Месяцев отстрочки
        /// </summary>
        public int Delay
        {
            get
            {
                return BankAccount.Delay;
            }
            set
            {
                if (BankAccount.Delay == value)
                    return;

                BankAccount.Delay = value;
            }
        }

        /// <summary>
        /// Тип счет
        /// </summary>
        public virtual AccountType AccountType
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
        
        /// <summary>
        /// Начальная сумма счета
        /// </summary>
        public decimal Amount
        {
            get
            {
                return BankAccount.Amount;
            }
            set
            {
                if (BankAccount.Amount == value)
                    return;

                BankAccount.Amount = value;
                NotifyPropertyChanged(nameof(Amount));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Доступно средств
        /// </summary>
        public decimal AmountAvailable { get { return BankAccount.Amount; } }

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
        public decimal InterestRate
        {
            get { return BankAccount.InterestRate; }
            set
            {
                if (BankAccount.InterestRate == value)
                    return;

                BankAccount.InterestRate = value;

                NotifyPropertyChanged(nameof(InterestRate));
                NotifyPropertyChanged(nameof(Income));
            }
        }

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
        public bool BadHistory { get; set; } = false;

        private event Action<object, decimal> amountAdded;
        /// <summary>
        /// Событие внесенния суммы на счет
        /// </summary>
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

        private event Action<object, decimal> amountWithdrawed;
        /// <summary>
        /// Событие снятия суммы со счета
        /// </summary>
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

        private event Action<object, ITransactable, decimal> amountTransact;
        /// <summary>
        /// Событие перевода суммы со счета
        /// </summary>
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
            if (MonthPassed == MinTerm || MonthPassed % MinTerm == 0)
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

            if (reciever != null)
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
    }
}
