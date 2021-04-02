using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;

namespace Модуль_13_ДЗ
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> Clients { get; set; }
        public List<BankAccount> Accounts { get; set; }
        public List<BankDepartment> BankDepartments { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            #region.Начальная инициализация
            
            Clients = new ObservableCollection<Client>()
            {
                new Client("Кулибяка", "Вадим", "Натанович", 27450, 12300),
                new Client("Пыпырин", "Владимир", "Юльевич", 38850, 17640),
                new Client("Прокопов", "Алексей", "Александрович", 40450, 22300),
                new Client("Никишин", "Олег", "Викторович", 120250, 33400),
                new Client("Крупская", "Анна", "Сергеевна", 117300, 52300),
                new Client("Коняев", "Станислав", "Валерьевич", 301200, 72300),
                new Client("Чизмар", "Валентина", "Витальевна", 178500, 42500)
            };
                        
            BankDepartments = new List<BankDepartment>()
            {
                new BankDepartment("Не выбрано!", 0, 0, true),
                new BankDepartment("Отдел по работе с физическими лицами", 6, 15),
                new BankDepartment("Отдел по работе с юридическими лицами", 12, 15),
                new BankDepartment("Отдел по работе с привелигированными клиентами", 18, 20)
            };
            
            Accounts = new List<BankAccount>()
            {
                new BankAccount(40000,10,1, 1, new DateTime(2020, 11, 05)),
                new BankAccount(78540,12,1, 1, new DateTime(2019, 10, 23)),
                new BankAccount(63400,10,2, 1, new DateTime(2020, 09, 04)),
                new BankAccount(-48900,10,2, 1, new DateTime(2020, 06, 12)),
                new BankAccount(34000,10,2, 2, new DateTime(2019, 01, 24)),
                new BankAccount(-500000,10,3, 2, new DateTime(2021, 01, 13)),
                new BankAccount(1240000,10,3, 2, new DateTime(2020, 08, 07)),
                new BankAccount(12700,10,4, 3, new DateTime(2020, 02, 14)),
                new BankAccount(-481500,15,5, 3, new DateTime(2020, 07, 15)),
                new BankAccount(3012250,20,6, 3, new DateTime(2018, 10, 12), true),
                new BankAccount(2012250,15,7, 3, new DateTime(2019, 11, 21), true),
            };
            
            #endregion
            //InitData();
            PropertyChanged += new PropertyChangedEventHandler(SelectionChangeHandler);

            SelectedDepartment = BankDepartments.First();
            SelectedClient = Clients.First();
                     
            SelectedAccount = SelectedDepartment.Accounts.First();
        }

        private BankDepartment selectedDepartment;

        public BankDepartment SelectedDepartment
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
            string jsonDepartments = JsonConvert.SerializeObject(BankDepartments);
            File.WriteAllText("Departments.json", jsonDepartments);

            string jsonClients = JsonConvert.SerializeObject(Clients);
            File.WriteAllText("Clients.json", jsonDepartments);

            string jsonAccounts = JsonConvert.SerializeObject(Accounts);
            File.WriteAllText("Accounts.json", jsonAccounts);
        }
        #endregion


        /// <summary>
        /// Инициализировать коллекции данных десериализовав json
        /// </summary>
        private void InitData()
        {
            if(File.Exists("Departments.json"))
            {
                string jsonDepartments = File.ReadAllText("Departments.json");
                BankDepartments = JsonConvert.DeserializeObject<List<BankDepartment>>(jsonDepartments);
            }

            if (File.Exists("Clients.json"))
            {
                string jsonDepartments = File.ReadAllText("Clients.json");
                Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonDepartments);
            }

            if (File.Exists("Accounts.json"))
            {
                string jsonDepartments = File.ReadAllText("Accounts.json");
                Accounts = JsonConvert.DeserializeObject<List<BankAccount>>(jsonDepartments);
            }      
        }

        /// <summary>
        /// Команда добавления счета
        /// </summary>
        private RelayCommand addAccountCommand;

        public RelayCommand AddAccountCommand
        {
            get
            {
                return addAccountCommand ??
                (addAccountCommand = new RelayCommand(new Action<object>(AddAccount)
                ));
            }
        }

        /// <summary>
        /// Метод добавления счета
        /// </summary>
        /// <param name="o"></param>
        private void AddAccount(object o)
        {
            //SelectedDepartment.Accounts.Add(new BankAccount(0, 0, SelectedDepartment.InterestRate, SelectedClient.ClientId, SelectedDepartment.DepartmentId, DateTime.Now));
        }        

        /// <summary>
        /// Метод запуска события изменения свойства
        /// </summary>
        /// <param name="propertyName">Изменеяемое свойство</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
