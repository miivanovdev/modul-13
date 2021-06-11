using System;

namespace ModelLib
{
    public class BankAccount
    {
        /// <summary>
        /// Идентификатор счета
        /// </summary>
        public int AccountId { get; set; }

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
        /// Месяцев отстрочки
        /// </summary>
        private int delay;
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                if (value < 0)
                    value = Math.Abs(value);

                delay = value;
            }
        }

        /// <summary>
        /// Тип счет
        /// </summary>
        public AccountType AccountType { get; set; }
       
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
                    value = Math.Abs(value);

                amount = value;                
            }
        }

        /// <summary>
        /// процент по вкладу
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Счет заблокирован
        /// </summary>
        public bool BadHistory { get; set; } = false;

        public BankAccount() { }

        public BankAccount(int ownerId, string ownerName, decimal amount, decimal interestRate, int departmentId, int minTerm, int delay, AccountType accountType)
        {
            Amount = amount;
            InterestRate = interestRate;
            OwnerId = ownerId;
            OwnerName = ownerName;
            DepartmentId = departmentId;
            MinTerm = minTerm;
            AccountType = accountType;
            CreatedDate = DateTime.Now;
        }        
    }
}
