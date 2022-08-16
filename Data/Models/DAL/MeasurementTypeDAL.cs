using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class MeasurementTypeDAL
    {
        private readonly IConfiguration _config;

        public MeasurementTypeDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<MeasurementType>> GetMeasurementTypesAsync()
        {
            List<MeasurementType> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurementTypes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    MeasurementType measurementType = new MeasurementType
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(measurementType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task AddMeasurementTypeAsync(MeasurementType measurementType)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddMeasurementType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", measurementType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", measurementType.Name);
                sqlCommand.Parameters.AddWithValue("sorting", measurementType.Sorting);
                sqlCommand.Parameters.AddWithValue("createdBy", measurementType.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", measurementType.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", measurementType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", measurementType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", measurementType.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<MeasurementType> GetMeasurementTypeByIDAsync(Guid id)
        {
            MeasurementType measurementType = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurementTypeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        measurementType.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        measurementType.Name = sqlDataReader["name"].ToString();
                        measurementType.Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"));
                        measurementType.CreatedBy = sqlDataReader["createdBy"].ToString();
                        measurementType.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        measurementType.ChangedBy = sqlDataReader["changedBy"].ToString();
                        measurementType.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        measurementType.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return measurementType;
        }

        public async Task SaveMeasurementTypeAsync(MeasurementType measurementType)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateMeasurementType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", measurementType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", measurementType.Name);
                sqlCommand.Parameters.AddWithValue("thissorting", measurementType.Sorting);
                sqlCommand.Parameters.AddWithValue("thischangedBy", measurementType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", measurementType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", measurementType.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteMeasurementTypeAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteMeasurementType";
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
