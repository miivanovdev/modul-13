using System;
using System.Data.Entity;
using Модуль_13_ДЗ.ViewModels;
using Модуль_13_ДЗ.Repos;
using Модуль_13_ДЗ.DataServices;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Главная модель-представление
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Конструктор инкапсулирует создание основных элементов
        /// </summary>
        public MainWindowViewModel(BankContext bankContext, DataInitializer dataInitializer, IWindow window)
        {
            BankContext = bankContext;
            Database.SetInitializer(dataInitializer);
            this.window = window;
        }

        private readonly IWindow window;
        private readonly BankContext BankContext;

        /// <summary>
        /// Модель представление всех счетов
        /// </summary>
        public IAllAccountsViewModel AllAccountsViewModel { get; set; }

        /// <summary>
        /// Модель представление всех департаментов
        /// </summary>
        public IAllDepartmentsViewModel AllDepartmentsViewModel { get; set; }

        /// <summary>
        /// Модель представление всех клиентов
        /// </summary>
        public IAllClientsViewModel AllClientsViewModel { get; set; }

        /// <summary>
        /// Модель представление журнала логгов
        /// </summary>
        public IAllLogsViewModel AllLogsViewModel { get; set; } 

        /// <summary>
        /// Модель представления типов счетов
        /// </summary>
        public IAllAccountTypesViewModel AllAccountTypesViewModel { get; set; }

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
