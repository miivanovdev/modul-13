using ModelLib;

namespace Модуль_13_ДЗ
{
    public class IndividualAccountViewModel : BankAccountViewModel
    {
        public IndividualAccountViewModel(BankAccount bankAccount)
            : base(bankAccount)
        {
            
        }

        public override AccountType AccountType
        {
            get { return AccountType.IndividualAccount; }
        }

        public override string Name
        {
            get { return $"Индивидуальный счет {OwnerName} Id {OwnerId}"; }
        }
               
        public override bool CanAdded
        {
            get
            {
                if (CreatedDate.AddMonths(Delay) < CurrentDate)
                    return true;

                return false;
            }
        }

        public override bool CanWithdrawed
        {
            get
            {
                if (CurrentDate > CreatedDate.AddMonths(MinTerm - Delay))
                    return true;

                return false;
            }
        }

        public override bool CanTransact
        {
            get { return CanWithdrawed; }
        }

        public override bool CanClose => CanWithdrawed;
    }
}
