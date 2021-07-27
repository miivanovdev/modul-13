using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    /// <summary>
    /// Сервис клиентов
    /// </summary>
    public class ClientsService : IClientsService
    {
        public ClientsService(IRepository<Clients> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Интерфейс взаймодействия с хранилищем
        /// </summary>
        private readonly IRepository<Clients> repository;
                
        /// <summary>
        /// Создать клиента
        /// </summary>
        /// <param name="clientsView"></param>
        public void Create(ClientsViewModel clientsView)
        {
            repository.Create(clientsView.Clients);
        }

        /// <summary>
        /// Удалить клиента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            repository.Delete(id);
        }

        /// <summary>
        /// Получить список клиентов
        /// </summary>
        /// <returns></returns>
        public List<ClientsViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        /// <summary>
        /// Получить клиента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientsViewModel GetOne(int id)
        {
            return WrapOne(repository.GetOne(id));
        }

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        public void Rollback()
        {
            repository.Rollback();
        }

        /// <summary>
        /// Обновить клиента
        /// </summary>
        /// <param name="item"></param>
        public void Update(ClientsViewModel item)
        {
            repository.Update(item.Clients);
        }

        /// <summary>
        /// Обновить несколько клиентов
        /// </summary>
        /// <param name="items"></param>
        public void UpdateRange(ClientsViewModel[] items)
        {
            List<Clients> itemsModel = new List<Clients>();

            foreach (var i in items)
                itemsModel.Add(i.Clients);

            repository.UpdateRange(itemsModel.ToArray());
        }

        /// <summary>
        /// Обернуть модель в модель-представление
        /// </summary>
        /// <param name="clients"></param>
        /// <returns></returns>
        private ClientsViewModel WrapOne(Clients clients)
        {
            return new ClientsViewModel(clients);
        }

        /// <summary>
        /// Обернуть список моделей в модели-представления
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ClientsViewModel> WrapIntoViewModel(IEnumerable<Clients> list)
        {
            List<ClientsViewModel> clients = new List<ClientsViewModel>();

            foreach (var l in list)
                clients.Add(WrapOne(l));

            return clients;
        }
    }
}
