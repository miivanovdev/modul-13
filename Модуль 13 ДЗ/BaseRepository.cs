using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class BaseRepository<T> : IDisposable, IRepository<T> where T : EntityBase, new()
    {
        private readonly BankEntities dbContext;
        private readonly DbSet<T> table;
        protected BankEntities context => dbContext;

        public BaseRepository(BankEntities db)
        {
            dbContext = db;
            table = dbContext.Set<T>();
        }

        public void Create(T item)
        {
            table.Add(item);
            SaveChanges();
        }

        public void Delete(int id)
        {
            T item = table.Find(id);

            if (item == null)
                throw new Exception($"Невозможно удалить элемент {id} - элемент не найден!");

            dbContext.Entry(item).State = EntityState.Deleted;
            SaveChanges();
        }

        public void Delete(int id, byte[] timestamp)
        {
            dbContext.Entry(new T() { Id = id, Timestamp = timestamp }).State = EntityState.Deleted;
            SaveChanges();
        }

        public virtual IEnumerable<T> GetList()
        {
            return table.ToList();
        }

        public T GetOne(int id)
        {
            return table.Find(id);
        }

        public void Update(T item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        public void UpdateRange(T[] items)
        {
            foreach(var i in items)
                dbContext.Entry(i).State = EntityState.Modified;

            SaveChanges();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        internal int SaveChanges()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw;
            }
            catch (CommitFailedException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Rollback()
        {
            var changedEntries = dbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
