using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class LimitDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public LimitDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<Limit> GetLimitsByParentId(Guid parentId)
        {
            List<Limit> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLimitsByParentId", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", parentId.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Limit limit = new Limit
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        ParentID = Guid.Parse(sqlDataReader["parentId"].ToString()),
                        MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeId"].ToString()),
                        MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString(),
                        LimitTypeID = Guid.Parse(sqlDataReader["limitTypeId"].ToString()),
                        LimitTypeName= sqlDataReader["limitTypeName"].ToString(),
                        LimitValue = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("limitValue")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                    };

                    lstream.Add(limit);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddLimit(Limit limit)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddLimit", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", limit.ID.ToString());
                sqlCmd.Parameters.AddWithValue("parentId", limit.ParentID.ToString());
                sqlCmd.Parameters.AddWithValue("measurementTypeId", limit.MeasurementTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("limitTypeId", limit.LimitTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("limitValue", limit.LimitValue.ToString());
                sqlCmd.Parameters.AddWithValue("createdBy", limit.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", limit.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", limit.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", limit.ChangeDate);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public void SaveLimit(Limit limit)
        {

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateLimit", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", limit.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thismeasurementTypeId", limit.MeasurementTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("thislimitValue", limit.LimitValue.ToString());
                sqlCmd.Parameters.AddWithValue("thischangedBy", limit.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", limit.ChangeDate);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public void DeleteJar(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteJar", sqlConnection);
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
