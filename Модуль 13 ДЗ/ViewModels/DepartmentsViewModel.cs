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
    public class DepartmentsViewModel : ObservableObject
    {
        public DepartmentsViewModel(Departments bankDepartment)
        {
            departments = bankDepartment;
        }

        /// <summary>
        /// Модель счета
        /// </summary>
        private readonly Departments departments;


        public Departments Departments { get { return departments; } }

        /// <summary>
        /// Наименование департамента
        /// </summary>
        public string Name
        {
            get { return departments.Name; }
            set
            {
                if (departments.Name == value)
                    return;

                NotifyPropertyChanged(nameof(Name));
            }
        }       

        /// <summary>
        /// Ставка по вкладам
        /// </summary>
        public decimal InterestRate
        {
            get { return departments.InterestRate; }
            set
            {
                if (departments.InterestRate == value)
                    return;

                NotifyPropertyChanged(nameof(InterestRate));
            }
        }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get { return departments.MinTerm; }
            set
            {
                if (departments.MinTerm == value)
                    return;

                NotifyPropertyChanged(nameof(MinTerm));
            }
        }
                
        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        public decimal MinAmount
        {
            get { return departments.MinAmount; }
            set
            {
                if (departments.MinAmount == value)
                    return;

                NotifyPropertyChanged(nameof(MinAmount));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get { return departments.Id; } }

        public int DefaultAccountType { get { return departments.AccountTypes.First().Id; } }

    }
}
