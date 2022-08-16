using YARG.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class PotDAL
    {
        private readonly IConfiguration _config;
        public PotDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<Pot>> GetPotsAsync()
        {
            List<Pot> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPots";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    Pot pot = new Pot
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task<IEnumerable<Pot>> GetActivePotsAsync()
        {
            List<Pot> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPots";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    Pot pot = new Pot
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }

        public async Task AddPotAsync(Pot pot)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Pot> GetPotByIDAsync(Guid id)
        {
            Pot pot = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPotByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return pot;
        }

        public async Task<short> PotCountAsync()
        {
            short count = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spPotCount";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        count = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("cntPots"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return flgUpdateWateringSchedule;
        }

        public async Task DeletePotAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeletePotsFromWateringSchedule";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeletePot";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFixPotQueuePositions";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<short> GetNextQueuePositionAsync()
        {
            short r = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetNextPotQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        r = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePos"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return r;
        }

        public async Task<short> GetEbbSpeedFromQueuePositionAsync(short queuePos)
        {
            short speed = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetEbbSpeedFromQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisqueuePosition", queuePos);

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        speed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return speed;
        }
        
        public async Task<short> GetFlowSpeedFromQueuePositionAsync(short queuePos)
        {
            short speed = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetFlowSpeedFromQueuePosition";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisqueuePosition", queuePos);

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        speed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return speed;
        }
    }
}
