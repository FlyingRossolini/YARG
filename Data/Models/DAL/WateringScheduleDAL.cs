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
using YARG.Data.Models.BusinessObjects;

namespace YARG.DAL
{
    public class WateringScheduleDAL
    {
        private readonly string _connectionString;
        private readonly GrowSeasonDAL _growseasonDAL;
        private readonly PotDAL _potDAL;
        private readonly LightCycleDAL _lightCycleDAL;

        public WateringScheduleDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
            _growseasonDAL = new(configuration);
            _potDAL = new(configuration);
            _lightCycleDAL = new(configuration);
        }

        private async Task DeleteAllWateringSchedulesAsync()
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteAllWateringSchedules";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

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

        public async Task AddWateringSchedule(WateringSchedule wateringSchedule)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddWateringSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", wateringSchedule.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_potID", wateringSchedule.PotID.ToString());
                sqlCommand.Parameters.AddWithValue("_efAmount", wateringSchedule.EFAmount);
                sqlCommand.Parameters.AddWithValue("_efDuration", wateringSchedule.EFDuration);
                sqlCommand.Parameters.AddWithValue("_efStartTime", wateringSchedule.EFStartTime);
                sqlCommand.Parameters.AddWithValue("_rollover", wateringSchedule.Rollover);
                //sqlCommand.Parameters.AddWithValue("isErrorState", wateringSchedule.IsErrorState);
                //sqlCommand.Parameters.AddWithValue("isAcknowledged", wateringSchedule.IsAcknowledged);
                //sqlCommand.Parameters.AddWithValue("isCompleted", wateringSchedule.IsCompleted);
                //sqlCommand.Parameters.AddWithValue("createdBy", wateringSchedule.CreatedBy);
                //sqlCommand.Parameters.AddWithValue("createDate", wateringSchedule.CreateDate);
                //sqlCommand.Parameters.AddWithValue("changedBy", wateringSchedule.ChangedBy);
                //sqlCommand.Parameters.AddWithValue("changeDate", wateringSchedule.ChangeDate);
                //sqlCommand.Parameters.AddWithValue("isActive", wateringSchedule.IsActive);

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

        public async Task RebuildWateringSchedule()
        {
            await DeleteAllWateringSchedulesAsync();

            GrowSeason growSeason = await _growseasonDAL.GetGrowSeasonByIDAsync(await _growseasonDAL.IDActiveGrowSeasonAsync());

            CurrentIrrigationCalcs currentIrrigationCalcs = await _growseasonDAL.GetCurrentIrrigationCalcs();

            int cntActiveBuckets = (from pot in await _potDAL.GetPotsAsync()
                                    where pot.IsActive == true
                                    select pot.ID).Count();

            double amtMinsOfDaylight = currentIrrigationCalcs.DaylightHoursPerDay * 60;

            foreach (Pot p in await _potDAL.GetPotsAsync())
            {

                if (p.IsActive)
                {
                    for (int j = 0; j < currentIrrigationCalcs.IrrigationEventsPerDay; j++)
                    {
                        DateTime dt = DateTime.Today;
                        DateTime firstdateoftheday = DateTime.MinValue;
                        if (DateTime.Now >= currentIrrigationCalcs.SunriseYesterday && DateTime.Now <= currentIrrigationCalcs.SunsetYesterday)
                        {
                            firstdateoftheday = dt.Add(currentIrrigationCalcs.SunriseYesterday.TimeOfDay).AddMinutes(amtMinsOfDaylight / (currentIrrigationCalcs.IrrigationEventsPerDay + 1)).AddSeconds(-(cntActiveBuckets - 1) * (9 * 60 + 30));
                        }
                        else
                        {
                            firstdateoftheday = dt.Add(currentIrrigationCalcs.Sunrise.TimeOfDay).AddMinutes(amtMinsOfDaylight / (currentIrrigationCalcs.IrrigationEventsPerDay + 1)).AddSeconds(-(cntActiveBuckets - 1) * (9 * 60 + 30));
                        }

                        WateringSchedule ws = new();
                        ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                        ws.PotID = p.ID;
                        ws.EFStartTime = DateTime.Parse(firstdateoftheday.AddMinutes(amtMinsOfDaylight / (currentIrrigationCalcs.IrrigationEventsPerDay + 1) * j).AddMinutes(16 * (p.QueuePosition - 1)).ToString("g"));
                        ws.EFDuration = currentIrrigationCalcs.SoakTime;
                        //ws.EFAmount = p.EFAmount;
                        ws.EFAmount = p.CurrentCapacity;
                        if (ws.EFStartTime.TimeOfDay < currentIrrigationCalcs.Sunrise.TimeOfDay)
                        {
                            ws.Rollover = 1;
                        }
                        else
                        {
                            ws.Rollover = 0;
                        }
                        //ws.IsCompleted = false;
                        //ws.IsAcknowledged = false;
                        //ws.IsErrorState = false;
                        //ws.CreatedBy = "HITMAN";
                        //ws.CreateDate = DateTime.Now;
                        //ws.ChangedBy = "HITMAN";
                        //ws.ChangeDate = DateTime.Now;
                        //ws.IsActive = true;

                        await AddWateringSchedule(ws);
                    }

                    if (currentIrrigationCalcs.IsMorningSip)
                    {
                        DateTime dt = DateTime.Today;
                        DateTime morningsplashdt = DateTime.MinValue;
                        if (DateTime.Now >= currentIrrigationCalcs.SunriseYesterday && DateTime.Now <= currentIrrigationCalcs.SunsetYesterday)
                        {
                            morningsplashdt = currentIrrigationCalcs.SunriseYesterday;                        }
                        else
                        {
                            morningsplashdt = currentIrrigationCalcs.Sunrise;
                        }

                        WateringSchedule ws = new();
                        ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                        ws.PotID = p.ID;
                        ws.EFStartTime = morningsplashdt.AddMinutes(4 * (p.QueuePosition - 1));
                        ws.EFDuration = 4;
                        //ws.EFAmount = p.EFAmount;
                        ws.EFAmount = p.CurrentCapacity;
                        if (ws.EFStartTime.TimeOfDay < currentIrrigationCalcs.Sunrise.TimeOfDay)
                        {
                            ws.Rollover = 1;
                        }
                        else
                        {
                            ws.Rollover = 0;
                        }
                        //ws.IsCompleted = false;
                        //ws.IsAcknowledged = false;
                        //ws.IsErrorState = false;
                        //ws.CreatedBy = "HITMAN";
                        //ws.CreateDate = DateTime.Now;
                        //ws.ChangedBy = "HITMAN";
                        //ws.ChangeDate = DateTime.Now;
                        //ws.IsActive = true;

                        await AddWateringSchedule(ws);
                    }
                    if (currentIrrigationCalcs.IsEveningSip)
                    {
                        DateTime dt = DateTime.Today;
                        DateTime eveningsplashdt = currentIrrigationCalcs.Sunrise.AddHours(currentIrrigationCalcs.DaylightHoursPerDay).AddMinutes(-4); //growSeason.SunsetTime.AddMinutes(-4);
                        
                        if (DateTime.Now >= currentIrrigationCalcs.SunriseYesterday && DateTime.Now <= currentIrrigationCalcs.SunsetYesterday)
                        {
                            eveningsplashdt = currentIrrigationCalcs.SunriseYesterday.AddHours(currentIrrigationCalcs.DaylightHoursPerDay).AddMinutes(-4);
                        }
                        else
                        {
                            eveningsplashdt = currentIrrigationCalcs.Sunrise.AddHours(currentIrrigationCalcs.DaylightHoursPerDay).AddMinutes(-4);
                        }

                        WateringSchedule ws = new();
                        ws.ID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                        ws.PotID = p.ID;
                        ws.EFStartTime = eveningsplashdt.AddMinutes(-4 * (p.QueuePosition - 1));
                        ws.EFDuration = 4;
                        ws.EFAmount = p.CurrentCapacity;
                        //ws.EFAmount = p.EFAmount;
                        if (ws.EFStartTime.TimeOfDay < currentIrrigationCalcs.Sunrise.TimeOfDay)
                        {
                            ws.Rollover = 1;
                        }
                        else
                        {
                            ws.Rollover = 0;
                        }
                        //ws.IsCompleted = false;
                        //ws.IsAcknowledged = false;
                        //ws.IsErrorState = false;
                        //ws.CreatedBy = "HITMAN";
                        //ws.CreateDate = DateTime.Now;
                        //ws.ChangedBy = "HITMAN";
                        //ws.ChangeDate = DateTime.Now;
                        //ws.IsActive = true;

                        await AddWateringSchedule (ws);
                    }

                }

            }


        }

        public async Task<IEnumerable<WateringSchedule>> GetWateringSchedulesAsync()
        {
            List<WateringSchedule> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetWateringSchedules";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    WateringSchedule wateringSchedule = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        PotID = Guid.Parse(sqlDataReader["potID"].ToString()),
                        PotName = sqlDataReader["potName"].ToString(),
                        PotQueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("potQueuePosition")),
                        EFStartTime = Convert.ToDateTime(sqlDataReader["efStartTime"].ToString()),
                        EFEndTime = Convert.ToDateTime(sqlDataReader["efEndTime"].ToString()),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        //IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged")),
                        //IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted")),
                        //IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState")),
                        //CreatedBy = sqlDataReader["createdBy"].ToString(),
                        //CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        //ChangedBy = sqlDataReader["changedBy"].ToString(),
                        //ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        //IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(wateringSchedule);
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

        public async Task AcknowledgeWateringSchedule(CommandTopic remoteHostCommandViewModel)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAcknowledgeWateringSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("wateringScheduleID", remoteHostCommandViewModel.CommandID.ToString());
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

        public async Task CompleteWateringSchedule(CommandTopic remoteHostCommandViewModel)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spCompleteWateringSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("wateringScheduleID", remoteHostCommandViewModel.CommandID.ToString());
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

        public async Task<FertigationEventCommand> AreWeThereYetAsync()
        {
            FertigationEventCommand wateringScheduleCommand = null;

            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetCurrentWateringScheduleCommand";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_date", DateTime.Now);

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    wateringScheduleCommand = new();
                    //wateringScheduleCommand.CommandID = Guid.Parse(sqlDataReader["commandID"].ToString());
                    wateringScheduleCommand.CommandID = Global.NewSequentialGuid(SequentialGuidType.SequentialAsString);
                    wateringScheduleCommand.PotID = Guid.Parse(sqlDataReader["potID"].ToString());
                    wateringScheduleCommand.PotNumber = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("potNumber"));
                    wateringScheduleCommand.EbbSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ebbSpeed"));
                    wateringScheduleCommand.EbbAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("ebbAmount"));
                    wateringScheduleCommand.EbbAntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbAntiShockRamp"));
                    wateringScheduleCommand.EbbExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("ebbExpectedFlowRate"));
                    wateringScheduleCommand.EbbPumpErrorThreshold = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ebbPumpErrorThreshold"));
                    wateringScheduleCommand.EbbPulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbPulsesPerLiter"));
                    wateringScheduleCommand.SoakDuration = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("soakDuration"));
                    wateringScheduleCommand.FlowSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("flowSpeed"));
                    wateringScheduleCommand.FlowAntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowAntiShockRamp"));
                    wateringScheduleCommand.FlowExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("flowExpectedFlowRate"));
                    wateringScheduleCommand.FlowPumpErrorThreshold = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowPumpErrorThreshold"));
                    wateringScheduleCommand.FlowPulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowPulsesPerLiter"));
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

            //if (wsID != Guid.Empty)
            //{
            //    WateringSchedule ws = await GetWateringScheduleByID(wsID);
            //    wateringScheduleCommand = new();
            //    wateringScheduleCommand.WateringScheduleID = ws.ID;
            //    wateringScheduleCommand.PotNumber = ws.PotQueuePosition;
            //    wateringScheduleCommand.Amount = Convert.ToInt16(ws.EFAmount * 1000);
            //    wateringScheduleCommand.EbbSpeed = await _potDAL.GetEbbSpeedFromQueuePositionAsync(ws.PotQueuePosition);
            //    wateringScheduleCommand.FlowSpeed = await _potDAL.GetFlowSpeedFromQueuePositionAsync(ws.PotQueuePosition);
            //    wateringScheduleCommand.Duration = ws.EFDuration;

            //}

            return wateringScheduleCommand;

        }

        private async Task<WateringSchedule> GetWateringScheduleByID(Guid wsID)
        {
            WateringSchedule wateringSchedule = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetWateringScheduleByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", wsID.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    wateringSchedule.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    wateringSchedule.PotID = Guid.Parse(sqlDataReader["potID"].ToString());
                    wateringSchedule.PotName = sqlDataReader["potName"].ToString();
                    wateringSchedule.PotQueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("potQueuePosition"));
                    wateringSchedule.EFStartTime = Convert.ToDateTime(sqlDataReader["efStartTime"].ToString());
                    wateringSchedule.EFEndTime = Convert.ToDateTime(sqlDataReader["efEndTime"].ToString());
                    wateringSchedule.EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration"));
                    wateringSchedule.EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount"));
                    //wateringSchedule.IsAcknowledged = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isAcknowledged"));
                    //wateringSchedule.IsCompleted = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isCompleted"));
                    //wateringSchedule.IsErrorState = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isErrorState"));
                    //wateringSchedule.CreatedBy = sqlDataReader["createdBy"].ToString();
                    //wateringSchedule.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    //wateringSchedule.ChangedBy = sqlDataReader["changedBy"].ToString();
                    //wateringSchedule.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    //wateringSchedule.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return wateringSchedule;
        }

        public async Task CreateFertigationEventRecord(Guid CommandID)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spCreateFertigationEventRecord";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_commandID", CommandID.ToString());
                sqlCommand.Parameters.AddWithValue("_FEDate", DateTime.Now);

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

        public async Task<bool> VerifyFertigationEventACK(Guid id)
        {
            bool isVerified = false;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spVerifyFertigationEventACK";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_commandID", id.ToString());

                await sqlConnection.OpenAsync();

                isVerified = (int)await sqlCommand.ExecuteScalarAsync() == 1;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return isVerified;
        }

        public async Task UpdateCAFEDateFertigationEventRecord(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);
            Console.WriteLine("Calling sp.... " + id.ToString());
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateCAFEDateFertigationEventRecord";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_commandID", id.ToString());

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

            Console.WriteLine("Ending sp!");
        }

    
    public async Task UpdateFertigationEventRecord(FertigationEventRecord fertigationEventRecord)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateFertigationEventRecord";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_commandID", fertigationEventRecord.CommandID.ToString());
                sqlCommand.Parameters.AddWithValue("_CAFEDate", fertigationEventRecord.CAFEDate);
                sqlCommand.Parameters.AddWithValue("_ebbPump_RunDate", fertigationEventRecord.EbbPump_RunDate);
                sqlCommand.Parameters.AddWithValue("_ebbFlowmeter_DoneDate", fertigationEventRecord.EbbFlowmeter_DoneDate);
                sqlCommand.Parameters.AddWithValue("_ebbPump_DoneDate", fertigationEventRecord.EbbPump_DoneDate);
                sqlCommand.Parameters.AddWithValue("_flowPump_StartDate", fertigationEventRecord.FlowPump_StartDate);
                sqlCommand.Parameters.AddWithValue("_flowPump_RunDate", fertigationEventRecord.FlowPump_RunDate);
                sqlCommand.Parameters.AddWithValue("_flowFlowmeter_DoneDate", fertigationEventRecord.FlowFlowmeter_DoneDate);
                sqlCommand.Parameters.AddWithValue("_flowPump_DoneDate", fertigationEventRecord.FlowPump_DoneDate);
                sqlCommand.Parameters.AddWithValue("_isError", fertigationEventRecord.IsError);
                sqlCommand.Parameters.AddWithValue("_errorDate", fertigationEventRecord.ErrorDate);

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
