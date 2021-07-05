using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Configuration;
using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Модель представление всех счетов
    /// </summary>
    public class AllBankAccountViewModel : ObservableObject
    {
        /// <summary>
        /// Конструктор обеспечивает загрузку данных из БД
        /// и обертку их в модель представление
        /// </summary>
        /// <param name="repository"></param>
        public AllBankAccountViewModel(IRepository<Accounts> repository)
        {
            try
            {
                Repository = repository;
                WrapIntoViewModel(Repository.GetList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Репозиторий счетов
        /// </summary>
        private readonly IRepository<Accounts> Repository;

        /// <summary>
        /// Коллекция счетов
        /// </summary>
        public List<BankAccountViewModel> Accounts { get; set; }

        /// <summary>
        /// Отфильтрованное по департаменту и клиенту представление счетов
        /// </summary>
        public ObservableCollection<BankAccountViewModel> AccountsView
        {
            get
            {
                return new ObservableCollection<BankAccountViewModel>(Accounts.Where(x => x.OwnerId == SelectedClient.ClientId &&
                                                                                          x.DepartmentId == SelectedDepartment.DepartmentId));
            }
        }

        private BankAccountViewModel selectedAccount;
        /// <summary>
        /// Выбранный счет
        /// </summary>
        public BankAccountViewModel SelectedAccount
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

        private BankDepartmentViewModel selectedDepartment;
        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public BankDepartmentViewModel SelectedDepartment
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

        private ClientViewModel selectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public ClientViewModel SelectedClient
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
        /// Метод оборачивающий коллекцию моделей счетов 
        /// в их модель представление
        /// </summary>
        /// <param name="list"></param>
        public void WrapIntoViewModel(IEnumerable<Accounts> list)
        {
            Accounts = new List<BankAccountViewModel>();

            foreach (var l in list)
                Accounts.Add(WrapOne(l));
        }

        /// <summary>
        /// Метод оборачивающий модель в
        /// соответствующую модель представление
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public BankAccountViewModel WrapOne(Accounts bankAccount)
        {
            BankAccountViewModel accountViewModel = null;

            switch (bankAccount.Type)
            {
                case AccountType.Basic:
                     accountViewModel = new BankAccountViewModel(bankAccount);
                    break;

                case AccountType.PhysicalAccount:
                    accountViewModel = new PhysicalAccountViewModel(bankAccount);
                    break;

                case AccountType.IndividualAccount:
                    accountViewModel = new IndividualAccountViewModel(bankAccount);
                    break;

                case AccountType.PrivilegedAccount:
                    accountViewModel = new PrivilegedAccountViewModel(bankAccount);
                    break;

                default:
                    throw new Exception("Невозможно создать модель представление! Не предусмотренный тип счета!");                    
            }

            return accountViewModel;
        }

        /// <summary>
        /// Метод реагирующий на смену клиента/департамента
        /// </summary>
        /// <param name="sender"></param>
        public void SelectionChange(object sender)
        {
            if(sender is ClientViewModel)
            {
                var selectedClient = sender as ClientViewModel;
                SelectedClient = selectedClient;
            }

            if (sender is BankDepartmentViewModel)
            {
                var selectedDepartment = sender as BankDepartmentViewModel;
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

                Accounts newAccount = newAccount = new Accounts(SelectedClient.ClientId,
                                                                      SelectedClient.Name,
                                                                      SelectedClient.Amount,
                                                                      SelectedDepartment.InterestRate,
                                                                      SelectedDepartment.DepartmentId,
                                                                      SelectedDepartment.MinTerm,
                                                                      SelectedDepartment.Delay,
                                                                      (int)SelectedDepartment.AccountType);
                Repository.Create(newAccount);

                SelectedClient.Amount = 0;
                Accounts.Add(WrapOne(newAccount));
                NotifyPropertyChanged(nameof(AccountsView));
                
                accountChangedEvent?.Invoke(this, new Log($"Открыт счет {newAccount.OwnerName} {newAccount.OwnerId} Type:{newAccount.AccountType}"));
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
                var mediator = new AccountToClientMediator(SelectedClient, SelectedAccount, true);
                mediator.Transaction();
                Repository.Update(SelectedAccount.BankAccount);
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

        private RelayCommand addCommand;
        /// <summary>
        /// Команда внесения средств на счет
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new RelayCommand(new Action<object>(Add), new Func<object, bool>(AccountCanBeAdd)));
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
        private void Add(object sender)
        {
            try
            {
                var mediator = new AccountToClientMediator(SelectedClient, SelectedAccount);
                mediator.Transaction();
                Repository.Update(SelectedAccount.BankAccount);
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
                   SelectedAccount.CanTransact;
        }

        /// <summary>
        /// Вызвать перевод
        /// </summary>
        /// <param name="sender"></param>
        private void Transact(object sender)
        {
            var mediator = new AccountToAccountMediator(Accounts.Where(x => x != SelectedAccount).ToList(), SelectedAccount);

            try
            {    
                mediator.Transaction();
                Repository.UpdateRange(new Accounts[]{ SelectedAccount.BankAccount, mediator.RecieverAccount.BankAccount });
                accountChangedEvent?.Invoke(this, mediator.Log);
            }        
            catch (Exception ex)
            {
                Repository.Rollback();
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
                Repository.Delete(SelectedAccount.AccountId);

                if (!Accounts.Remove(SelectedAccount))
                    throw new Exception($"Не удалось удалить {SelectedAccount.Name}");

                accountChangedEvent?.Invoke(this, new Log($"Закрыт счет {SelectedAccount.OwnerName} {SelectedAccount.OwnerId} Type:{SelectedAccount.AccountType}"));
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

        private event Action<object, Log> accountChangedEvent;
        /// <summary>
        /// Событие изменения состояния счета
        /// </summary>
        public event Action<object, Log> AccountChangedEvent
        {
            add { accountChangedEvent += value; }
            remove { accountChangedEvent -= value; }
        }
    }
}
