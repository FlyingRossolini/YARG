using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.DAL
{
    public class CropDAL
    {
        private readonly IConfiguration _config;

        public CropDAL(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<Crop> GetCrops()
        {
            List<Crop> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetCrops", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Crop crop = new Crop
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddCrop(Crop crop)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddCrop", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", crop.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", crop.Name);
                sqlCmd.Parameters.AddWithValue("createdBy", crop.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", crop.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", crop.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", crop.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", crop.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public Crop GetCropByID(Guid id)
        {
            Crop crop = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetCropByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    crop.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    crop.Name = sqlDataReader["name"].ToString();
                    crop.CreatedBy = sqlDataReader["createdBy"].ToString();
                    crop.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    crop.ChangedBy = sqlDataReader["changedBy"].ToString();
                    crop.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    crop.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return crop;
        }

        public void SaveCrop(Crop crop)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateCrop", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", crop.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", crop.Name);
                sqlCmd.Parameters.AddWithValue("thischangedBy", crop.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", crop.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", crop.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteCrop(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteCrop", sqlConnection);
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
