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
    class SqlLogRepository : IRepository<LogMessage>
    {
        private string SelectAllCommand { get; set; }
        private string SelectOneCommand { get; set; }
        private string UpdateCommand { get; set; }
        private string DeleteCommand { get; set; }
        private string InsertCommand { get; set; }
        private string ConnectionString { get; set; }

        public SqlLogRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
        {
            ConnectionString = connectionString;
            SelectAllCommand = selectAllCommand;
            SelectOneCommand = selectOneCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
            InsertCommand = insertCommand;
        }

        private LogMessage ReadOne(SqlDataReader reader)
        {
            LogMessage logMessage = new LogMessage();
            Type type = typeof(LogMessage);

            foreach (var prop in type.GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(logMessage, reader[prop.Name]);
                }
            }

            return logMessage;
        }

        public IEnumerable<LogMessage> GetList()
        {
            List<LogMessage> list = new List<LogMessage>();

            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);

            while (dataReader.Read())
            {
                LogMessage logMessage = ReadOne(dataReader);
                list.Add(logMessage);
            }

            dataReader.Close();

            return list;
        }

        public LogMessage GetOne(int id)
        {
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString,
                                                    SelectOneCommand,
                                                    CommandType.StoredProcedure,
                                                    new SqlParameter[] { new SqlParameter("@Id", id) });
            dataReader.Read();

            LogMessage logMessage = ReadOne(dataReader);

            dataReader.Close();

            return logMessage;
        }

        public void Create(LogMessage item)
        {
            SqlDataReader DataReader = SqlHelper.ExecuteReader(ConnectionString,
                                                                InsertCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@Message", item.Message),
                                                                new SqlParameter("@Time", item.Time)});
                                                                

            item.MessageId = (int)DataReader["MessageId"];
            DataReader.Close();
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

        public void Update(LogMessage item)
        {
            throw new NotImplementedException();
        }

    }
}
