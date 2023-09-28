using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Data.Models.MqttTopics;
using YARG.Models;

namespace YARG.DAL
{
    public class RPIDAL
    {
        private readonly string _connectionString;

        public RPIDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task AddRPIHeartbeat(RPIHeartbeatTopic rPIHeartbeatTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddRPIHeartbeat";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", rPIHeartbeatTopic.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_macAddress", rPIHeartbeatTopic.MacAddress);
                sqlCommand.Parameters.AddWithValue("_hostname", rPIHeartbeatTopic.Hostname);

                sqlCommand.Parameters.AddWithValue("_cpuUsage", rPIHeartbeatTopic.CpuUsage);
                sqlCommand.Parameters.AddWithValue("_memoryUsage", rPIHeartbeatTopic.MemoryUsage);
                sqlCommand.Parameters.AddWithValue("_diskUsage", rPIHeartbeatTopic.DiskUsage);
                sqlCommand.Parameters.AddWithValue("_uptimeMillis", rPIHeartbeatTopic.Uptime);
                sqlCommand.Parameters.AddWithValue("_temperature", rPIHeartbeatTopic.Temperature);
                sqlCommand.Parameters.AddWithValue("_loadAverage", rPIHeartbeatTopic.LoadAverage);
                sqlCommand.Parameters.AddWithValue("_voltage", rPIHeartbeatTopic.Voltage);
                sqlCommand.Parameters.AddWithValue("_createdBy", rPIHeartbeatTopic.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", rPIHeartbeatTopic.CreateDate);

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
        public async Task UpdateRPIHello(ESTOPTopic eSTOPTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateHelloRPI";
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
        public async Task AddRPIHello(RPIHelloTopic rPIHelloTopic)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddRPIHello";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", rPIHelloTopic.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_macAddress", rPIHelloTopic.MACAddress);
                sqlCommand.Parameters.AddWithValue("_hostname", rPIHelloTopic.Hostname);
                sqlCommand.Parameters.AddWithValue("_osName", rPIHelloTopic.OSName);
                sqlCommand.Parameters.AddWithValue("_osVersion", rPIHelloTopic.OSVersion);
                sqlCommand.Parameters.AddWithValue("_bootloaderFirmwareVersion", rPIHelloTopic.BootloaderFirmwareVersion);
                sqlCommand.Parameters.AddWithValue("_cpuModel", rPIHelloTopic.CPUModel);
                sqlCommand.Parameters.AddWithValue("_cpuCores", rPIHelloTopic.CPUCores);
                sqlCommand.Parameters.AddWithValue("_cpuTemperature", rPIHelloTopic.CPUTemperature);
                sqlCommand.Parameters.AddWithValue("_cpuSerialNumber", rPIHelloTopic.CPUSerialNumber);
                sqlCommand.Parameters.AddWithValue("_totalRAM", rPIHelloTopic.TotalRAM);
                sqlCommand.Parameters.AddWithValue("_totalDiskSpace", rPIHelloTopic.TotalDiskSpace);
                sqlCommand.Parameters.AddWithValue("_totalUsedDiskSpace", rPIHelloTopic.TotalUsedDiskSpace);
                sqlCommand.Parameters.AddWithValue("_uptimeMillis", rPIHelloTopic.UptimeMillis);
                sqlCommand.Parameters.AddWithValue("_firstBootDateTime", rPIHelloTopic.FirstBootDateTime);
                sqlCommand.Parameters.AddWithValue("_createdBy", rPIHelloTopic.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", rPIHelloTopic.CreateDate);

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

        public async Task AddRPIServiceYargHeartbeat(RPIServiceYARG rPIServiceYARG)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddRPIServiceYargHeartbeat";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", rPIServiceYARG.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_rpiHeartbeatID", rPIServiceYARG.RPIHeartbeatID.ToString());
                sqlCommand.Parameters.AddWithValue("_yargAppCurrentTasks", rPIServiceYARG.YargAppCurrentTasks);
                sqlCommand.Parameters.AddWithValue("_yargAppTaskLimit", rPIServiceYARG.YargAppTaskLimit);
                sqlCommand.Parameters.AddWithValue("_yargAppCpuCount", rPIServiceYARG.YargAppCpuCount);
                sqlCommand.Parameters.AddWithValue("_yargAppStatus", rPIServiceYARG.YargAppStatus);
                sqlCommand.Parameters.AddWithValue("_createdBy", rPIServiceYARG.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", rPIServiceYARG.CreateDate);

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

        public async Task<string> GetYARGServiceStatus()
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            string result = "";

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetAppStatus";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                result = (string)await sqlCommand.ExecuteScalarAsync();
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
    
        public async Task UpdateBackupInfo(BackupInfo backupInfo)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateBackupInfo";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_lastBackupEndTime", backupInfo.LastBackupEndTime);

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
