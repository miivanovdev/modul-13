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
            set { clientId = value; }
        }

        /// <summary>
        /// Сумма в кармане у клиента
        /// </summary>
        public decimal Amount { get; set; }      

        /// <summary>
        /// Клиент заблокирован
        /// </summary>
        public bool BadHistory { get; set; }

        public Client() { }

        public Client(string firsName, string surname, string secondName, decimal amount, bool badHistory = false)
        {
            FirstName = firsName;
            Surname = surname;
            SecondName = secondName;
            Amount = amount;
            BadHistory = badHistory;
        }     
        
    }
}
