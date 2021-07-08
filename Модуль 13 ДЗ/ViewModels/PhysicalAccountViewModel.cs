using ModelLib;

namespace Модуль_13_ДЗ.ViewModels
{
    public class PhysicalAccountViewModel : BankAccountViewModel
    {
        public PhysicalAccountViewModel(Accounts bankAccount)
                    :base(bankAccount)
        { }        

        public override AccountType AccountType
        {
            get { return AccountType.PhysicalAccount; }
        }

        public override string Name
        {
            get { return $"Физический счет {OwnerName} Id {OwnerId}"; }
        }

        public override bool CanAdded => true;
        
        public override bool CanWithdrawed
        {
            get
            {
                if (CreatedDate.AddMonths(MinTerm) <= CurrentDate)
                    return true;

                return false;
            }
        }

        public override bool CanClose
        {
            get { return Amount == 0; }
        }
        
        public override bool CanTransact
        {
            get { return CanWithdrawed;  }
        }
    }
}

