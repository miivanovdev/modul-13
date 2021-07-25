using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Модуль_13_ДЗ.ViewModels;
using ModelLib;

namespace Модуль_13_ДЗ.DataServices
{
    public class ClientsService : IClientsService
    {
        public ClientsService(IRepository<Clients> repository)
        {
            this.repository = repository;
        }

        private readonly IRepository<Clients> repository;
                
        public void Create(ClientsViewModel clientsView)
        {
            repository.Create(clientsView.Clients);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }

        public List<ClientsViewModel> GetList()
        {
            return WrapIntoViewModel(repository.GetList());
        }

        public ClientsViewModel GetOne(int id)
        {
            return WrapOne(repository.GetOne(id));
        }

        public void Rollback()
        {
            repository.Rollback();
        }

        public void Update(ClientsViewModel item)
        {
            repository.Update(item.Clients);
        }

        public void UpdateRange(ClientsViewModel[] items)
        {
            List<Clients> itemsModel = new List<Clients>();

            foreach (var i in items)
                itemsModel.Add(i.Clients);

            repository.UpdateRange(itemsModel.ToArray());
        }

        private ClientsViewModel WrapOne(Clients clients)
        {
            return new ClientsViewModel(clients);
        }

        public List<ClientsViewModel> WrapIntoViewModel(IEnumerable<Clients> list)
        {
            List<ClientsViewModel> clients = new List<ClientsViewModel>();

            foreach (var l in list)
                clients.Add(WrapOne(l));

            return clients;
        }
    }
}
