using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    public class LogViewModel : ObservableObject
    {
        public LogViewModel(Log log)
        {
            this.log = log;
        }

        private readonly Log log;

        public string LogMessage
        {
            get { return log.Message; }
        }

        public DateTime Time
        {
            get { return log.Time; }
        }

    }
}
