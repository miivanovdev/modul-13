using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Configuration;
using ModelLib;
using Модуль_13_ДЗ.DataServices;
using Модуль_13_ДЗ.Mediators;

namespace Модуль_13_ДЗ.ViewModels
{
    /// <summary>
    /// Модель представление всех счетов
    /// </summary>
    public class AllAccountsViewModel : ObservableObject, IAllAccountsViewModel
    {
        /// <summary>
        /// Конструктор обеспечивает загрузку данных из БД
        /// и обертку их в модель представление
        /// </summary>
        /// <param name="repository"></param>
        public AllAccountsViewModel(IAccountsService service, IAccountMediatorsFactory mediatorsFactory)
        {
            if (service == null || mediatorsFactory == null)
                throw new ArgumentNullException(service == null ? "service" : "mediatorsFactory");

            this.serivce = service;
            MediatorsFactory = mediatorsFactory;
            Accounts = serivce.GetList();
            MediatorsFactory.Accounts = Accounts;
        }

        /// <summary>
        /// Сервис счетов
        /// </summary>
        private readonly IAccountsService serivce;

        /// <summary>
        /// Фабрика посредников
        /// </summary>
        private IAccountMediatorsFactory MediatorsFactory { get; set; } 

        /// <summary>
        /// Коллекция счетов
        /// </summary>
        public List<AccountsViewModel> Accounts { get; set; }

        /// <summary>
        /// Отфильтрованное по департаменту и клиенту представление счетов
        /// </summary>
        public ObservableCollection<AccountsViewModel> AccountsView
        {
            get
            {
                return new ObservableCollection<AccountsViewModel>(Accounts.Where(x => x.OwnerId == SelectedClient.ClientId &&
                                                                                       x.DepartmentId == SelectedDepartment.DepartmentId));
            }
        }

        private AccountsViewModel selectedAccount;
        /// <summary>
        /// Выбранный счет
        /// </summary>
        public AccountsViewModel SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                if (selectedAccount == value)
                    return;

                selectedAccount = value;
                NotifyPropertyChanged(nameof(SelectedAccount));
            }
        }

        private DepartmentsViewModel selectedDepartment;
        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public DepartmentsViewModel SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                if (value == selectedDepartment)
                    return;

                selectedDepartment = value;

                NotifyPropertyChanged(nameof(AccountsView));
            }
        }

        private ClientsViewModel selectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public ClientsViewModel SelectedClient
        {
            get { return selectedClient; }
            set
            {
                if (value == selectedClient)
                    return;

                selectedClient = value;
                selectedClient.HaveAnAccounts = Accounts.Exists(x => x.OwnerId == selectedClient.ClientId);

                NotifyPropertyChanged(nameof(AccountsView));
            }
        }
                
        /// <summary>
        /// Метод реагирующий на смену клиента/департамента
        /// </summary>
        /// <param name="sender"></param>
        public void SelectionChange(object sender)
        {
            if(sender is ClientsViewModel)
            {
                var selectedClient = sender as ClientsViewModel;
                SelectedClient = selectedClient;
            }

            if (sender is DepartmentsViewModel)
            {
                var selectedDepartment = sender as DepartmentsViewModel;
                SelectedDepartment = selectedDepartment;
            }            
        }        

       
        private RelayCommand openAccountCommand;
        /// <summary>
        /// Команда открытия счета
        /// </summary>
        public RelayCommand OpenAccountCommand
        {
            get
            {
                return openAccountCommand ??
                (openAccountCommand = new RelayCommand(new Action<object>(OpenAccount),
                                                      new Func<object, bool>(AccountCanBeOpened)
                ));
            }
        }

        /// <summary>
        /// Метод проверки возможности вызова команды открытия счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeOpened(object o)
        {
            return SelectedDepartment != null &&
                   SelectedDepartment.DepartmentId > 1 &&
                   SelectedClient != null &&
                   !SelectedClient.BadHistory;

        }

        /// <summary>
        /// Метод открытия счета
        /// </summary>
        /// <param name="o"></param>
        private void OpenAccount(object o)
        {
            try
            {
                if (SelectedClient.AmountAvailable < SelectedDepartment.MinAmount)
                    throw new TransactionFailureException($"У клиента {SelectedClient.Name} недостаточно средств {SelectedClient.Amount}, минимальная сумма вклада {SelectedDepartment.MinAmount}");


                AccountsViewModel newAccount = serivce.Create(SelectedClient.ClientId,
                                                                SelectedClient.Name,
                                                                SelectedClient.Amount,
                                                                SelectedDepartment.InterestRate,
                                                                SelectedDepartment.DepartmentId,
                                                                SelectedDepartment.MinTerm,
                                                                SelectedDepartment.DefaultAccountType);
                
                SelectedClient.Amount = 0;
                
                NotifyPropertyChanged(nameof(AccountsView));
                
                accountChangedEvent?.Invoke(this, $"Открыт счет {newAccount.OwnerName} {newAccount.OwnerId}");
            }
            catch (TransactionFailureException ex)
            {
                MessageBox.Show(ex.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private RelayCommand withdrawCommand;
        /// <summary>
        /// Команда для списания средств со счета
        /// </summary>
        public RelayCommand WithdrawCommand
        {
            get
            {
                return withdrawCommand ??
                (withdrawCommand = new RelayCommand(new Action<object>(Withdraw), new Func<object, bool>(AccountCanBeWithdrawed)));
            }
        }

        /// <summary>
        /// Метод проверки возможности снятия средств
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeWithdrawed(object o)
        {
            return SelectedClient != null &&
                   SelectedAccount != null &&
                   SelectedAccount.CanWithdrawed;
        }

        /// <summary>
        /// Вызов снятия со счета
        /// </summary>
        /// <param name="sender"></param>
        private void Withdraw(object sender)
        {
            try
            {
                var mediator = MediatorsFactory.Create(nameof(Withdraw), SelectedAccount, SelectedClient);
                mediator.Transaction();
                serivce.Update(SelectedAccount);
                accountChangedEvent?.Invoke(this, mediator.Log);
            }
            catch (TransactionFailureException ex)
            {
                MessageBox.Show(ex.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private RelayCommand depositCommand;
        /// <summary>
        /// Команда внесения средств на счет
        /// </summary>
        public RelayCommand DepositCommand
        {
            get
            {
                return depositCommand ??
                (depositCommand = new RelayCommand(new Action<object>(Deposit), new Func<object, bool>(AccountCanBeAdd)));
            }
        }

        /// <summary>
        /// Метод проверки возможности добавления средств
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeAdd(object o)
        {
            return SelectedClient != null &&
                   SelectedAccount != null &&
                   SelectedAccount.CanAdded;
        }

        /// <summary>
        /// Вызов поплнения счета
        /// </summary>
        /// <param name="sender"></param>
        private void Deposit(object sender)
        {
            try
            {
                var mediator = MediatorsFactory.Create(nameof(Deposit), SelectedAccount, SelectedClient);
                mediator.Transaction();
                serivce.Update(SelectedAccount);
                accountChangedEvent?.Invoke(this, mediator.Log);
            }
            catch (TransactionFailureException ex)
            {
                MessageBox.Show(ex.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }

        
        private RelayCommand transactCommand;
        /// <summary>
        /// Команда вызова транзакции
        /// </summary>
        public RelayCommand TransactCommand
        {
            get
            {
                return transactCommand ??
                (transactCommand = new RelayCommand(new Action<object>(Transact), new Func<object, bool>(CanTransact)));
            }
        }

        private bool CanTransact(object o)
        {
            return SelectedAccount != null && 
                   SelectedAccount.CanWithdrawed;
        }

        /// <summary>
        /// Вызвать перевод
        /// </summary>
        /// <param name="sender"></param>
        private void Transact(object sender)
        {
            var mediator = MediatorsFactory.Create(nameof(Transact), SelectedAccount);

            try
            {    
                mediator.Transaction();
                serivce.UpdateRange(new AccountsViewModel[]{ SelectedAccount, (AccountsViewModel)mediator.GetReciever() });
                accountChangedEvent?.Invoke(this, mediator.Log);
            }        
            catch (Exception ex)
            {
                serivce.Rollback();
                MessageBox.Show(ex.Message);
            }        
        }

        
        private RelayCommand closeAccountCommand;
        /// <summary>
        /// Команда для закрытия счета
        /// </summary>
        public RelayCommand CloseAccountCommand
        {
            get
            {
                return closeAccountCommand ??
                (closeAccountCommand = new RelayCommand(new Action<object>(CloseAccount), new Func<object, bool>(AccountCanBeClosed)));
            }
        }

        /// <summary>
        /// Метод закрытия счета
        /// </summary>
        /// <param name="o"></param>
        private void CloseAccount(object o)
        {
            try
            {
                serivce.Delete(SelectedAccount.AccountId);

                if (!Accounts.Remove(SelectedAccount))
                    throw new Exception($"Не удалось удалить {SelectedAccount.Name}");

                accountChangedEvent?.Invoke(this, $"Закрыт счет {SelectedAccount.OwnerName} {SelectedAccount.OwnerId}");
                NotifyPropertyChanged(nameof(AccountsView));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Проверка на возможность закрытия счета
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool AccountCanBeClosed(object o)
        {
            return SelectedAccount != null &&
                   SelectedDepartment != null &&
                   SelectedAccount.CanClose;
        }

        private event Action<object, string> accountChangedEvent;
        /// <summary>
        /// Событие изменения состояния счета
        /// </summary>
        public event Action<object, string> AccountChangedEvent
        {
            add { accountChangedEvent += value; }
            remove { accountChangedEvent -= value; }
        }
    }
}
