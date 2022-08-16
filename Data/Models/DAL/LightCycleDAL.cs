using YARG.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class LightCycleDAL
    {
        private readonly IConfiguration _config;

        public LightCycleDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<LightCycle>> GetLightCyclesAsync()
        {
            List<LightCycle> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLightCycles";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    LightCycle lightCycle = new LightCycle
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        DaylightHours = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("daylightHours")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(lightCycle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
    
        public async Task AddLightCycleAsync(LightCycle lightCycle)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLightCycle";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", lightCycle.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", lightCycle.Name);
                sqlCommand.Parameters.AddWithValue("daylightHours", lightCycle.DaylightHours);
                sqlCommand.Parameters.AddWithValue("createdBy", lightCycle.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", lightCycle.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", lightCycle.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", lightCycle.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", lightCycle.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<LightCycle> GetLightCycleByIDAsync(Guid id)
        {
            LightCycle lightCycle = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLightCycleByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        lightCycle.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        lightCycle.Name = sqlDataReader["name"].ToString();
                        lightCycle.DaylightHours = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("daylightHours"));
                        lightCycle.CreatedBy = sqlDataReader["createdBy"].ToString();
                        lightCycle.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        lightCycle.ChangedBy = sqlDataReader["changedBy"].ToString();
                        lightCycle.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        lightCycle.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lightCycle;
        }
    
        public async Task SaveLightCycleAsync(LightCycle lightCycle)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateLightCycle";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", lightCycle.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", lightCycle.Name);
                sqlCommand.Parameters.AddWithValue("thisdaylightHours", lightCycle.DaylightHours);
                sqlCommand.Parameters.AddWithValue("thischangedBy", lightCycle.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", lightCycle.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", lightCycle.IsActive);


                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteLightCycleAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteLightCycle";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
