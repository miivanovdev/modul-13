using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ModelLib
{
    public class DataService
    {
        private SqlConnection SqlConnection { get; set; }

        public DataTable DataTable { get; set; }
                
        private SqlDataAdapter SqlDataAdapter { get; set; }

        public string SelectProcedureName { get; set; }
        public string InsertProcedureName { get; set; }
        public string UpdateProcedureName { get; set; }
        public string DeleteProcedureName { get; set; }

        public bool Initialized { get; private set; }

        public DataService(SqlConnection sqlConnection)
        {
            SqlConnection = sqlConnection;
            DataTable = new DataTable();
            SqlDataAdapter = new SqlDataAdapter();

            Initialized = false;
        }

        public DataService(SqlConnection sqlConnection, string selectProcedureName, string insertProcedureName, string updateProcedureName, string deleteProcedureName)
            : this(sqlConnection)
        {
            SelectProcedureName = selectProcedureName;
            InsertProcedureName = insertProcedureName;
            UpdateProcedureName = updateProcedureName;
            DeleteProcedureName = deleteProcedureName;

            Initialize();
        }

        public void Initialize()
        {
            SqlDataAdapter.SelectCommand = new SqlCommand(SelectProcedureName, SqlConnection) { CommandType = CommandType.StoredProcedure };

            SqlDataAdapter.InsertCommand = new SqlCommand(InsertProcedureName, SqlConnection) { CommandType = CommandType.StoredProcedure };
            
            SqlDataAdapter.UpdateCommand = new SqlCommand(UpdateProcedureName, SqlConnection) { CommandType = CommandType.StoredProcedure };

            SqlDataAdapter.DeleteCommand = new SqlCommand(DeleteProcedureName, SqlConnection) { CommandType = CommandType.StoredProcedure };

            SqlDataAdapter.Fill(DataTable);

            Initialized = true;
        }
    }
}
