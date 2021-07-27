namespace ModelLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Модель счета
    /// </summary>
    public partial class Accounts : EntityBase
    {        
        public Accounts() { }
             
        [Index("IDX_AccountDepartmentOwnerId", IsUnique = false, Order = 1)]
        public int? DepartmentsRefId { get; set; }

        [ForeignKey("DepartmentsRefId")]
        public virtual Departments Departments { get; set; }
        
        [Index("IDX_AccountDepartmentOwnerId", IsUnique = false, Order = 2)]
        public int ClientsRefId { get; set; }

        [ForeignKey("ClientsRefId")]
        public virtual Clients Clients { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ClientsName { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public int MinTerm { get; set; }

        [Column(TypeName = "money")]
        public decimal InterestRate { get; set; }
                
        public bool BadHistory { get; set; }        

        public int AccountTypesId { get; set; }

        [ForeignKey("AccountTypesId")]
        public virtual AccountTypes AccountTypes { get; set; }
    }

}