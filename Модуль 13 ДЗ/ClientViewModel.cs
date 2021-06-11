using System;
using ModelLib;
using System.ComponentModel;

namespace Модуль_13_ДЗ
{
    public class ClientViewModel : ObservableObject, ITransactable, IDataErrorInfo
    {
        public ClientViewModel(Client client)
        {
            Client = client;
        }

        public Client Client { get; private set; }

        public bool HaveAnAccounts { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return Client.FirstName; }
            set
            {
                if (Client.FirstName == value)
                    return;

                Client.FirstName = value;
                NotifyPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname
        {
            get { return Client.Surname; }
            set
            {
                if (Client.Surname == value)
                    return;

                Client.Surname = value;
                NotifyPropertyChanged(nameof(Surname));
            }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName
        {
            get { return Client.SecondName; }
            set
            {
                if (Client.SecondName == value)
                    return;

                Client.SecondName = value;
                NotifyPropertyChanged(nameof(SecondName));
            }
        }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int ClientId
        {
            get { return Client.ClientId; }
        }

        /// <summary>
        /// Сумма в кармане у клиента
        /// </summary>
        public decimal Amount
        {
            get { return Client.Amount; }
            set
            {
                if (Client.Amount == value)
                    return;

                Client.Amount = Math.Abs(value);
                NotifyPropertyChanged(nameof(Amount));
            }
        }

        /// <summary>
        /// Доступная клиенту сумма
        /// </summary>
        public decimal AmountAvailable { get { return Client.Amount; } }
        
        /// <summary>
        /// Клиент заблокирован
        /// </summary>
        public bool BadHistory
        {
            get { return Client.BadHistory; }
            set
            {
                if (Client.BadHistory == value)
                    return;

                Client.BadHistory = value;
                NotifyPropertyChanged(nameof(BadHistory));
            }
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

        /// <summary>
        /// ФИО
        /// </summary>
        public string Name
        {
            get { return $"{Surname} {FirstName} {SecondName}"; }
        }

        /// <summary>
        /// Сообщение об ошибке ввода
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Индексатор для проверки ввода
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
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

                    case nameof(Amount):

                        if (Amount < 0 && Amount > decimal.MaxValue)
                        {
                            Error = "Некорретная сумма";
                        }

                        break;
                }

                return Error;
            }
        }
    }
}
