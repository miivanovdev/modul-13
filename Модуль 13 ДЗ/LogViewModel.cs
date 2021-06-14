using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class LogViewModel
    {
        public LogViewModel(IRepository<LogMessage> repository)
        {
            Repository = repository;
            Log = new List<LogMessage>();

            Log.AddRange(Repository.GetList());
        }

        public List<LogMessage> Log { get; set; }

        private readonly IRepository<LogMessage> Repository;

        public void WriteLog(object sender, LogMessage logMessage)
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
