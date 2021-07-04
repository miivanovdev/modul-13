namespace Модуль_13_ДЗ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using ModelLib;

    public partial class Departments : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departments()
        {
            Accounts = new HashSet<Accounts>();
        }
                
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        public decimal MinAmount { get; set; }

        public int Delay { get; set; }

        public int MinTerm { get; set; }

        [Column(TypeName = "money")]
        public decimal InterestRate { get; set; }

        public int AccountType { get; set; }

        [NotMapped]
        public AccountType Type { get { return (AccountType)AccountType; } }

        public bool IsBasic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accounts> Accounts { get; set; }
    }
}
