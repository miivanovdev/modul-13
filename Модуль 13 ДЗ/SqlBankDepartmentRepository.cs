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
        private string SelectAllCommand { get; set; }
        private string SelectOneCommand { get; set; }
        private string UpdateCommand { get; set; }
        private string DeleteCommand { get; set; }
        private string InsertCommand { get; set; }
        private string ConnectionString { get; set; }

        public SqlBankDepartmentRepository(string connectionString, string selectAllCommand, string selectOneCommand, string updateCommand, string deleteCommand, string insertCommand)
        {
            ConnectionString = connectionString;
            SelectAllCommand = selectAllCommand;
            SelectOneCommand = selectOneCommand;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;
            InsertCommand = insertCommand;
        }

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

        
        public IEnumerable<BankDepartment> GetList()
        {
            List<BankDepartment> bankDepartments = new List<BankDepartment>();

            SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, SelectAllCommand, CommandType.StoredProcedure);

            while (dataReader.Read()) 
                bankDepartments.Add(ReadOne(dataReader));
            
            dataReader.Close();

            return bankDepartments;
        }        

        public void Create(BankDepartment item)
        {
            throw new NotImplementedException();
        }

        public void Update(BankDepartment item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
