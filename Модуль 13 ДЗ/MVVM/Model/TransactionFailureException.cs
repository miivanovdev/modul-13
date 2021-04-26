using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    /// <summary>
    /// Исключение возникающее при срыве транзакции
    /// </summary>
    class TransactionFailureException : ApplicationException
    {
        public TransactionFailureException(string info)
        {
            Info = info;
        }
        
        public string Info { get; set; }
    }
}
