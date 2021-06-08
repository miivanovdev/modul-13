using System;
using System.ComponentModel;


namespace ModelLib
{
    public class Client : ObservableObject, ITransactable, IDataErrorInfo
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
        
        public string Name
        {
            get { return $"{Surname} {FirstName} {SecondName}"; }
        }

        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case nameof(FirstName):

                        if (FirstName == string.Empty)
                        {
                            Error = "Введите имя!";
                        }
                        else
                        {
                            if (FirstName.Length > 25)
                                Error = "Слишком длинное имя!";
                        }

                        break;

                    case nameof(SecondName):

                        if (SecondName == string.Empty)
                        {
                            Error = "Введите отчество!";
                        }
                        else
                        {
                            if (SecondName.Length > 25)
                                Error = "Слишком длинное отчество!";
                        }                        

                        break;

                    case nameof(Surname):

                        if (Surname == string.Empty)
                        {
                            Error = "Введите фамилию!";
                        }                          
                        else
                        {
                            if (Surname.Length > 25)
                                Error = "Слишком длинная фамилия!";
                        }                      

                        break;
                }

                return Error;
            }
        }
    }
}
