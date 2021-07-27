using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using Модуль_13_ДЗ.Dialogs;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    public class AccountsService : IAccountsService
    {
        public AccountsService(IRepository<Accounts> repository)
        {
            this.repository = repository;
        }

        private readonly IRepository<Accounts> repository;

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public List<AccountsViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        public AccountsViewModel GetOne(int id)
        {
            return WrapOne(repository.GetOne(id));
        }

        public void Rollback()
        {
            repository.Rollback();
        }

        public void Update(AccountsViewModel item)
        {
            repository.Update(item.Accounts);
        }

        public void UpdateRange(AccountsViewModel[] items)
        {
            List<Accounts> itemsModel = new List<Accounts>();

            foreach (var i in items)
                itemsModel.Add(i.Accounts);

            repository.UpdateRange(itemsModel.ToArray());
        }

        /// <summary>
        /// Метод оборачивает коллекцию моделей 
        /// в их модель-представление
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<AccountsViewModel> WrapIntoViewModel(IEnumerable<Accounts> list)
        {
            List<AccountsViewModel>  Accounts = new List<AccountsViewModel>();

            foreach (var l in list)
                Accounts.Add(WrapOne(l));

            return Accounts;
        }

        private AccountsViewModel WrapOne(Accounts item)
        {
            return new AccountsViewModel(item);
        }

        /// <summary>
        /// Метод создания
        /// моделей представления счетов
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public AccountsViewModel Create(int ownerId, string ownerName, decimal amount, decimal interestRate, 
                                        int departmentId, int minTerm, int accountTypeId)
        {
            Accounts item = new Accounts()
            {
                ClientsRefId = ownerId,
                ClientsName = ownerName,
                Amount = amount,
                InterestRate = interestRate,
                DepartmentsRefId = departmentId,
                MinTerm = minTerm,
                AccountTypesId = accountTypeId,
            };
            repository.Create(item);
            AccountsViewModel accountViewModel = new AccountsViewModel(item);

            return accountViewModel;
        }
    }
}
