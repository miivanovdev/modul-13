using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class AccountTypes : EntityBase
    {
        /// <summary>
        /// Доступно снятие со счета
        /// </summary>
        public virtual bool CanWithdrawed { get; set; }

        /// <summary>
        /// Доступно пополнение счета
        /// </summary>
        public virtual bool CanAdded { get; set; }

        /// <summary>
        /// Доступно закрытие счета
        /// </summary>
        public virtual bool CanClose { get; set; }
               
        /// <summary>
        /// Операция внесения на счет
        /// зависит от даты
        /// </summary>
        public bool AddingDependsOnMinTerm { get; set; }

        /// <summary>
        /// Операция снятия со счета 
        /// зависит от даты
        /// </summary>
        public bool WithdrawingDependsOnMinTerm { get; set; }

        /// <summary>
        /// Оперция закрытия счета 
        /// зависит от даты
        /// </summary>
        public bool ClosingDependsOnMinTerm { get; set; }

        /// <summary>
        /// Счет с капитализацией
        /// </summary>
        public bool IsCapitalized { get; set; }

        /// <summary>
        /// Департамент владелец типа счета 
        /// может пересмотреть условия счета
        /// (ставку, мин.срок и т.д.)
        /// </summary>
        public bool AllowedRevision { get; set; }

        /// <summary>
        /// Идентификатор департамента
        /// </summary>
        public int? DepartmentsRefId { get; set; }

        /// <summary>
        /// Департамент поставщик типа счета
        /// </summary>
        [ForeignKey("DepartmentsRefId")]
        public virtual Departments Departments { get; set; }

        /// <summary>
        /// Коллекция счетов данного типа
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accounts> Accounts { get; set; }
                
        /// <summary>
        /// Название типа счета
        /// </summary>
        public string Name { get; set; }
    }
}
