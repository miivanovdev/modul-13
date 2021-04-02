using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Модуль_13_ДЗ
{
    class Client : INotifyPropertyChanged
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

        private decimal income;
        public decimal Income
        {
            get
            {
                return income;
            }
            set
            {
                if (income == value)
                    return;

                income = value;                
            }
        }

        private decimal costs;
        public decimal Costs
        {
            get
            {
                return costs;
            }
            set
            {
                if (costs == value)
                    return;

                costs = value;                
            }
        }        

        private static int id;

        public event PropertyChangedEventHandler PropertyChanged;

        private static int NextId()
        {
            id++;
            return id;
        }

        static Client()
        {
            id = 0;
        }

        public Client() { }

        public Client(string firsName, string surname, string secondName, decimal income, decimal costs)
        {
            FirstName = firsName;
            Surname = surname;
            SecondName = secondName;
            Income = income;
            Costs = costs;
            ClientId = NextId();
        }

        /// <summary>
        /// Метод запуска события изменения свойства
        /// </summary>
        /// <param name="propertyName">Изменеяемое свойство</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        
    }
}
