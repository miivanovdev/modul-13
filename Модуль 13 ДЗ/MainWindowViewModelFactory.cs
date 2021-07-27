using System;
using Модуль_13_ДЗ.ViewModels;
using Модуль_13_ДЗ.DataServices;
using Модуль_13_ДЗ.Repos;
using Модуль_13_ДЗ.Mediators;

namespace Модуль_13_ДЗ
{
    public class MainWindowViewModelFactory : IMainWindowViewModelFactory
    {
        private readonly BankContext context;
        private readonly DataInitializer dataInitializer;

        public MainWindowViewModelFactory(BankContext agent, DataInitializer dataInitializer)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            this.context = agent;

            if (dataInitializer == null)
            {
                throw new ArgumentNullException("dataInitializer");
            }

            this.dataInitializer = dataInitializer;
        }

        #region IViewModelFactory Members

        public MainWindowViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }

            var vm = new MainWindowViewModel(this.context, this.dataInitializer, window);

            vm.AllClientsViewModel = new AllClientsViewModel(
                                            new ClientsService(
                                                new ClientEntitiesRepository(context)));

            vm.AllDepartmentsViewModel = new AllDepartmentsViewModel(
                                            new DepartmentsService(
                                                new DepartmentsEntityRepository(context)));

            vm.AllAccountsViewModel = new AllAccountsViewModel(
                                            new AccountsService(
                                                new AccountsEntityRepository(context)),
                                            new AccountMediatorsFactory(window, vm.AllDepartmentsViewModel.Departments));

            vm.AllAccountTypesViewModel = new AllAccountTypesViewModel(
                                            new AccountTypesService(
                                                new AccountTypesEntityRepository(context)));

            vm.AllLogsViewModel = new AllLogsViewModel(new LogsDataService(new LogEntityRepository(context)));

            vm.AllAccountsViewModel.SelectedClient = vm.AllClientsViewModel.SelectedClient;
            vm.AllAccountsViewModel.SelectedDepartment = vm.AllDepartmentsViewModel.SelectedDepartment;

            vm.AllClientsViewModel.OnClientSelectionChange += vm.AllAccountsViewModel.SelectionChange;
            vm.AllDepartmentsViewModel.OnDepartmentSelectionChange += vm.AllAccountsViewModel.SelectionChange;

            vm.AllAccountsViewModel.AccountChangedEvent += vm.AllLogsViewModel.WriteLog;

            return vm;
        }

        #endregion
    }
}
