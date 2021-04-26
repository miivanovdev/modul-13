using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ModelLib
{
    public class BankDepartment<T> : ObservableObject where T : BankAccount
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

        protected ObservableCollection<BankAccount> accounts;

        /// <summary>
        /// Счета департамента
        /// </summary>
        public ObservableCollection<BankAccount> Accounts
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
        private uint minTerm;
        public uint MinTerm
        {
            get { return minTerm; }
            set
            {
                if (value == 0)
                    value = 1;

                minTerm = value;
            }
        }
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

            Accounts = new ObservableCollection<BankAccount>();
        }
         
        /// <summary>
        /// Получить счета
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="handler"></param>
        /// <param name="clientId"></param>
        public void GetAccounts(List<BankAccount> accounts, NotifyCollectionChangedEventHandler accountsHandler, int clientId = 0)
        {
            Unsubscribe();
           
            Accounts = new ObservableCollection<BankAccount>(accounts.GetAccountsSubset(DepartmentId, clientId));
            
            Accounts.CollectionChanged += accountsHandler;
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
                    newAccount = new BankAccount(MinAmount, InterestRate, client.ClientId, client.Name, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.PhysicalAccount:
                    newAccount = new PhysicalAccount(MinAmount, InterestRate, client.ClientId, client.Name,  DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.IndividualAccount:
                    newAccount = new IndividualAccount(MinAmount, InterestRate, client.ClientId, client.Name,  DepartmentId, (int)MinTerm, DateTime.Now, (int)Delay) as T;
                    break;

                case AccountType.PrivilegedAccount:
                    newAccount = new PrivilegedAccount(MinAmount, InterestRate, client.ClientId, client.Name,  DepartmentId, (int)MinTerm, DateTime.Now) as T;
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

        protected void Unsubscribe()
        {
            foreach (var a in Accounts)
            {
                a.AmountAdded -= LogAdding;
                a.AmountWithdrawed -= LogWithdrawing;
                a.AmountTransact -= LogTransact;
            }
        }

        /// <summary>
        /// Закрыть счет
        /// </summary>
        /// <param name="account"></param>
        public virtual bool CloseAccount(T account, out string message)
        {
            if (!Accounts.Remove(account))
            {
                message = "Не удалось закрыть счет!";
                return false;
            }

            Log.Add(new LogMessage($"Закрыт счет {account.OwnerName} {account.OwnerId} Type:{account.Type}"));
            message = "";
            return true;
        }

        protected virtual void LogAdding(object sender, decimal amount)
        {
            var account = sender as ITransactable;
            Log.Add(new LogMessage($"{account.Name} пополнен на сумму: {amount}"));
        }

        protected virtual void LogWithdrawing(object sender, decimal amount)
        {
            var account = sender as ITransactable;
            Log.Add(new LogMessage($"{account.Name} снята сумма: {amount}"));
        }

        protected virtual void LogTransact(object sender, ITransactable accountSender, decimal amount)
        {
            var accountReciever = sender as ITransactable;
            Log.Add(new LogMessage($"{accountSender.Name} перевод на счет {accountReciever.Name} на сумму: {amount}"));
        }

    }
}
