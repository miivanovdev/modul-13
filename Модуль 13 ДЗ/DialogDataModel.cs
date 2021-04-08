using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ
{
    public class DialogDataModel : IDataErrorInfo
    {
        public DialogDataModel(decimal totalAmount)
        {
            TotalAmount = totalAmount;
        }

        public decimal Amount { get; set; }
        public decimal TotalAmount { get; private set; }

        public bool IsValid
        {
            get
            {
                return Amount > 0 && Amount < TotalAmount;
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Amount":
                        if ((Amount < 0) || (Amount > TotalAmount))
                        {
                            error = "Нельзя снять больше суммы вклада!";                            
                        }                        
                        break;                    
                }
                return error;
            }
        }
        
    }
}
