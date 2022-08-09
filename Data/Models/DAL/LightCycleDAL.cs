using YARG.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace YARG.DAL
{
    public class LightCycleDAL
    {
        private readonly IConfiguration _config;

        public LightCycleDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<LightCycle> GetLightCycles()
        {
            List<LightCycle> lstream = new();

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLightCycles", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return lstream;
        }
    
        public void AddLightCycle(LightCycle lightCycle)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddLightCycle", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", lightCycle.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", lightCycle.Name);
                sqlCmd.Parameters.AddWithValue("daylightHours", lightCycle.DaylightHours);
                sqlCmd.Parameters.AddWithValue("createdBy", lightCycle.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", lightCycle.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", lightCycle.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", lightCycle.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", lightCycle.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public LightCycle GetLightCycleByID(Guid id)
        {
            LightCycle lightCycle = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLightCycleByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lightCycle;
        }
    
        public void SaveLightCycle(LightCycle lightCycle)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateLightCycle", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", lightCycle.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", lightCycle.Name);
                sqlCmd.Parameters.AddWithValue("thisdaylightHours", lightCycle.DaylightHours);
                sqlCmd.Parameters.AddWithValue("thischangedBy", lightCycle.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", lightCycle.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", lightCycle.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    
        public void DeleteLightCycle(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteLightCycle", sqlConnection);
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
