using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARG.Models;
using YARG.Common_Types;
using YARG.Data.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using YARG.Data.Models.MqttTopics;

namespace YARG.DAL
{
    public class PumpWorklogDAL
    {
        private readonly string _connectionString;

        public PumpWorklogDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task AddPumpWorklog(PumpWorklogTopic pumpWorklogTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddPumpWorklog";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", pumpWorklogTopic.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_pumpID", pumpWorklogTopic.PumpID.ToString());
                sqlCommand.Parameters.AddWithValue("_runtimeMillis", pumpWorklogTopic.RuntimeMillis);
                sqlCommand.Parameters.AddWithValue("_flowAmountmL", pumpWorklogTopic.FlowAmountmL);
                sqlCommand.Parameters.AddWithValue("_createdBy", pumpWorklogTopic.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", pumpWorklogTopic.CreateDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }

        public async Task<decimal> GetPPLByPumpID(Guid id)
        {
            decimal result = 0;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPPLByPumpID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    result = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("ppl"));
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return result;
        }
    }
}
