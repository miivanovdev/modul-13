using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление счета
    /// </summary>
    public class BankDepartmentViewModel : ObservableObject
    {
        public BankDepartmentViewModel(Departments bankDepartment)
        {
            BankDepartment = bankDepartment;
        }

        /// <summary>
        /// Модель счета
        /// </summary>
        private Departments BankDepartment { get; set; }

        /// <summary>
        /// Наименование департамента
        /// </summary>
        public string Name
        {
            get { return BankDepartment.Name; }
            set
            {
                if (BankDepartment.Name == value)
                    return;

                NotifyPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Тип департамента
        /// </summary>
        public AccountType AccountType { get { return BankDepartment.Type; } }

        /// <summary>
        /// Ставка по вкладам
        /// </summary>
        public decimal InterestRate
        {
            get { return BankDepartment.InterestRate; }
            set
            {
                if (BankDepartment.InterestRate == value)
                    return;

                NotifyPropertyChanged(nameof(InterestRate));
            }
        }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get { return BankDepartment.MinTerm; }
            set
            {
                if (BankDepartment.MinTerm == value)
                    return;

                NotifyPropertyChanged(nameof(MinTerm));
            }
        }

        /// <summary>
        /// Задержка месяцев
        /// </summary>
        public int Delay
        {
            get { return BankDepartment.Delay; }
            set
            {
                if (BankDepartment.Delay == value)
                    return;

                NotifyPropertyChanged(nameof(Delay));
            }
        }

        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        public decimal MinAmount
        {
            get { return BankDepartment.Delay; }
            set
            {
                if (BankDepartment.Delay == value)
                    return;

                NotifyPropertyChanged(nameof(Delay));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get { return BankDepartment.Id; } }

    }
}
