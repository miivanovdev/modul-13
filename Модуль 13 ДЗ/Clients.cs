namespace Модуль_13_ДЗ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clients : EntityBase
    {
        public Clients(string firstName, string secondName, string surname, decimal amount, bool badHistory = false)
        {
            FirstName = firstName;
            SecondName = secondName;
            Surname = surname;
            Amount = amount;
            BadHistory = badHistory;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clients()
        {
            Accounts = new HashSet<Accounts>();
        }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string SecondName { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public bool BadHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accounts> Accounts { get; set; }

        [NotMapped]
        public string FullName { get { return $"{Surname} {FirstName} {SecondName}"; } }
    }
}
