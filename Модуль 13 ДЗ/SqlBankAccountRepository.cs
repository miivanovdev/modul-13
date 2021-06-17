using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ModelLib;

namespace Модуль_13_ДЗ
{
    /// <summary>
    /// Репозиторий счето
    /// </summary>
    class SqlBankAccountRepository : IRepository<BankAccount>
    {
        public SqlBankAccountRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
        {
            ConnectionString = connectionString;
            SelectAllCommand = selectAllCommand;
            SelectOneCommand = selectOneCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
            InsertCommand = insertCommand;
        }

        /// <summary>
        /// Команда выбора всех счетов
        /// </summary>
        private readonly string SelectAllCommand;

        /// <summary>
        /// Команда выбора одного счета
        /// </summary>
        private readonly string SelectOneCommand;

        /// <summary>
        /// Команда обновления счета
        /// </summary>
        private readonly string UpdateCommand;

        /// <summary>
        /// Команда удаления счета
        /// </summary>
        private readonly string DeleteCommand;

        /// <summary>
        /// Команда вставки счета
        /// </summary>
        private readonly string InsertCommand;

        /// <summary>
        /// Строка подключения
        /// </summary>
        private readonly string ConnectionString;

        /// <summary>
        /// Метод привязывающий данные из БД
        /// к модели
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private BankAccount ReadOne(SqlDataReader reader)
        {
            BankAccount bankAccount = new BankAccount();
            Type type = typeof(BankAccount);

            foreach (var prop in type.GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(bankAccount, reader[prop.Name]);
                }
            }

            return bankAccount;
        }

        /// <summary>
        /// Метод создания записи о созданном
        /// счете в БД
        /// </summary>
        /// <param name="item"></param>
        public void Create(BankAccount item)
        {
            int id = (int)SqlHelper.ExecuteScalar(ConnectionString,
                                                                InsertCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@Amount", item.Amount),
                                                                new SqlParameter("@CreatedDate", item.CreatedDate),
                                                                new SqlParameter("@Delay", item.Delay),
                                                                new SqlParameter("@DepartmentId", item.DepartmentId),
                                                                new SqlParameter("@OwnerId", item.OwnerId),
                                                                new SqlParameter("@OwnerName", item.OwnerName),
                                                                new SqlParameter("@InterestRate", item.InterestRate),
                                                                new SqlParameter("@MinTerm", item.MinTerm),
                                                                new SqlParameter("@AccountType", item.AccountType),
                                                                new SqlParameter("@BadHistory", item.BadHistory)});

            item.AccountId = id;
        }

        /// <summary>
        /// Метод удаления записи об
        /// удаленном счете из БД
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            int rowAffected = SqlHelper.ExecuteNonQuery(ConnectionString,
                                                                DeleteCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] { new SqlParameter("@Id", id) });
            if (rowAffected == 0)
                throw new Exception("Не удалось удалить запись!");
        }

        /// <summary>
        /// Метод получения коллекции записей данных счетов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankAccount> GetList()
        {
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);

            List<BankAccount>  Accounts = new List<BankAccount>();

            if(dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Accounts.Add(ReadOne(dataReader));
                }
            }
            
            dataReader.Close();

            return Accounts;
        }

        /// <summary>
        /// Метод получения одной записи данных счета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BankAccount GetOne(int id)
        {
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString,
                                                    SelectOneCommand,
                                                    CommandType.StoredProcedure,
                                                    new SqlParameter[] { new SqlParameter("@Id", id) });
            dataReader.Read();
            BankAccount bankAccount = ReadOne(dataReader);
            dataReader.Close();
            return bankAccount;
        }

        /// <summary>
        /// Метод обновления записи о счете
        /// </summary>
        /// <param name="item"></param>
        public void Update(BankAccount item)
        {
            int rowAffected = SqlHelper.ExecuteNonQuery(ConnectionString,
                                                                UpdateCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@Amount", item.Amount),
                                                                new SqlParameter("@Id", item.AccountId)});
            if (rowAffected == 0)
                throw new Exception("Не удалось обновить запись!");
        }

        /// <summary>
        /// Метод обновления записи о счетах
        /// при переводе между ними
        /// </summary>
        /// <param name="item"></param>
        public void UpdateBoth(BankAccount item1, BankAccount item2)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = UpdateCommand;
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    // выполняем две отдельные команды
                    command.Parameters.AddRange(new SqlParameter[] {
                                                new SqlParameter("@Amount", item1.Amount),
                                                new SqlParameter("@Id", item1.AccountId)});

                    int rowAffected = command.ExecuteNonQuery();

                    if (rowAffected == 0)
                        throw new Exception("Не удалось обновить запись!");

                    command.Parameters.Clear();
                    command.Parameters.AddRange(new SqlParameter[] {
                                                new SqlParameter("@Amount", item2.Amount),
                                                new SqlParameter("@Id", item2.AccountId)});

                    rowAffected += command.ExecuteNonQuery();

                    if (rowAffected < 2)
                        throw new Exception("Не удалось обновить запись!");

                    // подтверждаем транзакцию
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // если ошибка, откатываем назад все изменения
                    transaction.Rollback();
                    connection.Close();
                    throw ex;
                }
            }            
        }
    }
}
