﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ
{
    public class DialogDataModel : IDataErrorInfo
    {
        public DialogDataModel(string label, decimal totalAmount, bool isWithdraw)
        {
            TotalAmount = totalAmount;
            IsWithdraw = isWithdraw;
            Label = label;
        }

        public string Label { get; set; }
        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (value < 0)
                    value *= -1;

                amount = value;
            }
        }

        public decimal TotalAmount { get; set; }
        public bool IsWithdraw { get; private set; }

        public bool IsValid
        {
            get
            {
                if(IsWithdraw)
                    return Amount > 0 && Amount < TotalAmount;
                
                return Amount > 0 && Amount < decimal.MaxValue;
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
                        
                        if(IsWithdraw)
                        {
                            if (Amount > TotalAmount)
                                error = "Нельзя снять больше суммы вклада!";                            
                        }
                        else
                        {
                            if (Amount < 0 || Amount > decimal.MaxValue)
                                error = "Недопустимая сумма!";
                        }
                        
                        break;                    
                }
                return error;
            }
        }
        
    }
}
