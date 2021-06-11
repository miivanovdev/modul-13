using System;
using System.Configuration;
using System.Data.Common;

namespace Модуль_13_ДЗ
{
    public class MainViewModel
    {
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

            AllBankAccountViewModel.SelectedClient = AllClientViewModel.SelectedClient;
            AllBankAccountViewModel.SelectedDepartment = AllBankDepartmentViewModel.SelectedDepartment;

            AllClientViewModel.OnClientSelectionChange += AllBankAccountViewModel.SelectionChange;
            AllBankDepartmentViewModel.OnDepartmentSelectionChange += AllBankAccountViewModel.SelectionChange;
        }

        public AllBankAccountViewModel AllBankAccountViewModel { get; set; }

        public AllBankDepartmentViewModel AllBankDepartmentViewModel { get; set; }

        public AllClientViewModel AllClientViewModel { get; set; }

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

        }
    }

    
}
