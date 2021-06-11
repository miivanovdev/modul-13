using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    /// <summary>
    /// Исключение возникающее при срыве транзакции
    /// </summary>
    public class TransactionFailureException : ApplicationException
    {
        public TransactionFailureException(string info)
        {
            Info = info;
        }
        
        public string Info { get; set; }
    }
}
