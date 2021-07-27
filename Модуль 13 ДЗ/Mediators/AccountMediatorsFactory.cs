using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using Модуль_13_ДЗ.Dialogs;

namespace Модуль_13_ДЗ.Mediators
{
    /// <summary>
    /// Фабрика посредников
    /// </summary>
    public class AccountMediatorsFactory : IAccountMediatorsFactory
    {
        public AccountMediatorsFactory(IWindow window, List<DepartmentsViewModel> departments)
        {
            this.window = window;
            this.departments = departments;
        }

        private readonly IWindow window;
        private readonly List<DepartmentsViewModel> departments;

        public List<AccountsViewModel> Accounts { get; set; }

        /// <summary>
        /// Метод создает посредника
        /// </summary>
        /// <param name="dialogType"></param>
        /// <param name="selectedAccount"></param>
        /// <param name="selectedClient"></param>
        /// <returns></returns>
        public TransactionMediator Create(string dialogType, AccountsViewModel selectedAccount, ClientsViewModel selectedClient = null)
        {
            switch(dialogType)
            {
                case "Deposit":

                    if (selectedAccount == null)
                        throw new ArgumentNullException("SelectedClient");

                    DialogAccountToClientViewModel vmDeposit = new DialogAccountToClientViewModel(selectedAccount, selectedClient);
                    return new AccountToClientMediator(window.CreateChild(vmDeposit, new DialogAccountToClient()), vmDeposit);

                case "Withdraw":

                    if (selectedAccount == null)
                        throw new ArgumentNullException("SelectedClient");

                    DialogAccountToClientViewModel vmWithdraw = new DialogAccountToClientViewModel(selectedAccount, selectedClient, true);
                    return new AccountToClientMediator(window.CreateChild(vmWithdraw, new DialogAccountToClient()), vmWithdraw);

                case "Transact":
                    DialogTransactionViewModel vmTransact = new DialogTransactionViewModel(departments, Accounts, selectedAccount);
                    return new AccountToAccountMediator(window.CreateChild(vmTransact, new DialogAccountTransaction()), vmTransact);

                default:
                    return new NullObjectMediator(new NullObjectDialog());

            }
        }        
    }
}
