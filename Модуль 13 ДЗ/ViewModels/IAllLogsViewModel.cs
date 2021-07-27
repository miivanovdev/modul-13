using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Модуль_13_ДЗ.ViewModels
{
    public interface IAllLogsViewModel
    {
        void WriteLog(object sender, string logMessage);
        ObservableCollection<LogViewModel> Log { get; set; }
    }
}
