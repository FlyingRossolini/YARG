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
    public class WateringScheduleDAL
    {
        private readonly IConfiguration _config;

        public WateringScheduleDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void DeleteAllWateringSchedules()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteAllWateringSchedules", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void AddWateringSchedule(WateringSchedule wateringSchedule)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddWateringSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", wateringSchedule.ID.ToString());
                sqlCmd.Parameters.AddWithValue("potID", wateringSchedule.PotID.ToString());
                sqlCmd.Parameters.AddWithValue("efAmount", wateringSchedule.EFAmount);
                sqlCmd.Parameters.AddWithValue("efDuration", wateringSchedule.EFDuration);
                sqlCmd.Parameters.AddWithValue("efStartTime", wateringSchedule.EFStartTime);
                sqlCmd.Parameters.AddWithValue("rollover", wateringSchedule.Rollover);
                sqlCmd.Parameters.AddWithValue("isErrorState", wateringSchedule.IsErrorState);
                sqlCmd.Parameters.AddWithValue("isAcknowledged", wateringSchedule.IsAcknowledged);
                sqlCmd.Parameters.AddWithValue("isCompleted", wateringSchedule.IsCompleted);
                sqlCmd.Parameters.AddWithValue("createdBy", wateringSchedule.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", wateringSchedule.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", wateringSchedule.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", wateringSchedule.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", wateringSchedule.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void RebuildWateringSchedule()
        {
            DeleteAllWateringSchedules();

            GrowSeasonDAL growSeasonDAL = new(_config);

            GrowSeason growSeason = growSeasonDAL.GetGrowSeasonByID(growSeasonDAL.IDActiveGrowSeason());

            PotDAL potDAL = new(_config);

            int cntActiveBuckets = (from pot in potDAL.GetPots()
                                    where pot.IsActive == true
                                    select pot.ID).Count();

            LightCycleDAL lightCycleDAL = new(_config);

            double amtMinsOfDaylight = lightCycleDAL.GetLightCycleByID(growSeason.LightCycleID).DaylightHours * 60;

            foreach (Pot p in potDAL.GetPots().Where(x => x.IsActive == true))
            {

                for (int j = 0; j < growSeason.EFEventsPerDay; j++)
                {
                    DateTime dt = DateTime.Today;
                    DateTime firstdateoftheday = dt.Add(growSeason.SunriseTime.TimeOfDay).AddMinutes(amtMinsOfDaylight / (growSeason.EFEventsPerDay + 1)).AddSeconds(-(cntActiveBuckets - 1) * (9 * 60 + 30));

                    WateringSchedule ws = new();
                    ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    ws.PotID = p.ID;
                    ws.EFStartTime = DateTime.Parse(firstdateoftheday.AddMinutes(amtMinsOfDaylight / (growSeason.EFEventsPerDay + 1) * j).AddMinutes(16 * (p.QueuePosition - 1)).ToString("g"));
                    ws.EFDuration = p.EFDuration;
                    ws.EFAmount = p.EFAmount;
                    if(ws.EFStartTime.TimeOfDay < growSeason.SunriseTime.TimeOfDay)
                    {
                        ws.Rollover = 1;
                    }
                    else
                    {
                        ws.Rollover = 0;
                    }
                    ws.IsCompleted = false;
                    ws.IsAcknowledged = false;
                    ws.IsErrorState = false;
                    ws.CreatedBy = "HITMAN";
                    ws.CreateDate = DateTime.Now;
                    ws.ChangedBy = "HITMAN";
                    ws.ChangeDate = DateTime.Now;
                    ws.IsActive = true;

                    AddWateringSchedule(ws);
                }

                if (growSeason.FlgAddMorningSplash)
                {
                    DateTime dt = DateTime.Today;
                    DateTime morningsplashdt = growSeason.SunriseTime;

                    WateringSchedule ws = new();
                    ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    ws.PotID = p.ID;
                    ws.EFStartTime = morningsplashdt.AddMinutes(4 * (p.QueuePosition - 1));
                    ws.EFDuration = 4;
                    ws.EFAmount = p.EFAmount;
                    if (ws.EFStartTime.TimeOfDay < growSeason.SunriseTime.TimeOfDay)
                    {
                        ws.Rollover = 1;
                    }
                    else
                    {
                        ws.Rollover = 0;
                    }
                    ws.IsCompleted = false;
                    ws.IsAcknowledged = false;
                    ws.IsErrorState = false;
                    ws.CreatedBy = "HITMAN";
                    ws.CreateDate = DateTime.Now;
                    ws.ChangedBy = "HITMAN";
                    ws.ChangeDate = DateTime.Now;
                    ws.IsActive = true;

                    AddWateringSchedule(ws);
                }
                if (growSeason.FlgAddEveningSplash)
                {
                    DateTime dt = DateTime.Today;
                    DateTime eveningsplashdt = growSeason.SunsetTime.AddMinutes(-4);

                    WateringSchedule ws = new();
                    ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    ws.PotID = p.ID;
                    ws.EFStartTime = eveningsplashdt.AddMinutes(-4 * (p.QueuePosition - 1));
                    ws.EFDuration = 4;
                    ws.EFAmount = p.EFAmount;
                    if (ws.EFStartTime.TimeOfDay < growSeason.SunriseTime.TimeOfDay)
                    {
                        ws.Rollover = 1;
                    }
                    else
                    {
                        ws.Rollover = 0;
                    }
                    ws.IsCompleted = false;
                    ws.IsAcknowledged = false;
                    ws.IsErrorState = false;
                    ws.CreatedBy = "HITMAN";
                    ws.CreateDate = DateTime.Now;
                    ws.ChangedBy = "HITMAN";
                    ws.ChangeDate = DateTime.Now;
                    ws.IsActive = true;

                    AddWateringSchedule(ws);
                }


            }


        }

        public IEnumerable<WateringSchedule> GetWateringSchedules()
        {
            List<WateringSchedule> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetWateringSchedules", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    WateringSchedule wateringSchedule = new WateringSchedule
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        PotID = Guid.Parse(sqlDataReader["potID"].ToString()),
                        PotName = sqlDataReader["potName"].ToString(),
                        PotQueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("potQueuePosition")),
                        EFStartTime = Convert.ToDateTime(sqlDataReader["efStartTime"].ToString()),
                        EFEndTime = Convert.ToDateTime(sqlDataReader["efEndTime"].ToString()),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged")),
                        IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted")),
                        IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(wateringSchedule);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }

        public void AcknowledgeWateringSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAcknowledgeWateringSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("wateringScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCmd.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCmd.Parameters.AddWithValue("ackDate", DateTime.Now);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public void CompleteWateringSchedule(RemoteHostCommandViewModel remoteHostCommandViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spCompleteWateringSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("wateringScheduleID", remoteHostCommandViewModel.CommandID.ToString());
                sqlCmd.Parameters.AddWithValue("remoteHostName", remoteHostCommandViewModel.RemoteHostname);
                sqlCmd.Parameters.AddWithValue("ackDate", DateTime.Now);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public WateringScheduleCommand AreWeThereYet()
        {
            WateringScheduleCommand wateringScheduleCommand = null;
            Guid wsID = Guid.Empty;

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spGetCurrentWateringScheduleID", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("dtTheDate", DateTime.Now);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCmd.ExecuteReader();
                sqlCmd.Dispose();

                while (sqlDataReader.Read())
                {
                    wsID = Guid.Parse(sqlDataReader["id"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            PotDAL potDAL = new(_config);

            if (wsID != Guid.Empty)
            {
                WateringSchedule ws = GetWateringScheduleByID(wsID);
                wateringScheduleCommand = new();
                wateringScheduleCommand.WateringScheduleID = ws.ID;
                wateringScheduleCommand.PotNumber = ws.PotQueuePosition;
                wateringScheduleCommand.Amount = Convert.ToInt16(ws.EFAmount * 1000);
                wateringScheduleCommand.EbbSpeed = potDAL.GetEbbSpeedFromQueuePosition(ws.PotQueuePosition);
                wateringScheduleCommand.FlowSpeed = potDAL.GetFlowSpeedFromQueuePosition(ws.PotQueuePosition);
                wateringScheduleCommand.Duration = ws.EFDuration;

            } 

            return wateringScheduleCommand;

        }
        private WateringSchedule GetWateringScheduleByID(Guid wsID)
        {
            WateringSchedule wateringSchedule = new();

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetWateringScheduleByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid",wsID.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    {
                        wateringSchedule.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        wateringSchedule.PotID = Guid.Parse(sqlDataReader["potID"].ToString());
                        wateringSchedule.PotName = sqlDataReader["potName"].ToString();
                        wateringSchedule.PotQueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("potQueuePosition"));
                        wateringSchedule.EFStartTime = Convert.ToDateTime(sqlDataReader["efStartTime"].ToString());
                        wateringSchedule.EFEndTime = Convert.ToDateTime(sqlDataReader["efEndTime"].ToString());
                        wateringSchedule.EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration"));
                        wateringSchedule.EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount"));
                        wateringSchedule.IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged"));
                        wateringSchedule.IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted"));
                        wateringSchedule.IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState"));
                        wateringSchedule.CreatedBy = sqlDataReader["createdBy"].ToString();
                        wateringSchedule.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        wateringSchedule.ChangedBy = sqlDataReader["changedBy"].ToString();
                        wateringSchedule.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        wateringSchedule.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    };
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return wateringSchedule;
        }

    }
}
