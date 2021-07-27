namespace Модуль_13_ДЗ
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ModelLib;

    public partial class BankContext : DbContext
    {
        public BankContext()
            : base("name=BankEntities")
        {
        }
        /// <summary>
        /// Таблица счетов
        /// </summary>
        public virtual DbSet<Accounts> Accounts { get; set; }

        /// <summary>
        /// Таблица клиентов
        /// </summary>
        public virtual DbSet<Clients> Clients { get; set; }

        /// <summary>
        /// Таблица департаментов
        /// </summary>
        public virtual DbSet<Departments> Departments { get; set; }
        
        /// <summary>
        /// Таблица типов счетов
        /// </summary>
        public virtual DbSet<AccountTypes> AccountTypes { get; set; }

        /// <summary>
        /// Таблица логов
        /// </summary>
        public virtual DbSet<Log> Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accounts>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Accounts>()
               .HasOptional<Departments>(d => d.Departments)
               .WithMany()
               .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<AccountTypes>()
              .HasOptional<Departments>(d => d.Departments)
              .WithMany()
              .WillCascadeOnDelete(false);
        }
    }
}
