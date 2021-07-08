using System;
using System.Data.Entity;
using Модуль_13_ДЗ.ViewModels;

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
            BankEntities BankEntities = new BankEntities();
            Database.SetInitializer(new DataInitializer());

            AllClientViewModel = new AllClientViewModel(new ClientEntitiesRepository(BankEntities));

            AllBankDepartmentViewModel = new AllBankDepartmentViewModel(new DepartmentsEntityRepository(BankEntities));

            AllBankAccountViewModel = new AllBankAccountViewModel(new AccountsEntityRepository(BankEntities));

            LogViewModel = new LogViewModel(new LogEntityRepository(BankEntities));
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
