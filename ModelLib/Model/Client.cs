using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.Linq.Mapping;

namespace ModelLib
{
    [Table(Name = "Clients")]
    public class Client : ObservableObject, ITransactable
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Column(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Column(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Column(Name = "SecondName")]
        public string SecondName { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>

        [Column(Name = "ClientId", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ClientId { get; set; }       
        

        /// <summary>
        /// Сумма в кармане у клиента
        /// </summary>
        private decimal amount;

        [Column(Name = "Amount")]
        public decimal Amount
        {
            get { return amount; }
            set
            {
                amount = Math.Abs(value);
                NotifyPropertyChanged(nameof(Amount));

            }
        }

        /// <summary>
        /// Доступная клиенту сумма
        /// </summary>
        public decimal AmountAvailable { get { return Amount; } }

        /// <summary>
        /// Клиент заблокирован
        /// </summary>
        [Column(Name = "BadHistory")]
        public bool BadHistory { get; set; }
        
        /// <summary>
        /// Положить сумму в карман
        /// </summary>
        /// <param name="amount"></param>
        public void Put(decimal amount)
        {
            Amount += amount;
            NotifyPropertyChanged(nameof(Amount));
        }

        /// <summary>
        /// Достать сумму из кармана
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="reciever"></param>
        public void Withdraw(decimal amount, ITransactable reciever)
        {
            Amount -= amount;
            NotifyPropertyChanged(nameof(Amount));
        }
        
        public Client() { }

        public Client(string firsName, string surname, string secondName, decimal amount, bool badHistory = false)
        {
            FirstName = firsName;
            Surname = surname;
            SecondName = secondName;
            Amount = amount;
            BadHistory = badHistory;
        }

        public Client(string firsName, string surname, string secondName, decimal amount, int clientId, bool badHistory)
        {
            FirstName = firsName;
            Surname = surname;
            SecondName = secondName;
            Amount = amount;
            ClientId = clientId;
            BadHistory = badHistory;
        }

        public string Name
        {
            get { return $"{Surname} {FirstName} {SecondName}"; }
        }
    }
}
