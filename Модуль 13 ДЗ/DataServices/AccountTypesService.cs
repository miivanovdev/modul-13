using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;

namespace Модуль_13_ДЗ.DataServices
{
    /// <summary>
    /// Сервис типов счетов
    /// </summary>
    public class AccountTypesService : IAccountTypesService
    {
        public AccountTypesService(IRepository<AccountTypes> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Интерфейс взаймодействия с хранилищем
        /// </summary>
        private readonly IRepository<AccountTypes> repository;

        /// <summary>
        /// Создать тип счета
        /// </summary>
        /// <returns></returns>
        public AccountTypesViewModel Create()
        {
            AccountTypes item = new AccountTypes(); //фабрика
            repository.Create(item);

            return WrapOne(item);
        }

        /// <summary>
        /// Получить список типов счетов
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Метод оборачивает модель в модель-представление
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private AccountTypesViewModel WrapOne(AccountTypes item)
        {
            return new AccountTypesViewModel(item);
        }

        /// <summary>
        /// Получить тип счета по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountTypesViewModel GetOne(int id)
        {
            return new AccountTypesViewModel(repository.GetOne(id));
        }

        /// <summary>
        /// Обновить тип счета
        /// </summary>
        /// <param name="item"></param>
        public void Update(AccountTypesViewModel item)
        {
            repository.Update(item.AccountTypes);
        }

        /// <summary>
        /// Обновить несколько типов счетов
        /// </summary>
        /// <param name="items"></param>
        public void UpdateRange(AccountTypesViewModel[] items)
        {
            List<AccountTypes> itemsModel = new List<AccountTypes>();

            foreach (var i in items)
                itemsModel.Add(i.AccountTypes);

            repository.UpdateRange(itemsModel.ToArray());
        }

        /// <summary>
        /// Удалить тип счета по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            repository.Delete(id);
        }

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        public void Rollback()
        {
            repository.Rollback();
        }
    }
}
