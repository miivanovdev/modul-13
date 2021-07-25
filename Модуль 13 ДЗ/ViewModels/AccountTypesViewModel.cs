using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    public class AccountTypesViewModel : ObservableObject, IDataErrorInfo, IEditableObject
    {
        public AccountTypesViewModel(AccountTypes accountTypes)
        {
            AccountTypes = accountTypes;
        }

        public AccountTypes AccountTypes { get; set; }

        public int AccountTypesId
        {
            get { return AccountTypes.Id; }
            set
            {
                if (value == AccountTypes.Id)
                    return;

                AccountTypes.Id = value;

                NotifyPropertyChanged(nameof(AccountTypesId));
            }
        }

        public string Name
        {
            get { return AccountTypes.Name; }
            set
            {
                if (value == AccountTypes.Name)
                    return;

                AccountTypes.Name = value;
                
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public bool CanAdded
        {
            get { return AccountTypes.CanAdded; }
            set
            {
                if (value == AccountTypes.CanAdded)
                    return;

                AccountTypes.CanAdded = value;

                if (value == false)
                    AddingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanAdded));
            }
        }

        public bool CanWithdrawed
        {
            get { return AccountTypes.CanWithdrawed; }
            set
            {
                if (value == AccountTypes.CanWithdrawed)
                    return;

                AccountTypes.CanWithdrawed = value;

                if (value == false)
                    WithdrawingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanWithdrawed));
            }
        }

        public bool CanClose
        {
            get { return AccountTypes.CanClose; }
            set
            {
                if (value == AccountTypes.CanClose)
                    return;

                AccountTypes.CanClose = value;

                if (value == false)
                    ClosingDependsOnMinTerm = false;

                NotifyPropertyChanged(nameof(CanClose));
            }
        }

        public bool AddingDependsOnMinTerm
        {
            get { return AccountTypes.AddingDependsOnMinTerm; }
            set
            {
                if (value == AccountTypes.AddingDependsOnMinTerm)
                    return;                    

                AccountTypes.AddingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(AddingDependsOnMinTerm));
            }
        }

        public bool WithdrawingDependsOnMinTerm
        {
            get { return AccountTypes.WithdrawingDependsOnMinTerm; }
            set
            {
                if (value == AccountTypes.WithdrawingDependsOnMinTerm)
                    return;

                AccountTypes.WithdrawingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(WithdrawingDependsOnMinTerm));
            }
        }

        public bool ClosingDependsOnMinTerm
        {
            get { return AccountTypes.ClosingDependsOnMinTerm; }
            set
            {
                if (value == AccountTypes.ClosingDependsOnMinTerm)
                    return;

                AccountTypes.ClosingDependsOnMinTerm = value;

                NotifyPropertyChanged(nameof(ClosingDependsOnMinTerm));
            }
        }

        public bool IsCapitalized
        {
            get { return AccountTypes.IsCapitalized; }
            set
            {
                if (value == AccountTypes.IsCapitalized)
                    return;

                AccountTypes.IsCapitalized = value;

                NotifyPropertyChanged(nameof(IsCapitalized));
            }
        }

        public bool AllowedRevision
        {
            get { return AccountTypes.AllowedRevision; }
            set
            {
                if (value == AccountTypes.AllowedRevision)
                    return;

                AccountTypes.AllowedRevision = value;

                NotifyPropertyChanged(nameof(AllowedRevision));
            }
        }

        public int? DepartmentsRefId
        {
            get { return AccountTypes.DepartmentsRefId; }
            set
            {
                if (value == AccountTypes.DepartmentsRefId)
                    return;

                AccountTypes.DepartmentsRefId = value;

                NotifyPropertyChanged(nameof(DepartmentsRefId));
            }
        }

        public bool HasChanges
        {
            get
            {
                if (OriginAccountTypes == null)
                    return false;

                if( this.Name                           != OriginAccountTypes.Name                          ||
                    this.CanAdded                       != OriginAccountTypes.CanAdded                      ||
                    this.CanClose                       != OriginAccountTypes.CanClose                      ||
                    this.CanWithdrawed                  != OriginAccountTypes.CanWithdrawed                 ||
                    this.AddingDependsOnMinTerm         != OriginAccountTypes.AddingDependsOnMinTerm        ||
                    this.ClosingDependsOnMinTerm        != OriginAccountTypes.ClosingDependsOnMinTerm       ||
                    this.WithdrawingDependsOnMinTerm    != OriginAccountTypes.WithdrawingDependsOnMinTerm   ||
                    this.IsCapitalized                  != OriginAccountTypes.IsCapitalized                 ||
                    this.DepartmentsRefId               != OriginAccountTypes.DepartmentsRefId)
                    return true;

                return false;
            }
        }       
                
        public string this[string columnName]
        {
            get
            {
                Error = String.Empty;
                switch (columnName)
                {
                    case nameof(Name):

                        if (Name == string.Empty)
                        {
                            Error = "Введите имя!";
                        }
                        else
                        {
                            if (Name.Length > 25)
                                Error = "Слишком длинное имя!";
                        }

                        break;
                }

                return Error;
            }
        }

        public string Error { get; set; }

        private AccountTypes OriginAccountTypes { get; set; }

        public void BeginEdit()
        {
            if (OriginAccountTypes == null)
                OriginAccountTypes = new AccountTypes()
                {
                    Name                            = this.Name,
                    CanAdded                        = this.CanAdded,
                    CanClose                        = this.CanClose,
                    CanWithdrawed                   = this.CanWithdrawed,
                    AddingDependsOnMinTerm          = this.AddingDependsOnMinTerm,
                    ClosingDependsOnMinTerm         = this.ClosingDependsOnMinTerm,
                    WithdrawingDependsOnMinTerm     = this.WithdrawingDependsOnMinTerm,
                    IsCapitalized                   = this.IsCapitalized,
                    DepartmentsRefId                = this.AccountTypes.DepartmentsRefId
                };
        }

        public void CancelEdit()
        {
            if (!HasChanges)
                return;

            this.Name                           = OriginAccountTypes.Name;
            this.CanAdded                       = OriginAccountTypes.CanAdded;
            this.CanClose                       = OriginAccountTypes.CanClose;
            this.CanWithdrawed                  = OriginAccountTypes.CanWithdrawed;
            this.AddingDependsOnMinTerm         = OriginAccountTypes.AddingDependsOnMinTerm;
            this.ClosingDependsOnMinTerm        = OriginAccountTypes.ClosingDependsOnMinTerm;
            this.WithdrawingDependsOnMinTerm    = OriginAccountTypes.WithdrawingDependsOnMinTerm;
            this.IsCapitalized                  = OriginAccountTypes.IsCapitalized;
            this.AccountTypes.DepartmentsRefId  = OriginAccountTypes.DepartmentsRefId;
        }

        public void EndEdit()
        {
            OriginAccountTypes = null;
        }
    }
}
