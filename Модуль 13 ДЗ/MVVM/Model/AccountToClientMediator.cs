using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.MVVM.Model
{
    class AccountToClientMediator : Mediator
    {
        public bool IsWithdraw { get; private set; }

        public AccountToClientMediator(ITransactable client, ITransactable account, bool isWithdraw = false)
        {            
            IsWithdraw = isWithdraw;            
            Sender = client;
            Reciever = account;
            
        }

        public override void Transaction()
        {
            string label = IsWithdraw ? "Снятие со счета" : "Пополнение счета";

            DialogViewModel dialogVM = new DialogViewModel(label, Reciever.AmountAvailable, IsWithdraw);
            DialogWindow dialogWindow = new DialogWindow(dialogVM, label);

            if (dialogWindow.ShowDialog() == true)
            {
                if(IsWithdraw)
                {
                    Sender.Put(dialogVM.Amount);
                    Reciever.Withdraw(dialogVM.Amount);
                }
                else
                {
                    Sender.Withdraw(dialogVM.Amount);
                    Reciever.Put(dialogVM.Amount);
                }
            }
        }
    }
}
