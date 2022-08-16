using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class JarDAL
    {
        private readonly IConfiguration _config;

        public JarDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<Jar>> GetJarsAsync()
        {
            List<Jar> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetJars";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        Jar jar = new()
                        {
                            ID = Guid.Parse(sqlDataReader["id"].ToString()),
                            MixFanPosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanPosition")),
                            ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString()),
                            ChemicalName = sqlDataReader["chemicalName"].ToString(),
                            MixTimesPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTimesPerDay")),
                            Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("duration")),
                            Capacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("capacity")),
                            CurrentAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentAmount")),
                            MixFanOverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed")),
                            CreatedBy = sqlDataReader["createdBy"].ToString(),
                            CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                            ChangedBy = sqlDataReader["changedBy"].ToString(),
                            ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                            IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                        };

                        lstream.Add(jar);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }

        public async Task AddJarAsync(Jar jar)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddJar";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", jar.ID.ToString());
                sqlCommand.Parameters.AddWithValue("mixFanPosition", jar.MixFanPosition);
                sqlCommand.Parameters.AddWithValue("chemicalID", jar.ChemicalID.ToString());
                sqlCommand.Parameters.AddWithValue("mixTimesPerDay", jar.MixTimesPerDay);
                sqlCommand.Parameters.AddWithValue("duration", jar.Duration);
                sqlCommand.Parameters.AddWithValue("capacity", jar.Capacity);
                sqlCommand.Parameters.AddWithValue("currentAmount", jar.CurrentAmount);
                sqlCommand.Parameters.AddWithValue("mixFanOverSpeed", jar.MixFanOverSpeed);
                sqlCommand.Parameters.AddWithValue("createdBy", jar.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", jar.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", jar.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", jar.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", jar.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<Jar> GetJarByIDAsync(Guid id)
        {
            Jar jar = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetJarByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        jar.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        jar.MixFanPosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanPosition"));
                        jar.ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString());
                        jar.ChemicalName = sqlDataReader["chemicalName"].ToString();
                        jar.MixTimesPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTimesPerDay"));
                        jar.Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("duration"));
                        jar.Capacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("capacity"));
                        jar.CurrentAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentAmount"));
                        jar.MixFanOverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed"));
                        jar.CreatedBy = sqlDataReader["createdBy"].ToString();
                        jar.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        jar.ChangedBy = sqlDataReader["changedBy"].ToString();
                        jar.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        jar.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return jar;
        }

        public async Task<bool> SaveJarAsync(Jar jar)
        {
            Jar j = await GetJarByIDAsync(jar.ID);

            bool flgUpdateMixingFanSchedule = false;

            if (j.IsActive != jar.IsActive ||
                j.MixTimesPerDay != jar.MixTimesPerDay ||
                j.Duration != jar.Duration)
            {
                flgUpdateMixingFanSchedule = true;
            }

            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateJar";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", jar.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thismixFanPosition", jar.MixFanPosition);
                sqlCommand.Parameters.AddWithValue("thischemicalID", jar.ChemicalID.ToString());
                sqlCommand.Parameters.AddWithValue("thismixTimesPerDay", jar.MixTimesPerDay);
                sqlCommand.Parameters.AddWithValue("thisduration", jar.Duration);
                sqlCommand.Parameters.AddWithValue("thiscapacity", jar.Capacity);
                sqlCommand.Parameters.AddWithValue("thiscurrentAmount", jar.CurrentAmount);
                sqlCommand.Parameters.AddWithValue("thismixFanOverSpeed", jar.MixFanOverSpeed);
                sqlCommand.Parameters.AddWithValue("thischangedBy", jar.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", jar.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", jar.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return flgUpdateMixingFanSchedule;
        }

        public async Task DeleteJarAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteJar";
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
