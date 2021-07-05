using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;

using System.Data;
using System.Data.SqlClient;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class SqlBankDepartmentRepository : IRepository<BankDepartment>
    {
        public SqlBankDepartmentRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
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
        private BankDepartment ReadOne(SqlDataReader reader)
        {
            BankDepartment department = new BankDepartment();
            Type type = typeof(BankDepartment);

            foreach (var prop in type.GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(department, reader[prop.Name]);
                }
            }

            return department;
        }

        /// <summary>
        /// Метод получения одной записи данных департамента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BankDepartment GetOne(int id)
        {
            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString,
                                                    SelectOneCommand,
                                                    CommandType.StoredProcedure,
                                                    new SqlParameter[] { new SqlParameter("@Id", id) });
            dataReader.Read();

            BankDepartment bankDepartment = ReadOne(dataReader);

            dataReader.Close();

            return bankDepartment;
        }

        /// <summary>
        /// Метод получения коллекции записей данных департаментов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BankDepartment> GetList()
        {
            List<BankDepartment> bankDepartments = new List<BankDepartment>();

            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);

            if(dataReader.HasRows)
            {
                while (dataReader.Read())
                    bankDepartments.Add(ReadOne(dataReader));
            }           
            
            dataReader.Close();

            return bankDepartments;
        }

        /// <summary>
        /// Метод создания записи о созданном
        /// департаменте в БД
        /// </summary>
        /// <param name="item"></param>
        public void Create(BankDepartment item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод обновления записи о департаменте
        /// </summary>
        /// <param name="item"></param>
        public void Update(BankDepartment item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод удаления записи об
        /// удаленном департаменте из БД
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод обновления нескольких элементов
        /// </summary>
        /// <param name="items"></param>
        public void UpdateRange(BankDepartment[] items)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, byte[] timestamp)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
