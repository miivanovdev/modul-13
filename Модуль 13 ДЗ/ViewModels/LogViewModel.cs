using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление журнала логов операций
    /// </summary>
    public class LogViewModel
    {
        public LogViewModel(IRepository<Log> repository)
        {
            Repository = repository;
            Log = new List<Log>();

            Log.AddRange(Repository.GetList());
        }

        /// <summary>
        /// Коллекция сообщения логов
        /// </summary>
        public List<Log> Log { get; set; }

        /// <summary>
        /// Репозиторий лога оперции
        /// </summary>
        private readonly IRepository<Log> Repository;

        /// <summary>
        /// Метод записи сообщения в лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="logMessage"></param>
        public void WriteLog(object sender, Log logMessage)
        {
            try
            {
                Repository.Create(logMessage);
                Log.Add(logMessage);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
