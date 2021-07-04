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
        public SqlLogRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
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

        /// <summary>
        /// Метод получения коллекции сообщений логов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogMessage> GetList()
        {
            List<LogMessage> list = new List<LogMessage>();

            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);

            if(dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    LogMessage logMessage = ReadOne(dataReader);
                    list.Add(logMessage);
                }
            }            

            dataReader.Close();

            return list;
        }

        /// <summary>
        /// Метод получения одной записи данных логов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод создания записи о созданном
        /// сообщении лога в БД
        /// </summary>
        /// <param name="item"></param>
        public void Create(LogMessage item)
        {
            int id = (int)SqlHelper.ExecuteScalar(ConnectionString,
                                                InsertCommand,
                                                CommandType.StoredProcedure,
                                                new SqlParameter[] {
                                                new SqlParameter("@Message", item.Message),
                                                new SqlParameter("@Time", item.Time)});
                                                                

            item.MessageId = id;
        }

        /// <summary>
        /// Метод удаления записи об
        /// удаленном сообщении лога из БД
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
        /// Метод обновления записи сообщения лога
        /// </summary>
        /// <param name="item"></param>
        public void Update(LogMessage item)
        {
            throw new NotImplementedException();
        }
                
        public void UpdateRange(LogMessage[] items)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, byte[] timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
