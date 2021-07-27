using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление лога
    /// </summary>
    public class LogViewModel : ObservableObject
    {
        public LogViewModel(Log log)
        {
            this.log = log;
        }

        /// <summary>
        /// Модель
        /// </summary>
        private readonly Log log;

        /// <summary>
        /// Сообщение лога
        /// </summary>
        public string LogMessage
        {
            get { return log.Message; }
        }

        /// <summary>
        /// Время создания лога
        /// </summary>
        public DateTime Time
        {
            get { return log.Time; }
        }

    }
}
