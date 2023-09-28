using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Data.Models.MqttTopics;
using YARG.Models;

namespace YARG.DAL
{
    public class BotDAL
    {
        private readonly string _connectionString;

        public BotDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task UpdateHelloBot(ESTOPTopic eSTOPTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateHelloBot";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_changedBy", eSTOPTopic.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", eSTOPTopic.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_expiryDate", eSTOPTopic.ExpiryDate);

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
        public async Task AddHelloBot(BotSaysHelloTopic botSaysHelloTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddBotHello";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", botSaysHelloTopic.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_uptimeMillis", botSaysHelloTopic.UptimeMillis);
                sqlCommand.Parameters.AddWithValue("_macAddress", botSaysHelloTopic.MacAddress);
                sqlCommand.Parameters.AddWithValue("_hostname", botSaysHelloTopic.Hostname);
                sqlCommand.Parameters.AddWithValue("_cpuFreqMHz", botSaysHelloTopic.CpuFreqMHz);
                sqlCommand.Parameters.AddWithValue("_flashChipSize", botSaysHelloTopic.FlashChipSize);
                sqlCommand.Parameters.AddWithValue("_flashChipSpeed", botSaysHelloTopic.FlashChipSpeed);
                sqlCommand.Parameters.AddWithValue("_vccVoltage", botSaysHelloTopic.VccVoltage);
                sqlCommand.Parameters.AddWithValue("_freeFlash", botSaysHelloTopic.FreeFlash);
                sqlCommand.Parameters.AddWithValue("_createdBy", botSaysHelloTopic.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", botSaysHelloTopic.CreateDate);

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

        public async Task AddBotHeartbeat(BotHeartbeatTopic botHeartbeatTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddBotHeartbeat";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", botHeartbeatTopic.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_hostname", botHeartbeatTopic.Hostname);
                sqlCommand.Parameters.AddWithValue("_uptimeMillis", botHeartbeatTopic.UptimeMillis);
                sqlCommand.Parameters.AddWithValue("_task", botHeartbeatTopic.Task);
                sqlCommand.Parameters.AddWithValue("_stackHighWaterMark", botHeartbeatTopic.StackHighWaterMark);
                sqlCommand.Parameters.AddWithValue("_vccVoltage", botHeartbeatTopic.VccVoltage);
                sqlCommand.Parameters.AddWithValue("_rssi", botHeartbeatTopic.RSSI);
                sqlCommand.Parameters.AddWithValue("_freeHeap", botHeartbeatTopic.FreeHeap);
                sqlCommand.Parameters.AddWithValue("_heapSize", botHeartbeatTopic.HeapSize);
                sqlCommand.Parameters.AddWithValue("_temperature", botHeartbeatTopic.Temperature);
                sqlCommand.Parameters.AddWithValue("_createdBy", botHeartbeatTopic.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", botHeartbeatTopic.CreateDate);

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
    }
}
