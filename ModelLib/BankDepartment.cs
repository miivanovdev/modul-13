using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Linq;

namespace ModelLib
{
    public class BankDepartment
    {
        /// <summary>
        /// Наименование департамента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип департамента
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Ставка по вкладам
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public int MinTerm { get; set; }

        /// <summary>
        /// Задержка месяцев
        /// </summary>
        public int Delay { get; set; }

        private decimal minAmount;
        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        public decimal MinAmount
        {
            get { return minAmount; }
            set { minAmount = Math.Abs(value); }
        }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId { get; set; }
        
        public BankDepartment() { }

        public BankDepartment(string name, decimal minAmount, int minTerm, decimal rate, bool isEmpty = false, int delay = 0)
        {
            Name = name;
            MinTerm = minTerm;
            Delay = delay;
            InterestRate = rate;
            MinAmount = minAmount;
        }       
    }
}
