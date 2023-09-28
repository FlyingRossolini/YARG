using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class CropDAL
    {
        private readonly string _connectionString;

        public CropDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<Crop>> GetCropsAsync()
        {
            List<Crop> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetCrops";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    Crop crop = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(crop);
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
        
        public async Task AddCropAsync(Crop crop)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddCrop";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", crop.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", crop.Name);
                sqlCommand.Parameters.AddWithValue("createdBy", crop.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", crop.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", crop.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", crop.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", crop.IsActive);

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

        public async Task<Crop> GetCropByIDAsync(Guid id)
        {
            Crop crop = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetCropByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    crop.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    crop.Name = sqlDataReader["name"].ToString();
                    crop.CreatedBy = sqlDataReader["createdBy"].ToString();
                    crop.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    crop.ChangedBy = sqlDataReader["changedBy"].ToString();
                    crop.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    crop.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return crop;
        }

        public async Task SaveCrop(Crop crop)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateCrop";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", crop.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", crop.Name);
                sqlCommand.Parameters.AddWithValue("thischangedBy", crop.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", crop.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", crop.IsActive);

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

        public async Task DeleteCrop(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteCrop";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

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
