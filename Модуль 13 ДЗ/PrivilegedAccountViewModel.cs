using ModelLib;

namespace Модуль_13_ДЗ
{
    public class PrivilegedAccountViewModel : BankAccountViewModel
    {
        public PrivilegedAccountViewModel(Accounts bankAccount)
            : base(bankAccount)
        {

        }

        public override AccountType AccountType
        {
            get { return AccountType.PrivilegedAccount; }
        }

        public override string Name
        {
            get { return $"Привелигированный счет {OwnerName} Id {OwnerId}"; }
        }

        protected override decimal CountIncome()
        {
            if(MonthPassed >= 0)
                return Capitalized(Amount, MonthPassed);

            return 0;
        }

        private decimal Capitalized(decimal amount, int month)
        {
            if (month == 0)
                return amount;

            return Capitalized(amount + (amount * InterestRate) / 100, --month);
        }

        public override bool CanWithdrawed => true;
        public override bool CanAdded => true;
        public override bool CanTransact => true;
        public override bool CanClose => true;
    }
}
