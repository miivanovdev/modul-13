using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using Модуль_13_ДЗ.MVVM.Model;

namespace Модуль_13_ДЗ
{
    class ViewModel : ObservableObject
    {
        public ObservableCollection<Client> Clients { get; set; }
        public List<BankDepartment<BankAccount>> BankDepartments { get; set; }       
        public List<BankAccount> Accounts { get; set; }
        public ObservableCollection<LogMessage> Log { get; set; }

        public ViewModel()
        {
            #region.Начальная инициализация
            
            Log = new ObservableCollection<LogMessage>();

            Clients = new ObservableCollection<Client>()
            {
                new Client("Кулибяка", "Вадим", "Натанович", 27450, true),
                new Client("Пыпырин", "Владимир", "Юльевич", 38850),
                new Client("Прокопов", "Алексей", "Александрович", 40450),
                new Client("Никишин", "Олег", "Викторович", 120250),
                new Client("Крупская", "Анна", "Сергеевна", 117300),
                new Client("Коняев", "Станислав", "Валерьевич", 301200),
                new Client("Чизмар", "Валентина", "Витальевна", 178500)
            };
                        
            BankDepartments = new List<BankDepartment<BankAccount>>()
            {
                new BankDepartment<BankAccount>(Log, "Не выбрано!", 0, 0, 0, true),
                new PhysicalDepartment(Log, "Отдел по работе с физическими лицами", 50000, 6, 15),
                new IndividualDepartment(Log, "Отдел по работе с юридическими лицами", 30000, 12, 15),
                new PrivilegedDepartment(Log, "Отдел по работе с привелигированными клиентами", 100000, 18, 20)
            };
            
            Accounts = new List<BankAccount>()
            {
                new PhysicalAccount(40000, 10, Clients[0].ClientId, Clients[0].FIO, 1 , 6, new DateTime(2020, 11, 05)),
                new PhysicalAccount(78540, 12, Clients[0].ClientId, Clients[0].FIO, 1, 6, new DateTime(2019, 10, 23)),
                new PhysicalAccount(63400, 10, Clients[1].ClientId, Clients[1].FIO, 1, 6, new DateTime(2020, 09, 04)),
                new PhysicalAccount(-48900, 10, Clients[1].ClientId, Clients[1].FIO, 1, 6, new DateTime(2020, 06, 12)),
                new IndividualAccount(34000, 10, Clients[2].ClientId, Clients[2].FIO, 2, 12, new DateTime(2019, 01, 24), 2),
                new IndividualAccount(-500000, 10, Clients[2].ClientId, Clients[2].FIO, 2, 12, new DateTime(2021, 01, 13), 2),
                new IndividualAccount(1240000, 10, Clients[3].ClientId, Clients[3].FIO, 2, 12, new DateTime(2020, 08, 07), 2),
                new IndividualAccount(12700, 10, Clients[3].ClientId, Clients[3].FIO, 2, 12, new DateTime(2020, 02, 14), 2),
                new IndividualAccount(481500, 15, Clients[4].ClientId, Clients[4].FIO, 2, 12, new DateTime(2020, 07, 15), 2),
                new PrivilegedAccount(3012250, 20, Clients[5].ClientId, Clients[5].FIO, 3, 18, new DateTime(2018, 10, 12)),
                new PrivilegedAccount(2012250, 15, Clients[6].ClientId, Clients[6].FIO, 3, 18, new DateTime(2019, 11, 21)),
            };
            
            #endregion
            //InitData();
            PropertyChanged += new PropertyChangedEventHandler(SelectionChangeHandler);

            SelectedDepartment = BankDepartments.First();
            SelectedClient = Clients.First();
                     
            SelectedAccount = SelectedDepartment.Accounts.First();
        }
               
        private BankDepartment<BankAccount> selectedDepartment;

        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public BankDepartment<BankAccount> SelectedDepartment
        {
            get
            {
                return selectedDepartment;
            }
            set
            {
                if (selectedDepartment == value)
                    return;

                selectedDepartment = value;
                NotifyPropertyChanged(nameof(SelectedDepartment));
            }
        }

        private Client selectedClient;

        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public Client SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                if (selectedClient == value)
                    return;

                selectedClient = value;
                NotifyPropertyChanged(nameof(SelectedClient));
            }
        }

        private BankAccount selectedAccount;

        /// <summary>
        /// Выбранный счет
        /// </summary>
        public BankAccount SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                if (selectedAccount == value)
                    return;

                selectedAccount = value;
                NotifyPropertyChanged(nameof(SelectedAccount));
            }
        }

        #region.Закрытие приложения
        /// <summary>
        /// Команда закрытия приложения
        /// </summary>
        private RelayCommand closeCommand;        

        public RelayCommand CloseCommand
        {
            get
            {
                return closeCommand ??
                (closeCommand = new RelayCommand(new Action<object>(SaveData)
                ));
            }
        }

        /// <summary>
        /// Метод сохранения данных в файл
        /// </summary>
        /// <param name="args"></param>
        private void SaveData(object args)
        {
            string jsonClients = JsonConvert.SerializeObject(Clients);
            File.WriteAllText("Clients.json", jsonClients);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            string jsonDepartments = JsonConvert.SerializeObject(BankDepartments, settings);
            File.WriteAllText("Departments.json", jsonDepartments);            

            string jsonAccounts = JsonConvert.SerializeObject(Accounts, settings);
            File.WriteAllText("Accounts.json", jsonAccounts);

            string jsonLog = JsonConvert.SerializeObject(Log, settings);
            File.WriteAllText("Log.json", jsonLog);
        }
        #endregion


        /// <summary>
        /// Инициализировать коллекции данных десериализовав json
        /// </summary>
        private void InitData()
        {
            if (File.Exists("Clients.json"))
            {
                string jsonClients = File.ReadAllText("Clients.json");
                Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonClients);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            if (File.Exists("Departments.json"))
            {
                string jsonDepartments = File.ReadAllText("Departments.json");
                BankDepartments = JsonConvert.DeserializeObject<List<BankDepartment<BankAccount>>>(jsonDepartments, settings);
            }            

            if (File.Exists("Accounts.json"))
            {
                string jsonAccounts = File.ReadAllText("Accounts.json");
                Accounts = JsonConvert.DeserializeObject<List<BankAccount>>(jsonAccounts, settings);
            }

            if (File.Exists("Log.json"))
            {
                string jsonLog = File.ReadAllText("Log.json");
                Log = JsonConvert.DeserializeObject<ObservableCollection<LogMessage>>(jsonLog, settings);
            }
        }


        #region.Commands
        /// <summary>
        /// Команда добавления клиента
        /// </summary>
        private RelayCommand addClientCommand;

        public RelayCommand AddClientCommand
        {
            get
            {
                return addClientCommand ??
                (addClientCommand = new RelayCommand(new Action<object>(AddClient),
                                                     new Func<object, bool>(ClientCanBeAdded)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности вызова команды добавления клиента
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool ClientCanBeAdded(object o)
        {
            return SelectedDepartment != null;
        }

        /// <summary>
        /// Метод добавления нового клиента
        /// </summary>
        /// <param name="o"></param>
        private void AddClient(object o)
        {
            SelectedDepartment.AddClient(Clients);
            NotifyPropertyChanged(nameof(Clients));
        }

        /// <summary>
        /// Команда удаления клиента
        /// </summary>
        private RelayCommand removeClientCommand;

        public RelayCommand RemoveClientCommand
        {
            get
            {
                return removeClientCommand ??
                (removeClientCommand = new RelayCommand(new Action<object>(RemoveClient),
                                                     new Func<object, bool>(CanClientBeRemove)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности удаления клиента
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClientBeRemove(object o)
        {
            return SelectedClient != null &&
                   !Accounts.Exists(x => x.OwnerId == SelectedClient.ClientId);
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="o"></param>
        private void RemoveClient(object o)
        {
            if (!Clients.Remove(SelectedClient))
                MessageBox.Show("Не удалось удалить клиента!");

            if(Clients.Count > 0)
                SelectedClient = Clients.First();
        }

        /// <summary>
        /// Команда открытия счета
        /// </summary>
        private RelayCommand openAccountCommand;

        public RelayCommand OpenAccountCommand
        {
            get
            {
                return openAccountCommand ??
                (openAccountCommand = new RelayCommand(new Action<object>(OpenAccount),
                                                      new Func<object, bool>(AccountCanBeOpened)
                ));
            }
        }

        /// <summary>
        /// Метод проверки возможности вызова команды открытия счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeOpened(object o)
        {
            return SelectedDepartment != null &&
                   SelectedDepartment.DepartmentId > 0 &&
                   SelectedClient != null &&
                   !SelectedClient.BadHistory;
                   
        }

        /// <summary>
        /// Метод открытия счета
        /// </summary>
        /// <param name="o"></param>
        private void OpenAccount(object o)
        {
            SelectedDepartment.OpenAccount(SelectedClient);
            SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), new NotifyCollectionChangedEventHandler(LogChanged), SelectedClient == null ? 0 : SelectedClient.ClientId);
        }
        

        /// <summary>
        /// Команда для списания средств со счета
        /// </summary>
        private RelayCommand withdrawCommand;

        public RelayCommand WithdrawCommand
        {
            get
            {
                return withdrawCommand ??
                (withdrawCommand = new RelayCommand(new Action<object>(Withdraw), new Func<object, bool>(AccountCanBeWithdrawed)));
            }
        }
                
        /// <summary>
        /// Метод проверки возможности снятия средств
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeWithdrawed(object o)
        {
            return SelectedClient != null &&
                   SelectedAccount != null &&
                   SelectedAccount.CanWithdrawed;
        }

        /// <summary>
        /// Вызов снятия со счета
        /// </summary>
        /// <param name="sender"></param>
        private void Withdraw(object sender)
        {
            SelectedAccount.Withdraw(this);
        }
                
        /// <summary>
        /// Команда внесения средств на счет
        /// </summary>
        private RelayCommand addCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new RelayCommand(new Action<object>(Add), new Func<object, bool>(AccountCanBeAdd)));
            }
        }

        /// <summary>
        /// Метод проверки возможности добавления средств
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeAdd(object o)
        {
            return SelectedClient != null &&
                   SelectedAccount != null &&                   
                   SelectedAccount.CanAdded;
        }

        /// <summary>
        /// Вызов поплнения счета
        /// </summary>
        /// <param name="sender"></param>
        private void Add(object sender)
        {
            SelectedAccount.Add(this);
        }
                
        /// <summary>
        /// Команда вызова транзакции
        /// </summary>
        private RelayCommand transactCommand;

        public RelayCommand TransactCommand
        {
            get
            {
                return transactCommand ??
                (transactCommand = new RelayCommand(new Action<object>(Transact), new Func<object, bool>(CanTransact)));
            }
        }
                
        private bool CanTransact(object o)
        {
            return SelectedAccount != null && SelectedAccount.CanTransact;
        }

        /// <summary>
        /// Вызвать перевод
        /// </summary>
        /// <param name="sender"></param>
        private void Transact(object sender)
        {
            SelectedAccount.Transact(Accounts);
        }
                
        /// <summary>
        /// Команда для списания средств со счета
        /// </summary>
        private RelayCommand closeAccountCommand;

        public RelayCommand CloseAccountCommand
        {
            get
            {
                return closeAccountCommand ??
                (closeAccountCommand = new RelayCommand(new Action<object>(CloseAccount), new Func<object, bool>(AccountCanBeClosed)));
            }
        }

        /// <summary>
        /// Метод закрытия счета
        /// </summary>
        /// <param name="o"></param>
        private void CloseAccount(object o)
        {
            SelectedDepartment.CloseAccount(SelectedAccount);
        }

        /// <summary>
        /// Проверка на возможность закрытия счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeClosed(object o)
        {
            return SelectedAccount != null &&
                   SelectedDepartment != null &&
                   SelectedAccount.CanClose;
        }

        #endregion

        /// <summary>
        /// Действия при смене департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectionChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedDepartment))
            {
                SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), new NotifyCollectionChangedEventHandler(LogChanged), SelectedClient == null ? 0 : SelectedClient.ClientId);                
            }

            if (e.PropertyName == nameof(SelectedClient)
            && SelectedClient != null)
            {
                SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), new NotifyCollectionChangedEventHandler(LogChanged), SelectedClient.ClientId);
            }
        }
               
        /// <summary>
        /// Действия при изменении в коллекции счетов 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AccountsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var a in e.NewItems)
                {
                    BankAccount account = (BankAccount)a;

                    if (!Accounts.Contains(account))
                        Accounts.Add(account);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (Accounts.Remove(SelectedAccount))
                    SelectedAccount = null;       
            }
        }

        /// <summary>
        /// Действия при изменении в коллекции логов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LogChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(var l in e.NewItems)
                {
                    LogMessage lm = l as LogMessage;

                    if (!Log.Contains(lm))
                        Log.Add(lm);

                }
            }
        }
    }
}
