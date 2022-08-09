using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARG.Models;
using YARG.Common_Types;
using YARG.Data.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace YARG.DAL
{
    public class MixingFanScheduleDAL
    {
        private readonly IConfiguration _config;

        public MixingFanScheduleDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void DeleteAllMixingFanSchedulesByJarID(Guid jarID)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteMixingFanScheduleByJarID", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", jarID.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void PrepMixingFanScheduleByID(Guid jarID)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spPrepMixingFanScheduleByID", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", jarID.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void AddMixingFanSchedule(MixingFanSchedule mixingFanSchedule)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddMixingFanSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", mixingFanSchedule.ID.ToString());
                sqlCmd.Parameters.AddWithValue("jarID", mixingFanSchedule.JarID.ToString());
                sqlCmd.Parameters.AddWithValue("mfDuration", mixingFanSchedule.MFDuration);
                sqlCmd.Parameters.AddWithValue("mfStartTime", mixingFanSchedule.MFStartTime);
                sqlCmd.Parameters.AddWithValue("isErrorState", mixingFanSchedule.IsErrorState);
                sqlCmd.Parameters.AddWithValue("isAcknowledged", mixingFanSchedule.IsAcknowledged);
                sqlCmd.Parameters.AddWithValue("isCompleted", mixingFanSchedule.IsCompleted);
                sqlCmd.Parameters.AddWithValue("errorDate", mixingFanSchedule.CreateDate);
                sqlCmd.Parameters.AddWithValue("completeDate", mixingFanSchedule.CreateDate);
                sqlCmd.Parameters.AddWithValue("acknowledgeDate", mixingFanSchedule.CreateDate);
                sqlCmd.Parameters.AddWithValue("createdBy", mixingFanSchedule.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", mixingFanSchedule.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", mixingFanSchedule.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", mixingFanSchedule.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", mixingFanSchedule.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void RebuildMixingFanSchedule(Jar jar)
        {
            DeleteAllMixingFanSchedulesByJarID(jar.ID);

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

                AddMixingFanSchedule(mixingFanSchedule);

            }
        }

        public IEnumerable<MixingFanSchedule> GetMixingFanSchedules()
        {
            List<MixingFanSchedule> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMixingFanSchedules", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }

        public void AcknowledgeMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAcknowledgeMixingFanSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("mixingFanScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCmd.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCmd.Parameters.AddWithValue("ackDate", DateTime.Now);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public void CompleteMixingFanSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spCompleteMixingFanSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("mixingFanScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCmd.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCmd.Parameters.AddWithValue("ackDate", DateTime.Now);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }


        }

        public IEnumerable<MixingFanScheduleCommand> AreWeThereYet()
        {
            List<MixingFanScheduleCommand> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetCurrentMixingFanSchedules", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("dtTheDate", DateTime.Now);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    MixingFanScheduleCommand mixingFanScheduleCommand = new();
                    mixingFanScheduleCommand.FanNumber = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("position"));
                    mixingFanScheduleCommand.PumpSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pumpSpeed"));
                    mixingFanScheduleCommand.OverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed"));
                    mixingFanScheduleCommand.Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mfDuration"));
                    mixingFanScheduleCommand.MixingFanScheduleID = Guid.Parse(sqlDataReader["id"].ToString());

                    lstream.Add(mixingFanScheduleCommand);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;

        }
        private MixingFanSchedule GetMixingFanScheduleByID(Guid mfsID)
        {
            MixingFanSchedule mixingFanSchedule = new();

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMixingFanScheduleByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", mfsID.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
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
                    };
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return mixingFanSchedule;
        }

    }
}
