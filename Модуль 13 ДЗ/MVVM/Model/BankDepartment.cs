using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Модуль_13_ДЗ.MVVM.Model
{
    internal class BankDepartment<T> : ObservableObject where T : BankAccount
    {
        public string Name { get; set; }

        public virtual AccountType AccountType
        {
            get { return AccountType.Basic; }
        }

        protected ObservableCollection<T> accounts;
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

        public decimal InterestRate { get; set; }
        
        public uint MinTerm { get; set; }
        public uint Delay { get; set; }

        public decimal minAmount;
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

        protected static int NextId()
        {
            id++;
            return id;
        }

        static BankDepartment()
        {
            id = 0;
        }

        public BankDepartment(string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false, uint delay = 0)
        {
            Name = name;
            MinTerm = minTerm;
            Delay = delay;
            InterestRate = rate;
            MinAmount = minAmount;

            if (!isEmpty)
                DepartmentId = NextId();

            Accounts = new ObservableCollection<T>();
        }
                
        public void GetAccounts(List<T> accounts, int clientId = 0)
        {
            if (clientId == 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<T>(accounts);

            if (clientId == 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.DepartmentId == DepartmentId));

            if(clientId > 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.ClientId == clientId));

            if (clientId > 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.ClientId == clientId && x.DepartmentId == DepartmentId));
        }   
        
        public void AddClient(IList<Client> clients)
        {
            clients.Add(new Client("Новый клиент", "", "", 0));
        }        
        

        public virtual void CloseAccount(IList<T> accounts, Client client)
        {
            throw new NotImplementedException();
        }

        public virtual void Transact(IList<T> accounts, Client client)
        {
            throw new NotImplementedException();
        }

        public virtual void OpenAccount(IList<T> accounts, Client client)
        {
            T newAccount = null;

            switch(AccountType)
            {
                case AccountType.Basic:
                    newAccount = new BankAccount(MinAmount, InterestRate, client.ClientId, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.PhysicalAccount:
                    newAccount = new PhysicalAccount(MinAmount, InterestRate, client.ClientId, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

                case AccountType.IndividualAccount:
                    newAccount = new IndividualAccount(MinAmount, InterestRate, client.ClientId, DepartmentId, (int)MinTerm, DateTime.Now, (int)Delay) as T;
                    break;

                case AccountType.PrivilegedAccount:
                    newAccount = new PrivilegedAccount(MinAmount, InterestRate, client.ClientId, DepartmentId, (int)MinTerm, DateTime.Now) as T;
                    break;

            }

            accounts.Add(newAccount);
        }

        public void Put(IList<T> accounts, Client client)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(IList<T> accounts, Client client)
        {
            throw new NotImplementedException();
        }        
    }
}
