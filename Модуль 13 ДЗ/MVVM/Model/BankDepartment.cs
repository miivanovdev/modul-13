using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Модуль_13_ДЗ.MVVM.Model
{
    internal class BankDepartment<T> : ObservableObject where T : BankAccount
    {
        public string Name { get; set; }
        public ObservableCollection<LogMessage> Log { get; set; }

        /// <summary>
        /// Тип департамента
        /// </summary>
        public virtual AccountType AccountType
        {
            get { return AccountType.Basic; }
        }

        protected ObservableCollection<T> accounts;

        /// <summary>
        /// Счета департамента
        /// </summary>
        public ObservableCollection<T> Accounts
        {
            get
            {
                return accounts;
            }
            set
            {
                if (accounts == value)
                    return;

                accounts = value;
                NotifyPropertyChanged(nameof(Accounts));
            }
        }

        /// <summary>
        /// Ставка по вкладам
        /// </summary>
        public decimal InterestRate { get; set; }
        
        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        public uint MinTerm { get; set; }
        public uint Delay { get; set; }

        public decimal minAmount;

        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        public decimal MinAmount
        {
            get
            {
                return minAmount;
            }
            set
            {
                if (value < 0)
                    value *= (-1);

                minAmount = value;
            }

        }

        protected int departmentId;

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int DepartmentId
        {
            get { return departmentId; }
            set
            {
                if (id < value)
                    id = value;

                departmentId = value;
            }
        }

        protected static int id;

        /// <summary>
        /// Получить следующий идентификатор
        /// </summary>
        /// <returns></returns>
        protected static int NextId()
        {
            id++;
            return id;
        }

        static BankDepartment()
        {
            id = 0;
        }

        public BankDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false, uint delay = 0)
        {
            Log = log;
            Name = name;
            MinTerm = minTerm;
            Delay = delay;
            InterestRate = rate;
            MinAmount = minAmount;

            if (!isEmpty)
                DepartmentId = NextId();

            Accounts = new ObservableCollection<T>();
        }
         
        /// <summary>
        /// Получить счета
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="handler"></param>
        /// <param name="clientId"></param>
        public void GetAccounts(List<T> accounts, NotifyCollectionChangedEventHandler accountsHandler, NotifyCollectionChangedEventHandler logHandler, int clientId = 0)
        {
            if (clientId == 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<T>(accounts);

            if (clientId == 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.DepartmentId == DepartmentId));

            if(clientId > 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.OwnerId == clientId));

            if (clientId > 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.OwnerId == clientId && x.DepartmentId == DepartmentId));

            Accounts.CollectionChanged += accountsHandler;
            Log.CollectionChanged += logHandler;
            Subscribe();
            
        }   
        
        /// <summary>
        /// Добавить клиента
        /// </summary>
        /// <param name="clients"></param>
        public void AddClient(IList<Client> clients)
        {
            clients.Add(new Client("Новый клиент", "", "", 0));
        }        

        /// <summary>
        /// Открыть счет
        /// </summary>
        /// <param name="client"></param>
        public virtual void OpenAccount(Client client)
        {
            T newAccount = null;

            switch(AccountType)
            {
                case AccountType.Basic:
                    newAccount = new BankAccount(MinAmount, InterestRate, client.ClientId, client.FIO, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.PhysicalAccount:
                    newAccount = new PhysicalAccount(MinAmount, InterestRate, client.ClientId, client.FIO,  DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.IndividualAccount:
                    newAccount = new IndividualAccount(MinAmount, InterestRate, client.ClientId, client.FIO,  DepartmentId, (int)MinTerm, DateTime.Now, (int)Delay) as T;
                    break;

                case AccountType.PrivilegedAccount:
                    newAccount = new PrivilegedAccount(MinAmount, InterestRate, client.ClientId, client.FIO,  DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

            }

            Accounts.Add(newAccount);
            Log.Add(new LogMessage($"Открыт счет {newAccount.OwnerName} {newAccount.OwnerId} Type:{newAccount.Type}"));
        }

        protected void Subscribe()
        {
            foreach(var a in Accounts)
            {
                a.AmountAdded += LogAdding;
                a.AmountWithdrawed += LogWithdrawing;
                a.AmountTransact += LogTransact;
            }
        }

        /// <summary>
        /// Закрыть счет
        /// </summary>
        /// <param name="account"></param>
        public virtual void CloseAccount(T account)
        {
            if (!Accounts.Remove(account))
                MessageBox.Show("Не удалось закрыть счет!");

            Log.Add(new LogMessage($"Закрыт счет {account.OwnerName} {account.OwnerId} Type:{account.Type}"));
        }

        protected virtual void LogAdding(object sender, decimal amount)
        {
            var account = sender as BankAccount;
            Log.Add(new LogMessage($"{account.OwnerName} Id:{account.OwnerId} Type: {account.Type} пополнен на сумму: {amount}"));
        }

        protected virtual void LogWithdrawing(object sender, decimal amount)
        {
            var account = sender as BankAccount;
            Log.Add(new LogMessage($"{account.OwnerName} Id:{account.OwnerId} Type: {account.Type} снята сумма: {amount}"));
        }

        protected virtual void LogTransact(object sender, BankAccount accountReciever, decimal amount)
        {
            var account = sender as BankAccount;
            Log.Add(new LogMessage($"{account.OwnerName} Id:{account.OwnerId} Type: {account.Type} перевод на счет {accountReciever.OwnerName} Id:{accountReciever.OwnerId} Type: {accountReciever.Type} на сумму: {amount}"));
        }

    }
}
