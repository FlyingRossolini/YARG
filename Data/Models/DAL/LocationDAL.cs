using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class LocationDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public LocationDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<Location> GetLocations()
        {
            List<Location> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLocations", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Location location = new Location
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting")),
                        IsShowOnLandingPage = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isShowOnLandingPage")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(location);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddLocation(Location location)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddLocation", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", location.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", location.Name);
                sqlCmd.Parameters.AddWithValue("sorting", location.Sorting);
                sqlCmd.Parameters.AddWithValue("isShowOnLandingPage", location.IsShowOnLandingPage);
                sqlCmd.Parameters.AddWithValue("createdBy", location.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", location.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", location.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", location.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", location.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public Location GetLocationByID(Guid id)
        {
            Location location = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLocationByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    location.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    location.Name = sqlDataReader["name"].ToString();
                    location.Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"));
                    location.IsShowOnLandingPage = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isShowOnLandingPage"));
                    location.CreatedBy = sqlDataReader["createdBy"].ToString();
                    location.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    location.ChangedBy = sqlDataReader["changedBy"].ToString();
                    location.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    location.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return location;
        }

        public void SaveLocation(Location location)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateLocation", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", location.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thissorting", location.Sorting);
                sqlCmd.Parameters.AddWithValue("thisisShowOnLandingPage", location.IsShowOnLandingPage);
                sqlCmd.Parameters.AddWithValue("thisname", location.Name);
                sqlCmd.Parameters.AddWithValue("thischangedBy", location.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", location.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", location.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteLocation(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteLocation", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public IEnumerable<Location> GetLocationsForRecipe()
        {
            List<Location> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLocationsForRecipe", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Location location = new Location
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"))
                    };

                    lstream.Add(location);
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
