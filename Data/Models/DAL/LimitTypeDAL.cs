using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;

namespace YARG.DAL
{
    public class LimitTypeDAL
    {
        private readonly IConfiguration _config;

        public LimitTypeDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<LimitType> GetLimitTypes()
        {
            List<LimitType> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLimitTypes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    LimitType limitType = new LimitType
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

                    lstream.Add(limitType);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddLimitType(LimitType limitType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddLimitType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", limitType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", limitType.Name);
                sqlCmd.Parameters.AddWithValue("sorting", limitType.Sorting);
                sqlCmd.Parameters.AddWithValue("createdBy", limitType.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", limitType.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", limitType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", limitType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", limitType.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public LimitType GetLimitTypeByID(Guid id)
        {
            LimitType limitType = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLimitTypeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    limitType.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    limitType.Name = sqlDataReader["name"].ToString();
                    limitType.Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"));
                    limitType.CreatedBy = sqlDataReader["createdBy"].ToString();
                    limitType.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    limitType.ChangedBy = sqlDataReader["changedBy"].ToString();
                    limitType.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    limitType.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return limitType;
        }

        public void SaveLimitType(LimitType limitType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateLimitType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", limitType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", limitType.Name);
                sqlCmd.Parameters.AddWithValue("thissorting", limitType.Sorting);
                sqlCmd.Parameters.AddWithValue("thischangedBy", limitType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", limitType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", limitType.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteLimitType(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteLimitType", sqlConnection);
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
