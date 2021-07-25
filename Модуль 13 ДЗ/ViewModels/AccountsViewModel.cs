using System;
using System.Linq;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление счета
    /// </summary>
    public class AccountsViewModel : ObservableObject, ITransactable
    {
        public AccountsViewModel(Accounts bankAccount)
        {
            Accounts = bankAccount;
            CurrentDate = DateTime.Now;
        }

        /// <summary>
        /// Модель счета
        /// </summary>
        public Accounts Accounts { get; private set; }

        /// <summary>
        /// Идентификатор счета
        /// </summary>
        public int AccountId { get { return Accounts.Id; }  }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int OwnerId { get { return Accounts.ClientsRefId; } }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string OwnerName { get { return Accounts.ClientsName; } }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get { return Accounts.CreatedDate; } }
                
        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get
            {
                return Accounts.MinTerm;
            }
            set
            {
                if (Accounts.MinTerm == value)
                    return;

                Accounts.MinTerm = value;
                NotifyPropertyChanged(nameof(MinTerm));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int? DepartmentId { get { return Accounts.DepartmentsRefId; } }

        /// <summary>
        /// Идентификатор типа счета
        /// </summary>
        public int AccountTypesId
        {
            get { return Accounts.AccountTypesId; }
            set
            {
                if (value == Accounts.AccountTypesId)
                    return;

                Accounts.AccountTypesId = value;

                NotifyPropertyChanged(nameof(AccountTypesId));
            }
        }

        /// <summary>
        /// Наименование счета
        /// </summary>
        public virtual string Name
        {
            get { return $"{Accounts.AccountTypes.Name} на имя {Accounts.ClientsName} - Id {Accounts.ClientsRefId}"; }
        }

        /// <summary>
        /// Доступно снятие со счета
        /// </summary>
        public virtual bool CanWithdrawed
        {
            get
            {
                if(Accounts.AccountTypes.CanWithdrawed)
                {
                    if(Accounts.AccountTypes.WithdrawingDependsOnMinTerm)
                    {
                        return CreatedDate.AddMonths(MinTerm) <= CurrentDate;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Доступно пополнение счета
        /// </summary>
        public virtual bool CanAdded
        {
            get
            {
                if (Accounts.AccountTypes.CanAdded)
                {
                    if (Accounts.AccountTypes.AddingDependsOnMinTerm)
                    {
                        return CreatedDate.AddMonths(MinTerm) <= CurrentDate;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Доступно закрытие счета
        /// </summary>
        public virtual bool CanClose
        {
            get
            {
                if (Accounts.AccountTypes.CanClose)
                {
                    if (Accounts.AccountTypes.ClosingDependsOnMinTerm)
                    {
                        return CreatedDate.AddMonths(MinTerm) <= CurrentDate;
                    }

                    return true;
                }

                return false;
            }
        }
                        
        /// <summary>
        /// Начальная сумма счета
        /// </summary>
        public decimal Amount
        {
            get
            {
                return Accounts.Amount;
            }
            set
            {
                if (Accounts.Amount == value)
                    return;

                Accounts.Amount = value;
                NotifyPropertyChanged(nameof(Amount));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Доступно средств
        /// </summary>
        public decimal AmountAvailable { get { return Accounts.Amount; } }

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
            get { return Accounts.InterestRate; }
            set
            {
                if (Accounts.InterestRate == value)
                    return;

                Accounts.InterestRate = value;

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

        /// <summary>
        /// Подсчет дохода
        /// </summary>
        /// <returns></returns>
        protected virtual decimal CountIncome()
        {
            if(Accounts.AccountTypes.IsCapitalized && MonthPassed >= 0)
                return Capitalized(Amount, MonthPassed);

            if (MonthPassed == MinTerm || MonthPassed % MinTerm == 0)
                return Amount + (MonthPassed / MinTerm) * (Amount * InterestRate) / 100;

            return 0;
        }

        /// <summary>
        /// Метод подсчета капитализации
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private decimal Capitalized(decimal amount, int month)
        {
            if (month == 0)
                return amount;

            return Capitalized(amount + (amount * InterestRate) / 100, --month);
        }

        /// <summary>
        /// Положить на счет
        /// </summary>
        /// <param name="amount"></param>
        public void Put(decimal amount)
        {
            Amount += amount;

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

            NotifyPropertyChanged(nameof(Amount));
            NotifyPropertyChanged(nameof(Income));
        }
    }
}
