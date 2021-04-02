using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Модуль_13_ДЗ
{
    class PhysicalDepartment : BankDepartment<BankAccount>
    {
        public ObservableCollection<Client> Clients { get; set; }

        public PhysicalDepartment(string name, bool isEmpty = false)
        {
            Name = name;

            if(!isEmpty)
                DepartmentId = NextId();

            Clients = new ObservableCollection<Client>();
        }

        
        public virtual BankAccount CreateAccount(Client client)
        {
            return new BankAccount(0, MinDays, InterestRate, client.ClientId, DepartmentId, DateTime.Now);
        }

        public override void GetAccounts(List<BankAccount> accounts, int clientId = 0)
        {
            if (clientId == 0)
            {
                Accounts = new ObservableCollection<BankAccount>(accounts.Where(x => x.ClientId == ));
            }
            else
            {
                Accounts = new ObservableCollection<T>(accounts.Where(x => x.DepartmentId == this.DepartmentId && x.ClientId == clientId));
            }

            foreach (var a in Accounts)
                a.PropertyChanged += new PropertyChangedEventHandler(AccountChangedHandler);
        }
    }
}
