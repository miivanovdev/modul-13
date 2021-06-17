using System;
using System.Configuration;
using System.Data.Common;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Главная модель-представление
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Конструктор инкапсулирует создание основных элементов
        /// </summary>
        public MainViewModel()
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
            builder.Add("Data Source", ConfigurationManager.AppSettings["DataSource"]);
            builder.Add("Initial Catalog", ConfigurationManager.AppSettings["InitialCatalog"]);
            builder.Add("Integrated Security", Convert.ToBoolean(ConfigurationManager.AppSettings["IntegratedSecurity"]));
            builder.Add("Pooling", Convert.ToBoolean(ConfigurationManager.AppSettings["Pooling"]));
                        
            AllClientViewModel = new AllClientViewModel(new SqlClientRepository(builder.ConnectionString, "getAllClients", "selectClient", "updateClient", "deleteClient", "createClient"));

            AllBankDepartmentViewModel = new AllBankDepartmentViewModel(new SqlBankDepartmentRepository(builder.ConnectionString, "getAllDepartments", "", "", "", ""));

            AllBankAccountViewModel = new AllBankAccountViewModel(new SqlBankAccountRepository(builder.ConnectionString, "getAllAccounts", "selectAccount", "updateAccount", "deleteAccount", "createAccount"));

            LogViewModel = new LogViewModel(new SqlLogRepository(builder.ConnectionString, "getAllLog", "selectLog", "", "deleteLog", "createLog"));

            AllBankAccountViewModel.SelectedClient = AllClientViewModel.SelectedClient;
            AllBankAccountViewModel.SelectedDepartment = AllBankDepartmentViewModel.SelectedDepartment;

            AllClientViewModel.OnClientSelectionChange += AllBankAccountViewModel.SelectionChange;
            AllBankDepartmentViewModel.OnDepartmentSelectionChange += AllBankAccountViewModel.SelectionChange;

            AllBankAccountViewModel.AccountChangedEvent += LogViewModel.WriteLog;
        }

        /// <summary>
        /// Модель представление всех счетов
        /// </summary>
        public AllBankAccountViewModel AllBankAccountViewModel { get; set; }

        /// <summary>
        /// Модель представление всех департаментов
        /// </summary>
        public AllBankDepartmentViewModel AllBankDepartmentViewModel { get; set; }

        /// <summary>
        /// Модель представление всех клиентов
        /// </summary>
        public AllClientViewModel AllClientViewModel { get; set; }

        /// <summary>
        /// Модель представление журнала логгов
        /// </summary>
        public LogViewModel LogViewModel { get; set; } 

        private RelayCommand closeCommand;
        /// <summary>
        /// Команда при закрытии приложения
        /// </summary>
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

        }
    }

    
}
