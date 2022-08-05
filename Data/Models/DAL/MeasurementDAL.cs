using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;
using GardenMVC.Data.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.DAL
{
    public class MeasurementDAL
    {
        private readonly IConfiguration _config;

        public MeasurementDAL(IConfiguration configuration)
        {
            _config = configuration;
        }
        
        public void AddMeasurement(Measurement measurement)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddMeasurement", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", measurement.ID.ToString());
                sqlCmd.Parameters.AddWithValue("locationID", measurement.LocationID.ToString());
                sqlCmd.Parameters.AddWithValue("measurementTypeID", measurement.MeasurementTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("measuredValue", measurement.MeasuredValue);
                sqlCmd.Parameters.AddWithValue("createdBy", measurement.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", measurement.CreateDate);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public Measurement GetMeasurementByID(Guid id)
        {
            Measurement measurement = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMeasurementByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return measurement;
        }
        public IEnumerable<Measurement> GetMeasurements()
        {
            List<Measurement> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMeasurements", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Measurement measurement = new Measurement
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }

    }
}
