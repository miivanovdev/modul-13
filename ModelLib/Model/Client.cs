using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModelLib
{
    public class Client : ObservableObject, ITransactable
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        private int clientId;
        public int ClientId
        {
            get { return clientId; }
            set
            {
                if (id < value)
                    id = value;

                clientId = value;
            }
        }

        /// <summary>
        /// Сумма в кармане у клиента
        /// </summary>
        private decimal amount;
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
        public bool BadHistory { get; set; }

        /// <summary>
        /// Последний использованный идентификатор
        /// </summary>
        private static int id;

        /// <summary>
        /// Выдать новый идентификатор
        /// </summary>
        /// <returns></returns>
        private static int NextId()
        {
            id++;
            return id;
        }

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

        static Client()
        {
            id = 0;
        }

        public Client() { }

        public Client(string firsName, string surname, string secondName, decimal amount, bool badHistory = false)
        {
            FirstName = firsName;
            Surname = surname;
            SecondName = secondName;
            Amount = amount;
            BadHistory = badHistory;
            ClientId = NextId();
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
