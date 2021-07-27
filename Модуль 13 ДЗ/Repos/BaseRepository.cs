using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ.Repos
{
    /// <summary>
    /// Базовый класс хранилища
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IDisposable, IRepository<T> where T : EntityBase, new()
    {
        /// <summary>
        /// Контекст базы данных
        /// </summary>
        private readonly BankContext dbContext;

        /// <summary>
        /// Таблица данных
        /// </summary>
        private readonly DbSet<T> table;
        protected BankContext context => dbContext;

        public BaseRepository(BankContext db)
        {
            if (db == null)
                throw new ArgumentNullException("BankContext");

            dbContext = db;
            table = dbContext.Set<T>();
        }

        /// <summary>
        /// Создать тип
        /// </summary>
        /// <param name="item"></param>
        public void Create(T item)
        {
            table.Add(item);
            SaveChanges();
        }

        /// <summary>
        /// Удалить тип по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            T item = table.Find(id);

            if (item == null)
                throw new Exception($"Невозможно удалить элемент {id} - элемент не найден!");

            dbContext.Entry(item).State = EntityState.Deleted;
            SaveChanges();
        }

        /// <summary>
        /// Удалить тип по идентификатору и временной метке
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        public void Delete(int id, byte[] timestamp)
        {
            dbContext.Entry(new T() { Id = id, Timestamp = timestamp }).State = EntityState.Deleted;
            SaveChanges();
        }

        /// <summary>
        /// Получить коллекцию данных типа
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetList()
        {
            return table.ToList();
        }

        /// <summary>
        /// Получить тип по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetOne(int id)
        {
            return table.Find(id);
        }

        /// <summary>
        /// Обновить тип
        /// </summary>
        /// <param name="item"></param>
        public void Update(T item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
            SaveChanges();
        }

        /// <summary>
        /// Обновить несколько экземпляров типа
        /// </summary>
        /// <param name="items"></param>
        public void UpdateRange(T[] items)
        {
            foreach(var i in items)
                dbContext.Entry(i).State = EntityState.Modified;

            SaveChanges();
        }

        /// <summary>
        /// Пометить объект у удалению
        /// </summary>
        public void Dispose()
        {
            dbContext?.Dispose();
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <returns></returns>
        internal int SaveChanges()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                Rollback();
                throw ex;
            }
            catch (CommitFailedException ex)
            {
                Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Откатить изменения
        /// </summary>
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
