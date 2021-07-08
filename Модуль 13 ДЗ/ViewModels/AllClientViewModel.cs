using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление всех клиентов
    /// </summary>
    public class AllClientViewModel : ObservableObject
    {
        /// <summary>
        /// Конструктор обеспечивающий загрузгу 
        /// </summary>
        /// <param name="repository"></param>
        public AllClientViewModel(IRepository<Clients> repository)
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

        /// <summary>
        /// Коллекция моделей представлений клиентов
        /// </summary>
        public ObservableCollection<ClientViewModel> Clients { get; set; }

        /// <summary>
        /// Репозиторий клиентов
        /// </summary>
        private readonly IRepository<Clients> Repository;

        private event Action<object> onClientSelectionChange;
        /// <summary>
        /// Событие смены выбранного клиента
        /// </summary>
        public event Action<object> OnClientSelectionChange
        {
            add { onClientSelectionChange += value; }
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

        /// <summary>
        /// Метод оборачивает коллекцию моделей клиентов
        /// в модель представление
        /// </summary>
        /// <param name="list"></param>
        public void WrapIntoViewModel(IEnumerable<Clients> list)
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
                        Repository.Update(SelectedClient.Client);
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
