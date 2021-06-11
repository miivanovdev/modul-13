using System;

namespace ModelLib
{
    public class Client 
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; set; } = string.Empty;

        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName { get; set; } = string.Empty;

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
        public decimal Amount { get; set; }      

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
        
    }
}
