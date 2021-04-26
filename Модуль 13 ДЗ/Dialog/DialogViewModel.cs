﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class DialogViewModel : ObservableObject
    {
        public DialogDataModel Data { get; set; }

        public DialogViewModel(string label, decimal totalAmount, bool isWithdraw)
        {
            Data = new DialogDataModel(label, totalAmount, isWithdraw);
        }

        public decimal Amount
        {
            get
            {
                return Data.Amount;
            }
        }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult == value)
                    return;

                dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));
            }
        }
        
        private RelayCommand okCommand;

        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                (okCommand = new RelayCommand(new Action<object>(OkClose),
                                              new Func<object, bool>(CanClose)
                ));
            }
        }

        private void OkClose(object o)
        {
            DialogResult = true;
        }

        private bool CanClose(object o)
        {
            return Data.IsValid; 
        }
    }
}
