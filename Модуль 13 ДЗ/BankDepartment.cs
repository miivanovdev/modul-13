using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ
{
    class BankDepartment : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected ObservableCollection<BankAccount> accounts;
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

        public decimal InterestRate { get; set; }
        
        public uint MinTerm { get; set; }

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

        public BankDepartment(string name, decimal minAmount, uint minTerm, decimal rate, bool isEmpty = false)
        {
            Name = name;
            MinTerm = minTerm;
            InterestRate = rate;
            MinAmount = minAmount;

            if (!isEmpty)
                DepartmentId = NextId();

            Accounts = new ObservableCollection<BankAccount>();
        }

        /// <summary>
        /// Метод запуска события изменения свойства
        /// </summary>
        /// <param name="propertyName">Изменеяемое свойство</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetAccounts(List<BankAccount> accounts, int clientId = 0)
        {
            if (clientId == 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<BankAccount>(accounts);

            if (clientId == 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<BankAccount>(accounts.Where(x => x.DepartmentId == DepartmentId));

            if(clientId > 0 && DepartmentId == 0)
                Accounts = new ObservableCollection<BankAccount>(accounts.Where(x => x.ClientId == clientId));

            if (clientId > 0 && DepartmentId > 0)
                Accounts = new ObservableCollection<BankAccount>(accounts.Where(x => x.ClientId == clientId && x.DepartmentId == DepartmentId));
        }      
        
        public void OpenAccount(List<BankAccount> accounts, Client client)
        {
            accounts.Add(new BankAccount(MinAmount, InterestRate, client.ClientId, DepartmentId, DateTime.Now));
        }
    }
}
