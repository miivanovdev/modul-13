using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{ 
    public class LogMessage
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }

        public LogMessage() { }

        public LogMessage(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
    }
}
