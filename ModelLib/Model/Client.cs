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
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string SecondName { get; set; }

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

        public decimal Amount { get; set; }

        public decimal AmountAvailable { get { return Amount; } }

        public bool BadHistory { get; set; }

        private static int id;

        private static int NextId()
        {
            id++;
            return id;
        }

        public void Put(decimal amount, ITransactable sender = null)
        {
            Amount += amount;
            NotifyPropertyChanged(nameof(Amount));
        }

        public void Withdraw(decimal amount, bool isTransact = false)
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
        
        public string Name
        {
            get { return $"{Surname} {FirstName} {SecondName}"; }
        }
    }
}
