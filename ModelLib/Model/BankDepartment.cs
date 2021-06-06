using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Data.Linq.Mapping;

namespace ModelLib
{
    [Table(Name = "Departments")]
    [InheritanceMapping(Code = AccountType.Basic, Type = typeof(BankDepartment<BankAccount>),
    IsDefault = true)]
    [InheritanceMapping(Code = AccountType.IndividualAccount, Type = typeof(IndividualDepartment))]
    [InheritanceMapping(Code = AccountType.PhysicalAccount, Type = typeof(PhysicalDepartment))]
    [InheritanceMapping(Code = AccountType.PrivilegedAccount, Type = typeof(PrivilegedDepartment))]
    public class BankDepartment<T> : ObservableObject where T : BankAccount
    {
        [Column(Name = "Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public ObservableCollection<LogMessage> Log { get; set; }

        /// <summary>
        /// Тип департамента
        /// </summary>
        [Column(Name = "AccountType", IsDiscriminator = true)]
        public virtual AccountType Type { get; set; } = AccountType.Basic;

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
        [Column(Name = "InterestRate")]
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Минимальный срок вклада в месяцах
        /// </summary>
        private int minTerm;

        [Column(Name = "MinTerm")]
        public int MinTerm
        {
            get { return minTerm; }
            set
            {
                if (value == 0)
                    value = 1;

                minTerm = value;
            }
        }

        /// <summary>
        /// Отсрочка
        /// </summary>
        [Column(Name = "Delay")]
        public int Delay { get; set; }

        public decimal minAmount;

        /// <summary>
        /// Минимальная сумма вклада
        /// </summary>
        [Column(Name = "MinAmount")]
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
        [Column(Name = "DepartmentId", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DepartmentId { get; set; }

        public BankDepartment() { }

        public BankDepartment(ObservableCollection<LogMessage> log, string name, decimal minAmount, int minTerm, decimal rate, int delay = 0)
        {
            Log = log;
            Name = name;
            MinTerm = minTerm;
            Delay = delay;
            InterestRate = rate;
            MinAmount = minAmount;

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
            if (client.AmountAvailable < MinAmount)
                throw new TransactionFailureException($"У клиента {client.Name} недостаточно средств {client.Amount}, минимальная сумма вклада {MinAmount}");

            T newAccount = null;

            switch (Type)
            {
                case AccountType.Basic:
                    newAccount = new BankAccount(MinAmount, InterestRate, client.ClientId, client.Name, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.PhysicalAccount:
                    newAccount = new PhysicalAccount(MinAmount, InterestRate, client.ClientId, client.Name, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.IndividualAccount:
                    newAccount = new IndividualAccount(MinAmount, InterestRate, client.ClientId, client.Name, DepartmentId, (int)MinTerm, DateTime.Now, (int)Delay) as T;
                    break;

                case AccountType.PrivilegedAccount:
                    newAccount = new PrivilegedAccount(MinAmount, InterestRate, client.ClientId, client.Name, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

            }

            client.Amount -= MinAmount;
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

        public void Unsubscribe()
        {
            if (Accounts == null)
                return;

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

        protected virtual void LogTransact(object sender, ITransactable accountReciever, decimal amount)
        {
            var accountSender = sender as ITransactable;
            Log.Add(new LogMessage($"{accountSender.Name} перевод на счет {accountReciever.Name} на сумму: {amount}"));
        }

    }
}
