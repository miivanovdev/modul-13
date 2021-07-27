using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using Модуль_13_ДЗ.DataServices;


namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление журнала логов операций
    /// </summary>
    public class AllLogsViewModel : IAllLogsViewModel
    {
        public AllLogsViewModel(ILogsService service)
        {
            this.service = service;
            Log = new ObservableCollection<LogViewModel>(service.GetList());
        }

        /// <summary>
        /// Коллекция сообщения логов
        /// </summary>
        public ObservableCollection<LogViewModel> Log { get; set; }

        /// <summary>
        /// Репозиторий лога оперции
        /// </summary>
        private readonly ILogsService service;

        /// <summary>
        /// Метод записи сообщения в лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="logMessage"></param>
        public void WriteLog(object sender, string logMessage)
        {
            try
            {
                Log.Add(service.Create(logMessage));                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
