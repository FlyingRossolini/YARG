using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Common_Types;
using YARG.Data.Models.MqttTopics;
using YARG.Data.Models.ViewModels;
using YARG.Models;

namespace YARG.DAL
{
    public class MixingFanScheduleDAL
    {
        private readonly string _connectionString;

        public MixingFanScheduleDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task DeleteAllMixingFanSchedulesByJarIDAsync(Guid jarID)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteMixingFanScheduleByJarID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", jarID.ToString());

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

        public async Task PrepMixingFanScheduleByIDAsync(Guid jarID)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spPrepMixingFanScheduleByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", jarID.ToString());

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

        public async Task AddMixingFanScheduleAsync(MixingFanSchedule mixingFanSchedule)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddMixingFanSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", mixingFanSchedule.ID.ToString());
                sqlCommand.Parameters.AddWithValue("jarID", mixingFanSchedule.JarID.ToString());
                sqlCommand.Parameters.AddWithValue("mfDuration", mixingFanSchedule.MFDuration);
                sqlCommand.Parameters.AddWithValue("mfStartTime", mixingFanSchedule.MFStartTime);
                sqlCommand.Parameters.AddWithValue("isErrorState", mixingFanSchedule.IsErrorState);
                sqlCommand.Parameters.AddWithValue("isAcknowledged", mixingFanSchedule.IsAcknowledged);
                sqlCommand.Parameters.AddWithValue("isCompleted", mixingFanSchedule.IsCompleted);
                sqlCommand.Parameters.AddWithValue("errorDate", mixingFanSchedule.CreateDate);
                sqlCommand.Parameters.AddWithValue("completeDate", mixingFanSchedule.CreateDate);
                sqlCommand.Parameters.AddWithValue("acknowledgeDate", mixingFanSchedule.CreateDate);
                sqlCommand.Parameters.AddWithValue("createdBy", mixingFanSchedule.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", mixingFanSchedule.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", mixingFanSchedule.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", mixingFanSchedule.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", mixingFanSchedule.IsActive);

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

        public async Task RebuildMixingFanScheduleAsync(Jar jar)
        {
            await DeleteAllMixingFanSchedulesByJarIDAsync(jar.ID);

            for (int i = 0; i < jar.MixTimesPerDay; i++)
            {
                DateTime dt = DateTime.Today;
                DateTime dtMixDate = dt.AddMinutes(1440 / jar.MixTimesPerDay * (i + 1)).AddMinutes(1 - (jar.Duration / 2));

                MixingFanSchedule mixingFanSchedule = new();
                mixingFanSchedule.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                mixingFanSchedule.JarID = jar.ID;
                mixingFanSchedule.MFStartTime = dtMixDate;
                mixingFanSchedule.MFDuration = jar.Duration;
                mixingFanSchedule.IsCompleted = false;
                mixingFanSchedule.IsAcknowledged = false;
                mixingFanSchedule.IsErrorState = false;
                mixingFanSchedule.CreatedBy = "HITMAN";
                mixingFanSchedule.CreateDate = DateTime.Now;
                mixingFanSchedule.ChangedBy = "HITMAN";
                mixingFanSchedule.ChangeDate = DateTime.Now;
                mixingFanSchedule.IsActive = true;

                await AddMixingFanScheduleAsync(mixingFanSchedule);

            }
        }

        public async Task<IEnumerable<MixingFanSchedule>> GetMixingFanSchedulesAsync()
        {
            List<MixingFanSchedule> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMixingFanSchedules";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    MixingFanSchedule mixingFanSchedule = new();
                    mixingFanSchedule.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    mixingFanSchedule.JarID = Guid.Parse(sqlDataReader["jarID"].ToString());
                    mixingFanSchedule.JarChemicalName = sqlDataReader["jarChemicalName"].ToString();
                    mixingFanSchedule.Position = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("position"));
                    mixingFanSchedule.MFStartTime = Convert.ToDateTime(sqlDataReader["mfStartTime"].ToString());
                    mixingFanSchedule.MFEndTime = Convert.ToDateTime(sqlDataReader["mfEndTime"].ToString());
                    mixingFanSchedule.MFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mfDuration"));
                    mixingFanSchedule.IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged"));
                    mixingFanSchedule.IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted"));
                    mixingFanSchedule.IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState"));
                    if (sqlDataReader["errorDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.ErrorDate = Convert.ToDateTime(sqlDataReader["errorDate"].ToString());
                    }
                    if (sqlDataReader["completeDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.CompleteDate = Convert.ToDateTime(sqlDataReader["completeDate"].ToString());
                    }
                    if (sqlDataReader["acknowledgeDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.AcknowledgeDate = Convert.ToDateTime(sqlDataReader["acknowledgeDate"].ToString());
                    }
                    mixingFanSchedule.CreatedBy = sqlDataReader["createdBy"].ToString();
                    mixingFanSchedule.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    mixingFanSchedule.ChangedBy = sqlDataReader["changedBy"].ToString();
                    mixingFanSchedule.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    mixingFanSchedule.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));

                    lstream.Add(mixingFanSchedule);
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

            return lstream;
        }

        public async Task AcknowledgeMixingFanScheduleAsync(CommandTopic remoteHostCommandViewModel)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAcknowledgeMixingFanSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("mixingFanScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCommand.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCommand.Parameters.AddWithValue("ackDate", DateTime.Now);

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

        public async Task CompleteMixingFanScheduleAsync(CommandTopic remoteHostCommandViewModel)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spCompleteMixingFanSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("mixingFanScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCommand.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCommand.Parameters.AddWithValue("ackDate", DateTime.Now);

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

        public async Task<IEnumerable<MixingFanScheduleCommand>> AreWeThereYetAsync()
        {
            List<MixingFanScheduleCommand> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetCurrentMixingFanSchedules";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("dtTheDate", DateTime.Now);

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    MixingFanScheduleCommand mixingFanScheduleCommand = new();
                    mixingFanScheduleCommand.FanNumber = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("position"));
                    mixingFanScheduleCommand.PumpSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pumpSpeed"));
                    mixingFanScheduleCommand.OverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed"));
                    mixingFanScheduleCommand.Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mfDuration"));
                    mixingFanScheduleCommand.MixingFanScheduleID = Guid.Parse(sqlDataReader["id"].ToString());

                    lstream.Add(mixingFanScheduleCommand);
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

            return lstream;
        }
        
        private async Task<MixingFanSchedule> GetMixingFanScheduleByID(Guid mfsID)
        {
            MixingFanSchedule mixingFanSchedule = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMixingFanScheduleByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", mfsID.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    mixingFanSchedule.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    mixingFanSchedule.JarID = Guid.Parse(sqlDataReader["jarID"].ToString());
                    mixingFanSchedule.JarChemicalName = sqlDataReader["jarChemicalName"].ToString();
                    mixingFanSchedule.Position = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("position"));
                    mixingFanSchedule.PumpSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pumpSpeed"));
                    mixingFanSchedule.MFStartTime = Convert.ToDateTime(sqlDataReader["mfStartTime"].ToString());
                    mixingFanSchedule.MFEndTime = Convert.ToDateTime(sqlDataReader["mfEndTime"].ToString());
                    mixingFanSchedule.MFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mfDuration"));
                    mixingFanSchedule.IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged"));
                    mixingFanSchedule.IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted"));
                    mixingFanSchedule.IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState"));
                    if (sqlDataReader["errorDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.ErrorDate = Convert.ToDateTime(sqlDataReader["errorDate"].ToString());
                    }
                    if (sqlDataReader["completeDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.CompleteDate = Convert.ToDateTime(sqlDataReader["completeDate"].ToString());
                    }
                    if (sqlDataReader["acknowledgeDate"] != DBNull.Value)
                    {
                        mixingFanSchedule.AcknowledgeDate = Convert.ToDateTime(sqlDataReader["acknowledgeDate"].ToString());
                    }
                    mixingFanSchedule.CreatedBy = sqlDataReader["createdBy"].ToString();
                    mixingFanSchedule.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    mixingFanSchedule.ChangedBy = sqlDataReader["changedBy"].ToString();
                    mixingFanSchedule.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    mixingFanSchedule.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return mixingFanSchedule;
        }

    }
}
