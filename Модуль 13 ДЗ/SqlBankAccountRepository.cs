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
    class SqlBankAccountRepository : IRepository<BankAccount>
    {
        private readonly string SelectAllCommand;
        private readonly string SelectOneCommand;
        private readonly string UpdateCommand;
        private readonly string DeleteCommand;
        private readonly string InsertCommand;
        private readonly string ConnectionString;

        public SqlBankAccountRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
        {
            ConnectionString = connectionString;
            SelectAllCommand = selectAllCommand;
            SelectOneCommand = selectOneCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
            InsertCommand = insertCommand;
        }

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

        public void Delete(int id)
        {
            int rowAffected = SqlHelper.ExecuteNonQuery(ConnectionString,
                                                                DeleteCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] { new SqlParameter("@Id", id) });
            if (rowAffected == 0)
                throw new Exception("Не удалось удалить запись!");
        }

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
    }
}
