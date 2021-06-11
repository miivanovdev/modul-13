using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class AllClientViewModel : ObservableObject
    {
        public AllClientViewModel(IRepository<Client> repository)
        {
            Repository = repository;

            try
            {
                WrapIntoViewModel(Repository.GetList());
                SelectedClient = Clients.First();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        public ObservableCollection<ClientViewModel> Clients { get; set; }

        private readonly IRepository<Client> Repository;

        private event Action<object> onClientSelectionChange;
        /// <summary>
        /// Событие смены выбранного клиента
        /// </summary>
        public event Action<object> OnClientSelectionChange
        {
            add
            {
                onClientSelectionChange += value;
            }
            remove { onClientSelectionChange -= value; }
        }

        private ClientViewModel selectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public ClientViewModel SelectedClient
        {
            get
            {
                return selectedClient;
            }
            set
            {
                if (selectedClient == value)
                    return;

                selectedClient = value;
                NotifyPropertyChanged(nameof(SelectedClient));
                onClientSelectionChange?.Invoke(selectedClient);
            }
        }

        public void WrapIntoViewModel(IEnumerable<Client> list)
        {
            Clients = new ObservableCollection<ClientViewModel>();

            foreach (var l in list)
                Clients.Add(new ClientViewModel(l));
        }

        
        private RelayCommand addClientCommand;

        /// <summary>
        /// Команда добавления клиента
        /// </summary>
        public RelayCommand AddClientCommand
        {
            get
            {
                return addClientCommand ??
                (addClientCommand = new RelayCommand(new Action<object>(AddClient),
                                                     new Func<object, bool>(ClientCanBeAdded)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности вызова команды добавления клиента
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool ClientCanBeAdded(object o)
        {
            return true;
        }

        /// <summary>
        /// Метод добавления нового клиента
        /// </summary>
        /// <param name="o"></param>
        private void AddClient(object o)
        {            
            try
            {
                ClientInputViewModel inputViewModel = new ClientInputViewModel();
                ClientInputDialog inputDialog = new ClientInputDialog(inputViewModel);

                if (inputDialog.ShowDialog() == true)
                {
                    Repository.Create(inputViewModel.Client.Client);
                    Clients.Add(inputViewModel.Client);

                    NotifyPropertyChanged(nameof(Clients));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                
            
        }

        
        private RelayCommand removeClientCommand;
        /// <summary>
        /// Команда удаления клиента
        /// </summary>
        public RelayCommand RemoveClientCommand
        {
            get
            {
                return removeClientCommand ??
                (removeClientCommand = new RelayCommand(new Action<object>(RemoveClient),
                                                     new Func<object, bool>(CanClientBeRemove)
                ));
            }
        }

        /// <summary>
        /// Проверка возможности удаления клиента
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClientBeRemove(object o)
        {
            return SelectedClient != null &&
                   !SelectedClient.HaveAnAccounts;
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="o"></param>
        private void RemoveClient(object o)
        {
            try
            {
                Repository.Delete(SelectedClient.ClientId);

                if (!Clients.Remove(SelectedClient))
                    throw new Exception("Не удалось удалить клиента!");

                if (Clients.Count > 0)
                    SelectedClient = Clients.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
    }
}
