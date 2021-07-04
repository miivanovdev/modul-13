using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ModelLib;

namespace Модуль_13_ДЗ
{
    class SqlClientRepository : IRepository<Client>
    {
        public SqlClientRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
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
        private Client ReadOne(SqlDataReader reader)
        {
            Client client = new Client();
            Type type = typeof(Client);

            foreach (var prop in type.GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(client, reader[prop.Name]);
                }
            }

            return client;
        }

        /// <summary>
        /// Метод получения коллекции записей данных клиентов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> GetList()
        {
            List<Client> list = new List<Client>();
            
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);                

            if(dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    Client client = ReadOne(dataReader);
                    list.Add(client);
                }
            }           

            dataReader.Close();
            
            return list;
        }

        /// <summary>
        /// Метод получения одной записи данных клиента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetOne(int id)
        {
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString,
                                                    SelectOneCommand,
                                                    CommandType.StoredProcedure,
                                                    new SqlParameter[] { new SqlParameter("@Id", id) });
            dataReader.Read();
            Client client = ReadOne(dataReader);
            dataReader.Close();

            return client;
        }

        /// <summary>
        /// Метод создания записи о созданном
        /// клиенте в БД
        /// </summary>
        /// <param name="item"></param>
        public void Create(Client item)
        {
            int id = (int)SqlHelper.ExecuteScalar(ConnectionString,
                                                                InsertCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@FirstName", item.FirstName),
                                                                new SqlParameter("@SecondName", item.SecondName),
                                                                new SqlParameter("@Surname", item.Surname),
                                                                new SqlParameter("@Amount", item.Amount),
                                                                new SqlParameter("@BadHistory", item.BadHistory)});

            item.ClientId = id;
        }

        /// <summary>
        /// Метод обновления записи о клиенте
        /// </summary>
        /// <param name="item"></param>
        public void Update(Client item)
        {
            int rowAffected = SqlHelper.ExecuteNonQuery(ConnectionString,
                                                                UpdateCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@ClientId", item.ClientId),
                                                                new SqlParameter("@FirstName", item.FirstName),
                                                                new SqlParameter("@SecondName", item.SecondName),
                                                                new SqlParameter("@Surname", item.Surname),
                                                                new SqlParameter("@Amount", item.Amount),
                                                                new SqlParameter("@BadHistory", item.BadHistory)});
            if (rowAffected == 0)
                throw new Exception("Не удалось обновить запись!");
        }

        /// <summary>
        /// Метод удаления записи об
        /// удаленном клиенте из БД
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
        /// Метод обновления нескольких элементов
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public void UpdateRange(Client[] items)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, byte[] timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
