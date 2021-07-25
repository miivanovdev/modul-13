using System;
using ModelLib;
using System.ComponentModel;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представления клиента
    /// </summary>
    public class ClientsViewModel : ObservableObject, ITransactable, IDataErrorInfo, IEditableObject
    {
        public ClientsViewModel(Clients client)
        {
            Clients = client;
        }

        /// <summary>
        /// Модель клиента
        /// </summary>
        public Clients Clients { get; private set; }

        /// <summary>
        /// Флаг указывающий на наличие у клиента 
        /// открытых счетов
        /// </summary>
        public bool HaveAnAccounts { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return Clients.FirstName; }
            set
            {
                if (Clients.FirstName == value)
                    return;

                Clients.FirstName = value;
                NotifyPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname
        {
            get { return Clients.Surname; }
            set
            {
                if (Clients.Surname == value)
                    return;

                Clients.Surname = value;
                NotifyPropertyChanged(nameof(Surname));
            }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName
        {
            get { return Clients.SecondName; }
            set
            {
                if (Clients.SecondName == value)
                    return;

                Clients.SecondName = value;
                NotifyPropertyChanged(nameof(SecondName));
            }
        }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int ClientId
        {
            get { return Clients.Id; }
        }

        /// <summary>
        /// Сумма в кармане у клиента
        /// </summary>
        public decimal Amount
        {
            get { return Clients.Amount; }
            set
            {
                if (Clients.Amount == value)
                    return;

                Clients.Amount = Math.Abs(value);
                NotifyPropertyChanged(nameof(Amount));
            }
        }

        /// <summary>
        /// Доступная клиенту сумма
        /// </summary>
        public decimal AmountAvailable { get { return Clients.Amount; } }
        
        /// <summary>
        /// Клиент заблокирован
        /// </summary>
        public bool BadHistory
        {
            get { return Clients.BadHistory; }
            set
            {
                if (Clients.BadHistory == value)
                    return;

                Clients.BadHistory = value;
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
        public void Withdraw(decimal amount)
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

        /// <summary>
        /// Оригинал клиента использующий для отката 
        /// "грязной записи" данных при редактировании
        /// </summary>
        private Clients OrigClient { get; set; }

        /// <summary>
        /// Флаг указывающий на наличие изменений 
        /// в данных клиента
        /// </summary>
        public bool HasChanges
        {
            get
            {
                if (OrigClient == null)
                    return false;

                if (FirstName   != OrigClient.FirstName     ||
                    SecondName  != OrigClient.SecondName    ||
                    Surname     != OrigClient.Surname       ||
                    BadHistory  != OrigClient.BadHistory    ||
                    Amount      != OrigClient.Amount)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Метод подготавливающий режим редактирования
        /// </summary>
        public void BeginEdit()
        {
            if(OrigClient == null)
                OrigClient = new Clients(FirstName, SecondName, Surname, Amount, BadHistory);
        }

        /// <summary>
        /// Метод завершающий режим редактирования
        /// </summary>
        public void EndEdit()
        {
            OrigClient = null;
        }

        /// <summary>
        /// Метод отменяющий режим редактирования
        /// </summary>
        public void CancelEdit()
        {
            FirstName   = OrigClient.FirstName;
            SecondName  = OrigClient.SecondName;
            Surname     = OrigClient.Surname;
            BadHistory  = OrigClient.BadHistory;
            Amount      = OrigClient.Amount;
            OrigClient = null;
        }
    }
}
