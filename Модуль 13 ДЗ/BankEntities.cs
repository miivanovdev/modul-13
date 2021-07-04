namespace Модуль_13_ДЗ
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BankEntities : DbContext
    {
        public BankEntities()
            : base("name=BankEntities")
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Accounts>()
                .Property(e => e.InterestRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Clients>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Clients>()
                .HasMany(e => e.Accounts);

            modelBuilder.Entity<Departments>()
                .Property(e => e.MinAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Departments>()
                .Property(e => e.InterestRate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Departments>()
                .HasMany(e => e.Accounts);                
        }
    }
}
