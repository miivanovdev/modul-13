using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using ModelLib;

namespace Модуль_13_ДЗ
{
    class ViewModel : ObservableObject
    {
        public ObservableCollection<Client> Clients { get; set; }
        public List<BankDepartment<BankAccount>> BankDepartments { get; set; }       
        public List<BankAccount> Accounts { get; set; }
        public ObservableCollection<LogMessage> Log { get; set; }

        private string DepartmentsFile { get; set; }
        private string AccountsFile { get; set; }
        private string ClientsFile { get; set; }
        private string LogFile { get; set; }

        private SqlConnection SqlConnection { get; set; }

        public ViewModel()
        {
            DepartmentsFile = ConfigurationManager.AppSettings["DepartmentsFile"];
            AccountsFile = ConfigurationManager.AppSettings["AccountsFile"];
            ClientsFile = ConfigurationManager.AppSettings["ClientsFile"];
            LogFile = ConfigurationManager.AppSettings["LogFile"];

            InitData();
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

                selectedDepartment?.Unsubscribe();

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
            File.WriteAllText(ClientsFile, jsonClients);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            string jsonDepartments = JsonConvert.SerializeObject(BankDepartments, settings);
            File.WriteAllText(DepartmentsFile, jsonDepartments);

            string jsonAccounts = JsonConvert.SerializeObject(Accounts, settings);
            File.WriteAllText(AccountsFile, jsonAccounts);

            WriteLogAsync();
        }

        public async void WriteLogAsync()
        {           
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(LogFile)))
            {
                foreach (var l in Log)
                {
                    await sw.WriteLineAsync(JsonConvert.SerializeObject(l));                    
                }
            }            
        }

        #endregion


        /// <summary>
        /// Инициализировать коллекции данных десериализовав json
        /// </summary>
        private void InitData()
        {            
            Log = new ObservableCollection<LogMessage>();            

            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = ConfigurationManager.AppSettings["DataSource"],
                InitialCatalog = ConfigurationManager.AppSettings["InitialCatalog"],
                IntegratedSecurity = Convert.ToBoolean(ConfigurationManager.AppSettings["IntegratedSecurity"]),
                Pooling = Convert.ToBoolean(ConfigurationManager.AppSettings["Pooling"])
            };

            SqlConnection = new SqlConnection(connectionString.ConnectionString);

            string selectClientQuery = "SELECT * FROM Clients ORDER BY ClientId";
            string selectDepartmentQuery = "SELECT * FROM Departments ORDER BY DepartmentId";
            string selectAccountQuery = "SELECT * FROM Accounts ORDER BY AccountId";

            Clients = new ObservableCollection<Client>();

            try
            {
                SqlConnection.Open();

                SqlCommand sqlCommandSelectClient = new SqlCommand(selectClientQuery, SqlConnection);
                SqlDataReader reader = sqlCommandSelectClient.ExecuteReader();

                while(reader.Read())
                {
                    Clients.Add(new Client(reader[1].ToString(), 
                                           reader[2].ToString(),
                                           reader[3].ToString(),
                                           (decimal)reader[4],
                                           (int)reader[0],
                                           (bool)reader[5]));
                }

                reader.Close();
                reader = null;

                SqlCommand sqlCommandSelectDepartments = new SqlCommand(selectDepartmentQuery, SqlConnection);
                reader = sqlCommandSelectDepartments.ExecuteReader();

                BankDepartments = new List<BankDepartment<BankAccount>>();

                if (reader == null)
                    throw new ArgumentException("Не удалось получить Reader!");

                while (reader.Read())
                {
                    switch (Enum.Parse(typeof(AccountType), reader[6].ToString()))
                    {
                        case AccountType.Basic:

                        BankDepartments.Add(new BankDepartment<BankAccount>(Log,
                                                                            reader[1].ToString(),
                                                                            (decimal)reader[2],
                                                                            (uint)((int)reader[4]),
                                                                            (decimal)reader[5],
                                                                            (bool)reader[7],
                                                                            (uint)((int)reader[3])));
                        break;

                        case AccountType.PhysicalAccount:
                        BankDepartments.Add(new PhysicalDepartment(Log,
                                                                    reader[1].ToString(),
                                                                    (decimal)reader[2],
                                                                    (uint)((int)reader[4]),
                                                                    (decimal)reader[5],
                                                                    (bool)reader[7],
                                                                    (uint)((int)reader[3])));
                        break;

                        case AccountType.IndividualAccount:
                        BankDepartments.Add(new IndividualDepartment(Log,
                                                                    reader[1].ToString(),
                                                                    (decimal)reader[2],
                                                                    (uint)((int)reader[4]),
                                                                    (decimal)reader[5],
                                                                    (bool)reader[7],
                                                                    (uint)((int)reader[3])));
                            break;

                        case AccountType.PrivilegedAccount:
                        BankDepartments.Add(new PrivilegedDepartment(Log,
                                                                    reader[1].ToString(),
                                                                    (decimal)reader[2],
                                                                    (uint)((int)reader[4]),
                                                                    (decimal)reader[5],
                                                                    (bool)reader[7],
                                                                    (uint)((int)reader[3])));
                            break;

                    }
                }

                reader.Close();
                reader = null;

                SqlCommand sqlCommandSelectAccounts = new SqlCommand(selectAccountQuery, SqlConnection);
                reader = sqlCommandSelectAccounts.ExecuteReader();

                if (reader == null)
                    throw new ArgumentException("Не удалось получить Reader!");

                Accounts = new List<BankAccount>();

                while (reader.Read())
                {
                    switch (Enum.Parse(typeof(AccountType), reader[9].ToString()))
                    {
                        case AccountType.Basic:
                            
                            break;

                        case AccountType.PhysicalAccount:
                            Accounts.Add(new PhysicalAccount((decimal)reader[4],
                                                            (decimal)reader[8],
                                                            (int)reader[2],
                                                            reader[3].ToString(),
                                                            (int)reader[1],
                                                            (int)reader[7],
                                                            (DateTime)reader[5]));

                            break;

                        case AccountType.IndividualAccount:
                            Accounts.Add(new IndividualAccount((decimal)reader[4],
                                                            (decimal)reader[8],
                                                            (int)reader[2],
                                                            reader[3].ToString(),
                                                            (int)reader[1],
                                                            (int)reader[7],
                                                            (DateTime)reader[5],
                                                            0));
                            break;

                        case AccountType.PrivilegedAccount:
                            Accounts.Add(new PrivilegedAccount((decimal)reader[4],
                                                            (decimal)reader[8],
                                                            (int)reader[2],
                                                            reader[3].ToString(),
                                                            (int)reader[1],
                                                            (int)reader[7],
                                                            (DateTime)reader[5]));
                            break;

                    }
                }

                SqlConnection.Close();
                SqlConnection.Dispose();
            }
            catch(Exception ex)
            {
                SqlConnection.Dispose();
                MessageBox.Show(ex.Message);
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
            try
            {
                SelectedDepartment.OpenAccount(SelectedClient);
            }
            catch(TransactionFailureException ex)
            {
                MessageBox.Show(ex.Info);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), SelectedClient == null ? 0 : SelectedClient.ClientId);
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
            var mediator = new AccountToClientMediator(SelectedClient, SelectedAccount, true);
            mediator.Transaction();
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
            var mediator = new AccountToClientMediator(SelectedClient, SelectedAccount);
            mediator.Transaction();
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
            var mediator = new AccountToAccountMediator(Accounts.Where(x => x != SelectedAccount).ToList(), SelectedAccount);
            mediator.Transaction();
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
            string message = string.Empty;

            if (!SelectedDepartment.CloseAccount(SelectedAccount, out message))
                MessageBox.Show(message);
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
                SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), SelectedClient == null ? 0 : SelectedClient.ClientId);                
            }

            if (e.PropertyName == nameof(SelectedClient)
            && SelectedClient != null)
            {
                SelectedDepartment.GetAccounts(Accounts, new NotifyCollectionChangedEventHandler(AccountsChanged), SelectedClient.ClientId);
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
        
    }
}
