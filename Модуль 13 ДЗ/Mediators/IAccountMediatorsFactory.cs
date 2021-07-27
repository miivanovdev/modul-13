using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.Mediators
{
    public interface IAccountMediatorsFactory
    {
        List<AccountsViewModel> Accounts { get; set; }
        TransactionMediator Create(string dialogType, AccountsViewModel selectedAccount, ClientsViewModel selectedClient = null);
    }
}
