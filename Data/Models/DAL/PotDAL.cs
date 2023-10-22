using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class PotDAL
    {
        private readonly string _connectionString;

        public PotDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<Pot>> GetPotsAsync()
        {
            List<Pot> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPots";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    Pot pot = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        QueuePosition = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("queuePosition")),
                        //EFDuration = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("efDuration")),
                        //EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        //EbbSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ebbSpeed")),
                        //FlowSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("flowSpeed")),
                        TotalCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalCapacity")),
                        Speed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("speed")),
                        CurrentCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentCapacity")),
                        AntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("antiShockRamp")),
                        ExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("expectedFlowRate")),
                        PumpFlowErrorThreshold = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("pumpFlowErrorThreshold")),
                        PulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pulsesPerLiter")),
                        IsReservoir = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isReservoir")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(pot);
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
        
        public async Task<IEnumerable<Pot>> GetActivePotsAsync()
        {
            List<Pot> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPots";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    Pot pot = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        QueuePosition = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("queuePosition")),
                        //EFDuration = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("efDuration")),
                        //EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        //EbbSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("ebbSpeed")),
                        //FlowSpeed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("flowSpeed")),
                        TotalCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalCapacity")),
                        Speed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("speed")),
                        CurrentCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentCapacity")),
                        AntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("antiShockRamp")),
                        ExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("expectedFlowRate")),
                        PumpFlowErrorThreshold = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("pumpFlowErrorThreshold")),
                        PulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pulsesPerLiter")),
                        IsReservoir = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isReservoir")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    if (pot.IsActive)
                    {
                        lstream.Add(pot);
                    }
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

        public async Task AddPotAsync(Pot pot)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddPot";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", pot.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_name", pot.Name);
                //sqlCommand.Parameters.AddWithValue("efDuration", pot.EFDuration);
                //sqlCommand.Parameters.AddWithValue("efAmount", pot.EFAmount);
                //sqlCommand.Parameters.AddWithValue("ebbSpeed", pot.EbbSpeed);
                //sqlCommand.Parameters.AddWithValue("flowSpeed", pot.FlowSpeed);
                //TotalCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalCapacity")),
                //        Speed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("speed")),
                //        CurrentCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentCapacity")),
                //        AntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("antiShockRamp")),
                //        ExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("expectedFlowRate")),
                //        PumpFlowErrorThreshold = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("pumpFlowErrorThreshold")),
                //        PulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pulsesPerLiter")),
                //        IsReservoir = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isReservoir")),
                sqlCommand.Parameters.AddWithValue("_totalCapacity", pot.TotalCapacity);
                sqlCommand.Parameters.AddWithValue("_speed", pot.Speed);
                sqlCommand.Parameters.AddWithValue("_currentCapacity", pot.CurrentCapacity);
                sqlCommand.Parameters.AddWithValue("_antiShockRamp", pot.AntiShockRamp);
                sqlCommand.Parameters.AddWithValue("_expectedFlowRate", pot.ExpectedFlowRate);
                sqlCommand.Parameters.AddWithValue("_pumpFlowErrorThreshold", pot.PumpFlowErrorThreshold);
                sqlCommand.Parameters.AddWithValue("_pulsesPerLiter", pot.PulsesPerLiter);
                sqlCommand.Parameters.AddWithValue("_isReservoir", pot.IsReservoir);
                sqlCommand.Parameters.AddWithValue("_createdBy", pot.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", pot.CreateDate);
                sqlCommand.Parameters.AddWithValue("_changedBy", pot.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", pot.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_isActive", pot.IsActive);

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

        public async Task<Pot> GetPotByIDAsync(Guid id)
        {
            Pot pot = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPotByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    pot.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    pot.Name = sqlDataReader["name"].ToString();
                    pot.TotalCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalCapacity"));
                    pot.Speed = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("speed"));
                    pot.CurrentCapacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentCapacity"));
                    pot.AntiShockRamp = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("antiShockRamp"));
                    pot.ExpectedFlowRate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("expectedFlowRate"));
                    pot.PumpFlowErrorThreshold = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("pumpFlowErrorThreshold"));
                    pot.PulsesPerLiter = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("pulsesPerLiter"));
                    pot.IsReservoir = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isReservoir"));
                    pot.CreatedBy = sqlDataReader["createdBy"].ToString();
                    pot.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    pot.ChangedBy = sqlDataReader["changedBy"].ToString();
                    pot.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    pot.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return pot;
        }

        public async Task<short> PotCountAsync()
        {
            short count = 0;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spPotCount";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                //await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                //while (await sqlDataReader.ReadAsync())
                //{
                //    count = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("cntPots"));
                //}

                count = Convert.ToInt16((long)await sqlCommand.ExecuteScalarAsync());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return count;
        }
        
        public async Task<bool> SavePotAsync(Pot pot)
        {
            Pot p = await GetPotByIDAsync(pot.ID);
            bool flgUpdateWateringSchedule = false;

            if(p.IsActive != pot.IsActive 
                //|| p.EFAmount != pot.EFAmount ||
                //p.EFDuration != pot.EFDuration
                )
            {
                flgUpdateWateringSchedule = true;
            }

            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdatePot";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", pot.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_name", pot.Name);
                sqlCommand.Parameters.AddWithValue("_totalCapacity", pot.TotalCapacity);
                sqlCommand.Parameters.AddWithValue("_speed", pot.Speed);
                sqlCommand.Parameters.AddWithValue("_currentCapacity", pot.CurrentCapacity);
                sqlCommand.Parameters.AddWithValue("_antiShockRamp", pot.AntiShockRamp);
                sqlCommand.Parameters.AddWithValue("_expectedFlowRate", pot.ExpectedFlowRate);
                sqlCommand.Parameters.AddWithValue("_pumpFlowErrorThreshold", pot.PumpFlowErrorThreshold);
                sqlCommand.Parameters.AddWithValue("_pulsesPerLiter", pot.PulsesPerLiter);
                sqlCommand.Parameters.AddWithValue("_isReservoir", pot.IsReservoir);
                sqlCommand.Parameters.AddWithValue("_changedBy", pot.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", pot.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_isActive", pot.IsActive);

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

            return flgUpdateWateringSchedule;
        }

        public async Task DeletePotAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeletePotsFromWateringSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

                using MySqlCommand sqlCommand2 = new();
                sqlCommand2.Connection = sqlConnection;
                sqlCommand2.CommandText = "spDeletePot";
                sqlCommand2.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand2.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand2.ExecuteNonQueryAsync();

                using MySqlCommand sqlCommand3 = new();
                sqlCommand3.Connection = sqlConnection;
                sqlCommand3.CommandText = "spFixPotQueuePositions";
                sqlCommand3.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await sqlCommand3.ExecuteNonQueryAsync();

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

        //public async Task<byte> GetNextQueuePositionAsync()
        //{
        //    byte r = 0;
        //    using MySqlConnection sqlConnection = new(_connectionString);

        //    try
        //    {
        //        using MySqlCommand sqlCommand = new();
        //        sqlCommand.Connection = sqlConnection;
        //        sqlCommand.CommandText = "spGetNextPotQueuePosition";
        //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

        //        await sqlConnection.OpenAsync();

        //        r = (byte)await sqlCommand.ExecuteScalarAsync();

        //    }
        //    catch (MySqlException ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //    finally
        //    {
        //        await sqlConnection.CloseAsync();
        //    }

        //    return r;
        //}

    }
}
