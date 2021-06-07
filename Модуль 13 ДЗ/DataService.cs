using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace Модуль_13_ДЗ
{
    class DataService<T>
    {        
        public SqlDataReader DataReader { get; private set; }
        
        public DataService(SqlConnection connection, string selectCommand)
        {
            SqlCommand sqlCommand = new SqlCommand(selectCommand, connection) { CommandType = CommandType.StoredProcedure };
            DataReader = sqlCommand.ExecuteReader();
        }       

        public List<T> GetTableData()
        {
            List<T> list = new List<T>();

            Type type = typeof(T);

            while(DataReader.Read())
            {
                T t = Activator.CreateInstance<T>();

                foreach (var prop in type.GetProperties())
                {
                    if(prop.CanWrite)
                    {
                        prop.SetValue(t, DataReader[prop.Name]);
                    }                    
                }

                list.Add(t);
            }            

            return list;
        }
    }
}
