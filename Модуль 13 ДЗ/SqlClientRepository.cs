using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ModelLib;

namespace Модуль_13_ДЗ
{
    class SqlClientRepository : IRepository<Client>
    {        
        private string SelectAllCommand { get; set; }
        private string SelectOneCommand { get; set; }
        private string UpdateCommand { get; set; }
        private string DeleteCommand { get; set; }
        private string InsertCommand { get; set; }
        private string ConnectionString { get; set; }

        public SqlClientRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
        {
            ConnectionString = connectionString;
            SelectAllCommand = selectAllCommand;
            SelectOneCommand = selectOneCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
            InsertCommand = insertCommand;
        }        

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

        public void Update(Client item)
        {
            int rowAffected = SqlHelper.ExecuteNonQuery(ConnectionString,
                                                                UpdateCommand,
                                                                CommandType.StoredProcedure,
                                                                new SqlParameter[] {
                                                                new SqlParameter("@FirstName", item.FirstName),
                                                                new SqlParameter("@SecondName", item.SecondName),
                                                                new SqlParameter("@Surname", item.Surname),
                                                                new SqlParameter("@Amount", item.Amount),
                                                                new SqlParameter("@BadHistory", item.BadHistory)});
            if (rowAffected == 0)
                throw new Exception("Не удалось обновить запись!");
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
    }
}
