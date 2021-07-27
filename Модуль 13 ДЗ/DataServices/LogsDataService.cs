using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    /// <summary>
    /// Сервис логов
    /// </summary>
    public class LogsDataService : ILogsService
    {
        public LogsDataService(IRepository<Log> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Интерфейс взаимодействия с хранилищем
        /// </summary>
        private readonly IRepository<Log> repository;

        public LogViewModel Create(string message)
        {
            var log = new Log(message);
            repository.Create(log);
            return new LogViewModel(log);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить списко логов
        /// </summary>
        /// <returns></returns>
        public List<LogViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        /// <summary>
        /// Метод оборачивает список моделей
        /// в их модель-представление
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<LogViewModel> WrapIntoViewModel(IEnumerable<Log> list)
        {
            List<LogViewModel> logs = new List<LogViewModel>();

            foreach (var l in list)
                logs.Add(WrapOne(l));

            return logs;
        }

        /// <summary>
        /// Метод оборачивает модель
        /// в её модель-представление
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private LogViewModel WrapOne(Log item)
        {
            return new LogViewModel(item);
        }

        public LogViewModel GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Update(LogViewModel item)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(LogViewModel[] items)
        {
            throw new NotImplementedException();
        }
    }
}
