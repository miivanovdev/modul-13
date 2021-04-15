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
        public List<BankAccount> Accounts { get; set; }
        public List<BankDepartment<BankAccount>> BankDepartments { get; set; }

        public ViewModel()
        {
            #region.Начальная инициализация
            
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
                new BankDepartment<BankAccount>("Не выбрано!", 0, 0, 0, true),
                new PhysicalDepartment("Отдел по работе с физическими лицами", 50000, 6, 15),
                new IndividualDepartment("Отдел по работе с юридическими лицами", 30000, 12, 15),
                new PrivilegedDepartment("Отдел по работе с привелигированными клиентами", 100000, 18, 20)
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
        }
        #endregion


        /// <summary>
        /// Инициализировать коллекции данных десериализовав json
        /// </summary>
        private void InitData()
        {
            if (File.Exists("Clients.json"))
            {
                string jsonDepartments = File.ReadAllText("Clients.json");
                Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonDepartments);
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
                string jsonDepartments = File.ReadAllText("Accounts.json");
                Accounts = JsonConvert.DeserializeObject<List<BankAccount>>(jsonDepartments, settings);
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
                   SelectedClient != null &&
                   SelectedDepartment.DepartmentId > 0;
        }

        /// <summary>
        /// Метод открытия счета
        /// </summary>
        /// <param name="o"></param>
        private void OpenAccount(object o)
        {
            SelectedDepartment.OpenAccount(Accounts, SelectedClient);
            SelectedDepartment.GetAccounts(Accounts, SelectedClient == null ? 0 : SelectedClient.ClientId);
        }
        #endregion

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
            return SelectedDepartment != null &&
                   SelectedClient != null &&
                   SelectedAccount != null &&
                   SelectedDepartment.DepartmentId > 0 &&
                   SelectedAccount.CanWithdrawed;
        }

        /// <summary>
        /// Метод снятия со счета
        /// </summary>
        /// <param name="o"></param>
        private void Withdraw(object o)
        {
            try
            {
                SelectedAccount.Amount -= ShowDialog("Снятие средств со счета", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
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
            return SelectedDepartment != null &&
                   SelectedClient != null &&
                   SelectedAccount != null &&
                   SelectedDepartment.DepartmentId > 0 &&
                   SelectedAccount.CanAdded;
        }


        /// <summary>
        /// Метод внесения средств на счет
        /// </summary>
        /// <param name="o"></param>
        private void Add(object o)
        {
            try
            {
                SelectedAccount.Amount += ShowDialog("Внесение средств на счет");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Вывод диалогового окна для внесения/снятия
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="isWithdraw"></param>
        /// <returns></returns>
        private decimal ShowDialog(string operationName, bool isWithdraw = false)
        {
            DialogViewModel dialogVM = new DialogViewModel(operationName, SelectedAccount.Amount, isWithdraw);
            DialogWindow dialogWindow = new DialogWindow(dialogVM, operationName);

            if (dialogWindow.ShowDialog() == true)
                return dialogVM.Amount;
            
            return 0;
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
                (transactCommand = new RelayCommand(new Action<object>(TransactionDialog), new Func<object, bool>(CanTransact)));
            }
        }

        private void TransactionDialog(object o)
        {
            try
            {
                TransactionDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanTransact(object o)
        {
            return SelectedAccount != null && SelectedAccount.CanTransact;
        }

        /// <summary>
        /// Вывод диалогового окна для внесения/снятия
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="isWithdraw"></param>
        /// <returns></returns>
        private decimal TransactionDialog()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel(Accounts.Where( x => x != SelectedAccount).ToList());
            DialogTransaction dialogTransaction = new DialogTransaction(transactionViewModel);

            if (dialogTransaction.ShowDialog() == true)
            {
                SelectedAccount.Amount -= transactionViewModel.Amount;
                transactionViewModel.SelectedAccount.Amount += transactionViewModel.Amount;
            }

            return 0;
        }

        /// <summary>
        /// Действия при смене департамента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectionChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedDepartment))
            {
                SelectedDepartment.GetAccounts(Accounts, SelectedClient == null ? 0 : SelectedClient.ClientId);
            }

            if (e.PropertyName == nameof(SelectedClient)
            && SelectedClient != null)
            {
                SelectedDepartment.GetAccounts(Accounts, SelectedClient.ClientId);
                SelectedDepartment.Accounts.CollectionChanged += new NotifyCollectionChangedEventHandler(AccountsChanged);
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
        }
    }
}
