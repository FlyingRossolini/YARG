using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GardenMVC.Common_Types;
using GardenMVC.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace GardenMVC.DAL
{
    public class GrowSeasonDAL
    {
        private readonly IConfiguration _config;

        public GrowSeasonDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public bool FlgActiveGrowSeason()
        {
            bool flg = false;
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spActiveGrowSeason", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    flg = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgActiveGrowSeason"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return flg;
        }

        public Guid IDActiveGrowSeason()
        {
            Guid id = Guid.Empty;

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spIDofActiveGrowSeason", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    id = Guid.Parse(sqlDataReader["id"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return id;
        }

        public IEnumerable<GrowSeason> GetGrowSeasons()
        {
            List<GrowSeason> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetGrowSeasons", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    GrowSeason growSeason = new GrowSeason();
                    growSeason.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    growSeason.Name = sqlDataReader["name"].ToString();
                    if (sqlDataReader["startDate"] != DBNull.Value)
                    {
                        growSeason.StartDate = Convert.ToDateTime(sqlDataReader["startDate"].ToString());
                    }
                    if (sqlDataReader["endDate"] != DBNull.Value)
                    {
                        growSeason.EndDate = Convert.ToDateTime(sqlDataReader["endDate"].ToString());
                    }
                    growSeason.SunriseTime = Convert.ToDateTime(sqlDataReader["sunriseTime"].ToString());
                    growSeason.SunsetTime = Convert.ToDateTime(sqlDataReader["sunsetTime"].ToString());
                    growSeason.CropID = Guid.Parse(sqlDataReader["cropID"].ToString());
                    growSeason.CropName = sqlDataReader["cropName"].ToString();
                    growSeason.FlgAddMorningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddMorningSplash"));
                    growSeason.FlgAddEveningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddEveningSplash"));
                    growSeason.EFEventsPerDay = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("efEventsPerDay"));
                    growSeason.IsComplete = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isComplete"));
                    growSeason.CreatedBy = sqlDataReader["createdBy"].ToString();
                    growSeason.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    growSeason.ChangedBy = sqlDataReader["changedBy"].ToString();
                    growSeason.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    growSeason.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));

                    lstream.Add(growSeason);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return lstream;
        }
   
        public void AddGrowSeason(GrowSeason growSeason)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddGrowSeason", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", growSeason.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", growSeason.Name);
                sqlCmd.Parameters.AddWithValue("startDate", growSeason.StartDate);
                sqlCmd.Parameters.AddWithValue("endDate", growSeason.EndDate);
                sqlCmd.Parameters.AddWithValue("sunriseTime", growSeason.SunriseTime);
                sqlCmd.Parameters.AddWithValue("cropID", Guid.Parse(growSeason.CropID.ToString()));
                sqlCmd.Parameters.AddWithValue("flgAddMorningSplash", growSeason.FlgAddMorningSplash);
                sqlCmd.Parameters.AddWithValue("flgAddEveningSplash", growSeason.FlgAddEveningSplash);
                sqlCmd.Parameters.AddWithValue("efEventsPerDay", growSeason.EFEventsPerDay);
                sqlCmd.Parameters.AddWithValue("isComplete", growSeason.IsComplete);
                sqlCmd.Parameters.AddWithValue("createdBy", growSeason.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", growSeason.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", growSeason.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", growSeason.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", growSeason.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public GrowSeason GetGrowSeasonByID(Guid id)
        {
            GrowSeason growSeason = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetGrowSeasonByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    growSeason.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    growSeason.Name = sqlDataReader["name"].ToString();
                    if (sqlDataReader["startDate"] != DBNull.Value)
                    {
                        growSeason.StartDate = Convert.ToDateTime(sqlDataReader["startDate"].ToString());
                    }
                    if (sqlDataReader["endDate"] != DBNull.Value)
                    {
                        growSeason.EndDate = Convert.ToDateTime(sqlDataReader["endDate"].ToString());
                    }
                    growSeason.SunriseTime = Convert.ToDateTime(sqlDataReader["sunriseTime"].ToString());
                    growSeason.SunsetTime = Convert.ToDateTime(sqlDataReader["sunsetTime"].ToString());
                    growSeason.CropID = Guid.Parse(sqlDataReader["cropID"].ToString());
                    growSeason.CropName = sqlDataReader["cropName"].ToString();
                    growSeason.FlgAddMorningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddMorningSplash"));
                    growSeason.FlgAddEveningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddEveningSplash"));
                    growSeason.EFEventsPerDay = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("efEventsPerDay"));
                    growSeason.IsComplete = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isComplete"));
                    growSeason.CreatedBy = sqlDataReader["createdBy"].ToString();
                    growSeason.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    growSeason.ChangedBy = sqlDataReader["changedBy"].ToString();
                    growSeason.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    growSeason.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return growSeason;

        }
        
        public void SaveGrowSeason(GrowSeason growSeason)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateGrowSeason", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", growSeason.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", growSeason.Name);
                sqlCmd.Parameters.AddWithValue("thisstartDate", growSeason.StartDate);
                sqlCmd.Parameters.AddWithValue("thisendDate", growSeason.EndDate);
                sqlCmd.Parameters.AddWithValue("thissunriseTime", growSeason.SunriseTime);
                sqlCmd.Parameters.AddWithValue("thiscropID", Guid.Parse(growSeason.CropID.ToString()));
                sqlCmd.Parameters.AddWithValue("thisflgAddMorningSplash", growSeason.FlgAddMorningSplash);
                sqlCmd.Parameters.AddWithValue("thisflgAddEveningSplash", growSeason.FlgAddEveningSplash);
                sqlCmd.Parameters.AddWithValue("thisefEventsPerDay", growSeason.EFEventsPerDay);
                sqlCmd.Parameters.AddWithValue("thisisComplete", growSeason.IsComplete);
                sqlCmd.Parameters.AddWithValue("thischangedBy", growSeason.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", growSeason.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", growSeason.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    
        public DateTime GetSunriseToday()
        {
            DateTime dtTemp = DateTime.Today;
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spGetSunriseToday", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCmd.ExecuteReader();
                sqlCmd.Dispose();

                while (sqlDataReader.Read())
                {
                    dtTemp = Convert.ToDateTime(sqlDataReader["sunriseToday"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return dtTemp;
        }
        public DateTime GetSunsetToday()
        {
            DateTime dtTemp = DateTime.Today;
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spGetSunsetToday", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCmd.ExecuteReader();
                sqlCmd.Dispose();

                while (sqlDataReader.Read())
                {
                    dtTemp = Convert.ToDateTime(sqlDataReader["sunsetToday"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return dtTemp;
        }
    }
}
