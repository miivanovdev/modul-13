using System;

namespace Модуль_13_ДЗ.Mediators
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
