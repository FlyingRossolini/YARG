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
                        QueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePosition")),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed")),
                        FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed")),
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
                        QueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePosition")),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed")),
                        FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed")),
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
                sqlCommand.Parameters.AddWithValue("id", pot.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", pot.Name);
                sqlCommand.Parameters.AddWithValue("queuePosition", pot.QueuePosition);
                sqlCommand.Parameters.AddWithValue("efDuration", pot.EFDuration);
                sqlCommand.Parameters.AddWithValue("efAmount", pot.EFAmount);
                sqlCommand.Parameters.AddWithValue("ebbSpeed", pot.EbbSpeed);
                sqlCommand.Parameters.AddWithValue("flowSpeed", pot.FlowSpeed);
                sqlCommand.Parameters.AddWithValue("createdBy", pot.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", pot.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", pot.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", pot.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", pot.IsActive);

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
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    pot.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    pot.Name = sqlDataReader["name"].ToString();
                    pot.EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration"));
                    pot.EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount"));
                    pot.EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed"));
                    pot.FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed"));
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

            if(p.IsActive != pot.IsActive ||
                p.EFAmount != pot.EFAmount ||
                p.EFDuration != pot.EFDuration)
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
                sqlCommand.Parameters.AddWithValue("thisid", pot.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", pot.Name);
                sqlCommand.Parameters.AddWithValue("thisefDuration", pot.EFDuration);
                sqlCommand.Parameters.AddWithValue("thisefAmount", pot.EFAmount);
                sqlCommand.Parameters.AddWithValue("thisebbSpeed", pot.EbbSpeed);
                sqlCommand.Parameters.AddWithValue("thisflowSpeed", pot.FlowSpeed);
                sqlCommand.Parameters.AddWithValue("thischangedBy", pot.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", pot.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", pot.IsActive);

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

        public async Task<short> GetNextQueuePositionAsync()
        {
            short r = 0;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetNextPotQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                r = (short)await sqlCommand.ExecuteScalarAsync();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return r;
        }

        public async Task<short> GetEbbSpeedFromQueuePositionAsync(short queuePos)
        {
            short speed = 0;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetEbbSpeedFromQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisqueuePosition", queuePos);

                await sqlConnection.OpenAsync();

                speed = (short)await sqlCommand.ExecuteScalarAsync();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return speed;
        }
        
        public async Task<short> GetFlowSpeedFromQueuePositionAsync(short queuePos)
        {
            short speed = 0;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetFlowSpeedFromQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisqueuePosition", queuePos);

                await sqlConnection.OpenAsync();

                speed = (short)await sqlCommand.ExecuteScalarAsync();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return speed;
        }
    }
}
