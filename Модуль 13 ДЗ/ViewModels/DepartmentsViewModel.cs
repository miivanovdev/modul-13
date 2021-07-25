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
            Departments = bankDepartment;
        }

        /// <summary>
        /// Модель счета
        /// </summary>
        public Departments Departments { get; set; }

        /// <summary>
        /// Наименование департамента
        /// </summary>
        public string Name
        {
            get { return Departments.Name; }
            set
            {
                if (Departments.Name == value)
                    return;

                NotifyPropertyChanged(nameof(Name));
            }
        }       

        /// <summary>
        /// Ставка по вкладам
        /// </summary>
        public decimal InterestRate
        {
            get { return Departments.InterestRate; }
            set
            {
                if (Departments.InterestRate == value)
                    return;

                NotifyPropertyChanged(nameof(InterestRate));
            }
        }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm
        {
            get { return Departments.MinTerm; }
            set
            {
                if (Departments.MinTerm == value)
                    return;

                NotifyPropertyChanged(nameof(MinTerm));
            }
        }
                
        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        public decimal MinAmount
        {
            get { return Departments.MinAmount; }
            set
            {
                if (Departments.MinAmount == value)
                    return;

                NotifyPropertyChanged(nameof(MinAmount));
            }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get { return Departments.Id; } }

    }
}
