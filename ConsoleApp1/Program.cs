using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        class Department<T> where T : Account
        {
            public virtual void Show()
            {
                Console.WriteLine("Parent");
            }

            public T Account { get; set; }
        }

        class DepositDepartment<DepositeAccount> : Department<Account>
        {
            public override void Show()
            {
                Console.WriteLine("Child");
            }
        }

        class Bank : IBank<Account>
        {
            public virtual Account CreateAccount(decimal sum)
            {
                return new Account(sum);
            }

            public virtual void Show()
            {
                Console.WriteLine("Банк!");
            }
        }

        interface IBank<T> where T : Account
        {
            T CreateAccount(decimal sum);
        }

        class DepositeBank : Bank
        {
            public override void Show()
            {
                Console.WriteLine("Депозитный банк");
            }

            public override Account CreateAccount(decimal sum)
            {
                return new DepositeAccount(sum);
            }
        }

        class Account : IAccount
        {
            public Account(decimal amount)
            {
                Amount = amount;
            }

            public decimal Amount { get; set; }

            protected const decimal Percent = 10M;

            public virtual void CountIncome()
            {
                Amount += (Amount * Percent / 100);
            }

            public virtual void Show()
            {
                Console.WriteLine($"Базовый счет - доход {Amount}!");
            }
        }

        interface IAccount
        {
            void CountIncome();
        }

        class DepositeAccount : Account
        {
            public DepositeAccount(decimal amount)
                : base(amount)
            {
                Amount = amount;
            }

            protected new const decimal Percent = 15M;

            public override void CountIncome()
            {
                Amount += (Amount * Percent / 100);
            }

            public override void Show()
            {
                Console.WriteLine($"Депозитный счет - доход {Amount}!");
            }
        }
        

        class Client
        {
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

            public string Name { get; set; }
            private static int id;
                        
            private static int NextId()
            {
                id++;
                return id;
            }

            static Client()
            {
                id = 0;
            }

            public Client() { }

            public Client(string name)
            {
                Name = name;
                ClientId = NextId();
            }
        }


        static void Main(string[] args)
        {
            Department<Account>[] departments = new Department<Account>[]
            {
                new Department<Account>()
                {
                    Account = new Account(31250)
                },
                new DepositDepartment<DepositeAccount>()
                {
                    Account = new DepositeAccount(50123)
                }
            };

            foreach (var d in departments)
            {
                d.Show();
                d.Account.Show();
            }                

            Console.ReadKey();

           Account[] accounts = new Account[3]
            {
                new Account(32321),
                new DepositeAccount(30000),
                new DepositeAccount(30000)
            };

            foreach (var a in accounts)
                a.Show();
        
            /*
            List<Client> clients = new List<Client>()
            {
                new Client("Вася"),
                new Client("Вова"),
                new Client("Костя"),
                new Client("Саша"),
                new Client("Маша")
            };

            foreach(var c in clients)
                Console.WriteLine($"{c.Name} Id:{c.ClientId}");

            string json = JsonConvert.SerializeObject(clients);
            File.WriteAllText("clients.json", json);
            */
            /*
            string json = File.ReadAllText("clients.json");

            List<Client> clients = JsonConvert.DeserializeObject<List<Client>>(json);

            clients.Add(new Client("Вадим"));
            clients.Add(new Client("Лука"));

            foreach (var c in clients)
                Console.WriteLine($"{c.Name} Id:{c.ClientId}");
            */

            DateTime Present = DateTime.Now;
            Console.WriteLine($"Now {Present}");
            DateTime Future = Present.AddDays(72);
            Console.WriteLine($"Future {Future}");
            double gap = Future.Subtract(Present).Days / (365.2425 / 12);
            Console.WriteLine($"Month passed: {Convert.ToInt32(gap)}");
            

            /*
            List<Bank> banks = new List<Bank>();

            banks.Add(new Bank());
            banks.Add(new DepositeBank());

            Console.WriteLine("До");

            foreach (var b in banks)
                b.Show();

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            string jsonDepartments = JsonConvert.SerializeObject(banks, settings);
            File.WriteAllText("banks.json", jsonDepartments);

            banks.Clear();

            var text = File.ReadAllText("banks.json");
            banks = JsonConvert.DeserializeObject<List<Bank>>(text, settings);


            Console.WriteLine("После");

            foreach (var b in banks)
                b.Show();            

            List<Account> accounts = new List<Account>();
                        
            accounts.Add(banks.First().CreateAccount(50000));
            accounts.Add(banks[1].CreateAccount(50000));

            Console.WriteLine("Счета");

            foreach (var a in accounts)
            {
                a.CountIncome();
                a.Show();
            }
            */
            Console.ReadKey();
        }
    }
}
