using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Модуль_13_ДЗ.ViewModels
{
    public interface IAllAccountsViewModel
    {
        /// <summary>
        /// Коллекция счетов
        /// </summary>
        List<AccountsViewModel> Accounts { get; set; }
        ObservableCollection<AccountsViewModel> AccountsView { get; }
        AccountsViewModel SelectedAccount { get; set; }
        DepartmentsViewModel SelectedDepartment { get; set; }
        ClientsViewModel SelectedClient { get; set; }
        event Action<object, string> AccountChangedEvent;
        void SelectionChange(object sender);

        RelayCommand OpenAccountCommand { get; }
        RelayCommand WithdrawCommand { get; }
        RelayCommand DepositCommand { get; }
        RelayCommand TransactCommand { get; }
        RelayCommand CloseAccountCommand { get; }

    }
}
