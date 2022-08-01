using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class MeasurementTypeDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public MeasurementTypeDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<MeasurementType> GetMeasurementTypes()
        {
            List<MeasurementType> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMeasurementTypes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddMeasurementType(MeasurementType measurementType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddMeasurementType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", measurementType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", measurementType.Name);
                sqlCmd.Parameters.AddWithValue("sorting", measurementType.Sorting);
                sqlCmd.Parameters.AddWithValue("createdBy", measurementType.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", measurementType.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", measurementType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", measurementType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", measurementType.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public MeasurementType GetMeasurementTypeByID(Guid id)
        {
            MeasurementType measurementType = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMeasurementTypeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return measurementType;
        }

        public void SaveMeasurementType(MeasurementType measurementType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateMeasurementType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", measurementType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", measurementType.Name);
                sqlCmd.Parameters.AddWithValue("thissorting", measurementType.Sorting);
                sqlCmd.Parameters.AddWithValue("thischangedBy", measurementType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", measurementType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", measurementType.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteMeasurementType(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteMeasurementType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}
