﻿using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class MeasurementDAL
    {
        private readonly string _connectionString;

        public MeasurementDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }
        
        public async Task AddMeasurementAsync(Measurement measurement)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddMeasurement";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", measurement.ID.ToString());
                sqlCommand.Parameters.AddWithValue("locationID", measurement.LocationID.ToString());
                sqlCommand.Parameters.AddWithValue("measurementTypeID", measurement.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("measuredValue", measurement.MeasuredValue);
                sqlCommand.Parameters.AddWithValue("createdBy", measurement.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", measurement.CreateDate);

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

        public async Task<Measurement> GetMeasurementByIDAsync(Guid id)
        {
            Measurement measurement = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurementByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    measurement.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    measurement.LocationID = Guid.Parse(sqlDataReader["locationID"].ToString());
                    measurement.LocationName = sqlDataReader["locationName"].ToString();
                    measurement.MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString());
                    measurement.MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString();
                    measurement.MeasuredValue = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("measuredValue"));
                    measurement.CreatedBy = sqlDataReader["createdBy"].ToString();
                    measurement.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
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

            return measurement;
        }
        
        public async Task<IEnumerable<Measurement>> GetMeasurementsAsync()
        {
            List<Measurement> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurements";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    Measurement measurement = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        LocationID = Guid.Parse(sqlDataReader["locationID"].ToString()),
                        LocationName = sqlDataReader["locationName"].ToString(),
                        MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString()),
                        MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString(),
                        MeasuredValue = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("measuredValue")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                    };

                    lstream.Add(measurement);
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

    }
}
