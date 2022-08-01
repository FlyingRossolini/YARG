using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class FeedingChartTypeDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public FeedingChartTypeDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<FeedingChartType> GetFeedingChartTypes()
        {
            List<FeedingChartType> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetFeedingChartTypes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    FeedingChartType feedingChartType = new ()
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

                    lstream.Add(feedingChartType);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddFeedingChartType(FeedingChartType feedingChartType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddFeedingChartType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", feedingChartType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", feedingChartType.Name);
                sqlCmd.Parameters.AddWithValue("sorting", feedingChartType.Sorting);
                sqlCmd.Parameters.AddWithValue("createdBy", feedingChartType.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", feedingChartType.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", feedingChartType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", feedingChartType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", feedingChartType.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public FeedingChartType GetFeedingChartTypeByID(Guid id)
        {
            FeedingChartType feedingChartType = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetFeedingChartTypeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    feedingChartType.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    feedingChartType.Name = sqlDataReader["name"].ToString();
                    feedingChartType.Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"));
                    feedingChartType.CreatedBy = sqlDataReader["createdBy"].ToString();
                    feedingChartType.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    feedingChartType.ChangedBy = sqlDataReader["changedBy"].ToString();
                    feedingChartType.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    feedingChartType.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return feedingChartType;
        }

        public void SaveFeedingChartType(FeedingChartType feedingChartType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateFeedingChartType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", feedingChartType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", feedingChartType.Name);
                sqlCmd.Parameters.AddWithValue("thissorting", feedingChartType.Sorting);
                sqlCmd.Parameters.AddWithValue("thischangedBy", feedingChartType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", feedingChartType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", feedingChartType.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteFeedingChartType(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteFeedingChartType", sqlConnection);
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
