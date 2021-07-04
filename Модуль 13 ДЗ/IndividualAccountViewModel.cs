using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Модель представления индивидуальных счетов
    /// </summary>
    public class IndividualAccountViewModel : BankAccountViewModel
    {
        public IndividualAccountViewModel(Accounts bankAccount)
            : base(bankAccount)
        {
            
        }

        /// <summary>
        /// Свойство указывающая на тип счета
        /// </summary>
        public override AccountType AccountType
        {
            get { return AccountType.IndividualAccount; }
        }

        /// <summary>
        /// Наименование счета
        /// </summary>
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
