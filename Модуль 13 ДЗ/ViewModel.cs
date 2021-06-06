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
using System.Data;
using System.Data.Linq;
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

        public Table<Client> ClientsTable { get; set; }
        public Table<BankAccount> AccountsTable { get; set; }
        public Table<BankDepartment<BankAccount>> DepartmentsTable { get; set; }
        public DataContext DataContext { get; set; }

        private SqlConnection SqlConnection { get; set; }

        public ViewModel()
        {
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
        


        /// <summary>
        /// Выбранный клиент
        /// </summary> 
        private Client selectedClient;
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
            DataContext.SubmitChanges();
            SqlConnection.Close();
            SqlConnection.Dispose();
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

            try
            {                
                SqlConnection.Open();

                DataContext = new DataContext(SqlConnection);

                ClientsTable = DataContext.GetTable<Client>();
                Clients = new ObservableCollection<Client>(ClientsTable.ToList());

                AccountsTable = DataContext.GetTable<BankAccount>();
                Accounts = AccountsTable.ToList();

                DepartmentsTable = DataContext.GetTable<BankDepartment<BankAccount>>();
                BankDepartments = DepartmentsTable.ToList();

                foreach (var d in BankDepartments)
                    d.Log = Log;                
            }
            catch (Exception ex)
            {
                SqlConnection.Close();
                SqlConnection.Dispose();
                MessageBox.Show(ex.Message);
                System.Windows.Application.Current.Shutdown();
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
                    {
                        Accounts.Add(account);

                        try
                        {
                            DataContext.GetTable<BankAccount>().InsertOnSubmit(account);
                            DataContext.SubmitChanges();
                        }
                        catch(Exception ex)
                        {                            
                            MessageBox.Show(ex.Message);
                        }
                    }                        
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (Accounts.Remove(SelectedAccount))
                {
                    try
                    {
                        DataContext.GetTable<BankAccount>().DeleteOnSubmit(SelectedAccount);
                        DataContext.SubmitChanges();
                    }
                    catch (Exception ex)
                    {                        
                        MessageBox.Show(ex.Message);
                    }
                    
                    SelectedAccount = null;
                }
            }
        }
        
    }
}
