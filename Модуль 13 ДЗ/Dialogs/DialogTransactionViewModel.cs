using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ModelLib;
using Модуль_13_ДЗ.ViewModels;
using Модуль_13_ДЗ.Mediators;

namespace Модуль_13_ДЗ.Dialogs
{
    public class DialogTransactionViewModel : ObservableObject
    {
        public DialogTransactionViewModel(IEnumerable<DepartmentsViewModel> departments, IEnumerable<AccountsViewModel> accounts, AccountsViewModel sender)
        {
            if (accounts == null || departments == null || sender.AmountAvailable == 0)
                throw new TransactionFailureException("Неверно переданы счета или отсутствует счет отправитель!");

            this.Departments = departments;
            SelectedDepartment = departments.First();
            this.Accounts = accounts;
            Data = new DialogAccountToClientModel(sender.AmountAvailable, true);
            SenderAccount = sender;
        }
        /// <summary>
        /// Счет отправителя
        /// </summary>
        public AccountsViewModel SenderAccount { get; set; }

        /// <summary>
        /// Сумма перевода
        /// </summary>
        public decimal Amount { get { return Data.Amount; } }

        /// <summary>
        /// Модель представления ввода
        /// </summary>
        public DialogAccountToClientModel Data { get; set; }

        /// <summary>
        /// Все счета
        /// </summary>
        private IEnumerable<AccountsViewModel> Accounts;

        public IEnumerable<DepartmentsViewModel> Departments { get; private set; }

        /// <summary>
        /// Отфильтрованные счета
        /// </summary>
        public ReadOnlyCollection<AccountsViewModel> AccountViews
        {
            get { return new ReadOnlyCollection<AccountsViewModel>(GetSubset()); }
        }

        /// <summary>
        /// Фильтр по департаменту
        /// </summary>
        private DepartmentsViewModel selectedDepartment;
        public DepartmentsViewModel SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                if (selectedDepartment != value)
                {
                    selectedDepartment = value;
                    NotifyPropertyChanged(nameof(SelectedDepartment));
                    NotifyPropertyChanged(nameof(AccountViews));
                }
            }
        }

        /// <summary>
        /// Отфильтровывает список
        /// по департаменту исключая
        /// счет отправитель
        /// </summary>
        /// <returns></returns>
        private List<AccountsViewModel> GetSubset()
        {
            return Accounts.Where(x => x.DepartmentId == SelectedDepartment.DepartmentId && x != SenderAccount).ToList();
        }

        /// <summary>
        /// Результат диалога
        /// </summary>
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult == value)
                    return;

                dialogResult = value;
                NotifyPropertyChanged(nameof(DialogResult));
            }
        }

        private AccountsViewModel recieverAccount;
        /// <summary>
        /// Выбранный счет получатель
        /// </summary>
        public AccountsViewModel RecieverAccount
        {
            get
            {
                return recieverAccount;
            }
            set
            {
                if (recieverAccount == value)
                    return;

                recieverAccount = value;
                Data.TotalAmount = recieverAccount.Amount;
                NotifyPropertyChanged(nameof(RecieverAccount));
            }
        }

        private RelayCommand okCommand;
        /// <summary>
        /// Команда закрытия при положительном результате диалога
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return okCommand ??
                (okCommand = new RelayCommand(new Action<object>(OkClose),
                                              new Func<object, bool>(CanClose)
                ));
            }
        }

        /// <summary>
        /// Положительное закрытие диалога
        /// </summary>
        /// <param name="o"></param>
        private void OkClose(object o)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Проверка допустимости положительного исхода диалога
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool CanClose(object o)
        {
            return RecieverAccount != null &&
                   Data.IsValid;
        }
    }
}
