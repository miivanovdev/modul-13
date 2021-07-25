using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    public class AccountTypesService : IAccountTypesService
    {
        public AccountTypesService(IRepository<AccountTypes> repository)
        {
            this.repository = repository;
        }

        private readonly IRepository<AccountTypes> repository;

        public AccountTypesViewModel Create()
        {
            AccountTypes item = new AccountTypes(); //фабрика
            repository.Create(item);

            return WrapOne(item);
        }

        public List<AccountTypesViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        /// <summary>
        /// Метод оборачивает коллекцию моделей типов счетов
        /// в модель представление
        /// </summary>
        /// <param name="list"></param>
        private List<AccountTypesViewModel> WrapIntoViewModel(IEnumerable<AccountTypes> list)
        {
            List<AccountTypesViewModel> Types = new List<AccountTypesViewModel>();

            foreach (var l in list)
                Types.Add(WrapOne(l));

            return Types;
        }

        private AccountTypesViewModel WrapOne(AccountTypes item)
        {
            return new AccountTypesViewModel(item);
        }

        public AccountTypesViewModel GetOne(int id)
        {
            return new AccountTypesViewModel(repository.GetOne(id));
        }

        public void Update(AccountTypesViewModel item)
        {
            repository.Update(item.AccountTypes);
        }

        public void UpdateRange(AccountTypesViewModel[] items)
        {
            List<AccountTypes> itemsModel = new List<AccountTypes>();

            foreach (var i in items)
                itemsModel.Add(i.AccountTypes);

            repository.UpdateRange(itemsModel.ToArray());
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public void Rollback()
        {
            repository.Rollback();
        }
    }
}
