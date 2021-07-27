using System;
using System.Linq;
using ModelLib;
using Модуль_13_ДЗ.Mediators;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление счета
    /// </summary>
    public class AccountsViewModel : ObservableObject, ITransactable
    {
        public AccountsViewModel(Accounts bankAccount)
        {
            accounts = bankAccount;
            CurrentDate = DateTime.Now;
        }

        /// <summary>
        /// Модель счета
        /// </summary>
        private readonly Accounts accounts;

        public Accounts Accounts { get { return accounts; } }

        /// <summary>
        /// Идентификатор счета
        /// </summary>
        public int AccountId { get { return accounts.Id; }  }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int OwnerId { get { return accounts.ClientsRefId; } }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string OwnerName { get { return accounts.ClientsName; } }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get { return accounts.CreatedDate; } }
                
        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get
            {
                return accounts.MinTerm;
            }
            set
            {
                if (accounts.MinTerm == value)
                    return;

                accounts.MinTerm = value;
                NotifyPropertyChanged(nameof(MinTerm));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int? DepartmentId { get { return accounts.DepartmentsRefId; } }

        /// <summary>
        /// Идентификатор типа счета
        /// </summary>
        public int AccountTypesId
        {
            get { return accounts.AccountTypesId; }
            set
            {
                if (value == accounts.AccountTypesId)
                    return;

                accounts.AccountTypesId = value;

                NotifyPropertyChanged(nameof(AccountTypesId));
            }
        }

        /// <summary>
        /// Наименование счета
        /// </summary>
        public virtual string Name
        {
            get { return $"{accounts.AccountTypes.Name} на имя {accounts.ClientsName} - Id {accounts.ClientsRefId}"; }
        }

        /// <summary>
        /// Доступно снятие со счета
        /// </summary>
        public virtual bool CanWithdrawed
        {
            get
            {
                if(accounts.AccountTypes.CanWithdrawed)
                {
                    if(accounts.AccountTypes.WithdrawingDependsOnMinTerm)
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
                if (accounts.AccountTypes.CanAdded)
                {
                    if (accounts.AccountTypes.AddingDependsOnMinTerm)
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
                if (accounts.AccountTypes.CanClose)
                {
                    if (accounts.AccountTypes.ClosingDependsOnMinTerm)
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
                return accounts.Amount;
            }
            set
            {
                if (accounts.Amount == value)
                    return;

                accounts.Amount = value;
                NotifyPropertyChanged(nameof(Amount));
                NotifyPropertyChanged(nameof(Income));
            }
        }

        /// <summary>
        /// Доступно средств
        /// </summary>
        public decimal AmountAvailable { get { return accounts.Amount; } }

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
            get { return accounts.InterestRate; }
            set
            {
                if (accounts.InterestRate == value)
                    return;

                accounts.InterestRate = value;

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
            if(accounts.AccountTypes.IsCapitalized && MonthPassed >= 0)
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
