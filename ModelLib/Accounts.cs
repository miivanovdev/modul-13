namespace ModelLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Accounts : EntityBase
    {
        public Accounts(int ownerId, string ownerName, decimal amount, decimal interestRate,
                        int departmentId, int minTerm, int delay, int accountType)
        {
            OwnerId         = ownerId;
            OwnerName       = OwnerName;
            Amount          = amount;
            InterestRate    = interestRate;
            DepartmentId    = departmentId;
            MinTerm         = minTerm;
            Delay           = delay;
            AccountType     = accountType;
        }

        public Accounts() { }
                
        [Index("IDX_AccountDepartmentOwnerId", IsUnique = false, Order = 1)]
        public int DepartmentId { get; set; }
        
        [Index("IDX_AccountDepartmentOwnerId", IsUnique = false, Order = 2)]
        public int OwnerId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OwnerName { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public int MinTerm { get; set; }

        [Column(TypeName = "money")]
        public decimal InterestRate { get; set; }

        public int AccountType { get; set; }

        public bool BadHistory { get; set; }

        public int Delay { get; set; }

        [NotMapped]
        public AccountType Type { get { return (AccountType)AccountType; } }
    }

}