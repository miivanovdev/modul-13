using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ModelLib;
using Модуль_13_ДЗ.DataServices;
using Модуль_13_ДЗ.Dialogs;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление всех клиентов
    /// </summary>
    public class AllClientsViewModel : ObservableObject, IAllClientsViewModel
    {
        /// <summary>
        /// Конструктор обеспечивающий загрузгу 
        /// </summary>
        /// <param name="repository"></param>
        public AllClientsViewModel(IClientsService service)
        {
            this.service = service;
            Clients = service.GetList();
            SelectedClient = Clients.First();
        }

        /// <summary>
        /// Коллекция моделей представлений клиентов
        /// </summary>
        public IList<ClientsViewModel> Clients { get; set; }

        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IClientsService service;

        private event Action<object> onClientSelectionChange;
        /// <summary>
        /// Событие смены выбранного клиента
        /// </summary>
        public event Action<object> OnClientSelectionChange
        {
            add { onClientSelectionChange += value; }
            remove { onClientSelectionChange -= value; }
        }

        private ClientsViewModel selectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public ClientsViewModel SelectedClient
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
                    service.Create(inputViewModel.Client);
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
                service.Delete(SelectedClient.ClientId);

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

        private RelayCommand updateClientBeginCommand;
        /// <summary>
        /// Команда при входе в режима редактирования
        /// </summary>
        public RelayCommand UpdateClientBeginCommand
        {
            get
            {
                return updateClientBeginCommand ??
                (updateClientBeginCommand = new RelayCommand(new Action<object>(RowEditBegin)));
            }
        }

        public void RowEditBegin(object args)
        {
            SelectedClient.BeginEdit();
        }

        private RelayCommand updateClientCommitCommand;
        /// <summary>
        /// Команда при выходе из режима редактирования
        /// </summary>
        public RelayCommand UpdateClientCommitCommand
        {
            get
            {
                return updateClientCommitCommand ??
                (updateClientCommitCommand = new RelayCommand(new Action<object>(RowEditEnding)));
            }
        }
        
        /// <summary>
        /// Метод отвечающий за сохранение изменений 
        /// данных клиента, если такие имеются
        /// </summary>
        /// <param name="args"></param>
        private void RowEditEnding(object args)
        {
            if (SelectedClient.HasChanges)
            {
                var result = MessageBox.Show(App.Current.MainWindow, "Вы хотите сохранить изменения?", $"Изменения клиента {SelectedClient.Name}", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        service.Update(SelectedClient);
                        SelectedClient.EndEdit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        SelectedClient.CancelEdit();
                    }
                }
                else
                {
                    SelectedClient.CancelEdit();
                }
            }
        }
    }
}
