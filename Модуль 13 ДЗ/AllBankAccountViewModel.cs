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
    public class AllBankAccountViewModel : ObservableObject
    {
        public AllBankAccountViewModel(IRepository<BankAccount> repository)
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

        private readonly IRepository<BankAccount> Repository;

        public List<BankAccountViewModel> Accounts { get; set; }

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

        public ObservableCollection<LogMessage> Log { get; set; }

        private BankDepartmentViewModel selectedDepartment;
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

        public void WrapIntoViewModel(IEnumerable<BankAccount> list)
        {
            Accounts = new List<BankAccountViewModel>();

            foreach (var l in list)
                Accounts.Add(new BankAccountViewModel(l));
        }


        public BankAccountViewModel WrapOne(BankAccount bankAccount)
        {
            BankAccountViewModel accountViewModel = null;

            switch (bankAccount.AccountType)
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

        /// <summary>
        /// Команда открытия счета
        /// </summary>
        private RelayCommand openAccountCommand;

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

                BankAccount newAccount = newAccount = new BankAccount(SelectedClient.ClientId,
                                                                      SelectedClient.Name,
                                                                      SelectedClient.Amount,
                                                                      SelectedDepartment.InterestRate,
                                                                      SelectedDepartment.DepartmentId,
                                                                      SelectedDepartment.MinTerm,
                                                                      SelectedDepartment.Delay,
                                                                      SelectedDepartment.AccountType);
                Repository.Create(newAccount);

                SelectedClient.Amount = 0;
                Accounts.Add(WrapOne(newAccount));
                NotifyPropertyChanged(nameof(AccountsView));
                //Log.Add(new LogMessage($"Открыт счет {newAccount.OwnerName} {newAccount.OwnerId} Type:{newAccount.AccountType}"));
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

        /// <summary>
        /// Команда для списания средств со счета
        /// </summary>
        private RelayCommand withdrawCommand;

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

        /// <summary>
        /// Команда внесения средств на счет
        /// </summary>
        private RelayCommand addCommand;

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
            return SelectedAccount != null && SelectedAccount.CanTransact;
        }

        /// <summary>
        /// Вызвать перевод
        /// </summary>
        /// <param name="sender"></param>
        private void Transact(object sender)
        {
            try
            {
                var mediator = new AccountToAccountMediator(Accounts.Where(x => x != SelectedAccount).ToList(), SelectedAccount);
                mediator.Transaction();
                Repository.Update(SelectedAccount.BankAccount);
                Repository.Update(mediator.RecieverAccount.BankAccount);
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

        /// <summary>
        /// Команда для списания средств со счета
        /// </summary>
        private RelayCommand closeAccountCommand;

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

                NotifyPropertyChanged(nameof(AccountsView));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                

            //Log.Add(new LogMessage($"Закрыт счет {SelectedAccount.OwnerName} {SelectedAccount.OwnerId} Type:{SelectedAccount.AccountType}"));
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
        
        protected virtual void LogAdding(object sender, decimal amount)
        {
            var account = sender as ITransactable;
            Log.Add(new LogMessage($"{account.Name} пополнен на сумму: {amount}"));
        }

        protected virtual void LogWithdrawing(object sender, decimal amount)
        {
            var account = sender as ITransactable;
            Log.Add(new LogMessage($"{account.Name} снята сумма: {amount}"));
        }

        protected virtual void LogTransact(object sender, ITransactable accountReciever, decimal amount)
        {
            var accountSender = sender as ITransactable;
            Log.Add(new LogMessage($"{accountSender.Name} перевод на счет {accountReciever.Name} на сумму: {amount}"));
        }
    }
}
