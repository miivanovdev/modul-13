using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using ModelLib;


namespace Модуль_13_ДЗ
{
    class ClientInputMediator
    {
        private readonly SqlConnection Connection;

        public ClientInputMediator(SqlConnection dbConnection)
        {
            Connection = dbConnection;
        }

        public Client GetClient()
        {
            ClientInputViewModel inputViewModel = new ClientInputViewModel();
            ClientInputDialog inputDialog = new ClientInputDialog(inputViewModel);
            int rowAffected = 0;

            if (inputDialog.ShowDialog() == true)
            {
                if(Connection.State == ConnectionState.Open)
                {
                    SqlCommand insertCommand = new SqlCommand("createClient", Connection) { CommandType = CommandType.StoredProcedure };
                    insertCommand.Parameters.AddWithValue("@FirstName", inputViewModel.Client.FirstName);
                    insertCommand.Parameters.AddWithValue("@SecondName", inputViewModel.Client.SecondName);
                    insertCommand.Parameters.AddWithValue("@Surname", inputViewModel.Client.Surname);
                    insertCommand.Parameters.AddWithValue("@Amount", inputViewModel.Client.Amount);
                    insertCommand.Parameters.AddWithValue("@BadHistory", inputViewModel.Client.BadHistory);

                    try
                    {
                        rowAffected = insertCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }                
            }

            return inputViewModel.Client;
        }
    }
}
