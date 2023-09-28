using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class GrowSeasonDAL
    {
        private readonly string _connectionString;

        public GrowSeasonDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<bool> FlgActiveGrowSeasonAsync()
        {
            bool flg = false;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spActiveGrowSeason";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                flg = (bool)await sqlCommand.ExecuteScalarAsync();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return flg;
        }

        public async Task<Guid> IDActiveGrowSeasonAsync()
        {
            Guid id = Guid.Empty;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spIDofActiveGrowSeason";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                id = Guid.Parse((string)await sqlCommand.ExecuteScalarAsync());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return id;
        }

        public async Task<IEnumerable<GrowSeason>> GetGrowSeasonsAsync()
        {
            List<GrowSeason> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetGrowSeasons";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    GrowSeason growSeason = new();
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
                    growSeason.CropID = Guid.Parse(sqlDataReader["cropID"].ToString());
                    growSeason.CropName = sqlDataReader["cropName"].ToString();
                    //growSeason.FlgAddMorningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddMorningSplash"));
                    //growSeason.FlgAddEveningSplash = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("flgAddEveningSplash"));
                    //growSeason.EFEventsPerDay = sqlDataReader.GetByte(sqlDataReader.GetOrdinal("efEventsPerDay"));
                    growSeason.IsComplete = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isComplete"));
                    growSeason.CreatedBy = sqlDataReader["createdBy"].ToString();
                    growSeason.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    growSeason.ChangedBy = sqlDataReader["changedBy"].ToString();
                    growSeason.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    growSeason.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));

                    lstream.Add(growSeason);
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
   
        public async Task AddGrowSeasonAsync(GrowSeason growSeason)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddGrowSeason";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", growSeason.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", growSeason.Name);
                sqlCommand.Parameters.AddWithValue("startDate", growSeason.StartDate);
                sqlCommand.Parameters.AddWithValue("endDate", growSeason.EndDate);
                sqlCommand.Parameters.AddWithValue("sunriseTime", growSeason.SunriseTime);
                sqlCommand.Parameters.AddWithValue("cropID", Guid.Parse(growSeason.CropID.ToString()));
                sqlCommand.Parameters.AddWithValue("recipeID", Guid.Parse(growSeason.RecipeID.ToString()));
                sqlCommand.Parameters.AddWithValue("isComplete", growSeason.IsComplete);
                sqlCommand.Parameters.AddWithValue("createdBy", growSeason.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", growSeason.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", growSeason.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", growSeason.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", growSeason.IsActive);

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

        public async Task<GrowSeason> GetGrowSeasonByIDAsync(Guid id)
        {
            GrowSeason growSeason = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetGrowSeasonByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
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
                    growSeason.CropID = Guid.Parse(sqlDataReader["cropID"].ToString());
                    growSeason.CropName = sqlDataReader["cropName"].ToString();
                    growSeason.RecipeName = sqlDataReader["recipeName"].ToString();
                    growSeason.IsComplete = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isComplete"));
                    growSeason.CreatedBy = sqlDataReader["createdBy"].ToString();
                    growSeason.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    growSeason.ChangedBy = sqlDataReader["changedBy"].ToString();
                    growSeason.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    growSeason.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return growSeason;

        }
        
        public async Task<CurrentIrrigationCalcs> GetCurrentIrrigationCalcs()
        {
            CurrentIrrigationCalcs currentIrrigationCalcs = new();
            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spCurrentIrrigationCalcs";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    currentIrrigationCalcs.GrowName = sqlDataReader["growName"].ToString();
                    currentIrrigationCalcs.GrowDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("growDay"));
                    currentIrrigationCalcs.GrowWeek = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("growWeek"));
                    currentIrrigationCalcs.IsDay = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isDay"));
                        currentIrrigationCalcs.Sunrise = Convert.ToDateTime(sqlDataReader["sunrise"].ToString());
                        currentIrrigationCalcs.Sunset = Convert.ToDateTime(sqlDataReader["sunset"].ToString());
                    currentIrrigationCalcs.SunriseYesterday = Convert.ToDateTime(sqlDataReader["sunriseYesterday"].ToString());
                    currentIrrigationCalcs.SunsetYesterday = Convert.ToDateTime(sqlDataReader["sunsetYesterday"].ToString());
                    currentIrrigationCalcs.IrrigationEventsPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("irrigationEventsPerDay"));
                    currentIrrigationCalcs.SoakTime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("soakTime"));
                    currentIrrigationCalcs.IsMorningSip = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isMorningSip"));
                    currentIrrigationCalcs.IsEveningSip = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isEveningSip"));
                    currentIrrigationCalcs.DaylightHoursPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("daylightPerDay"));
                    currentIrrigationCalcs.RecipeID = Guid.Parse(sqlDataReader["recipeID"].ToString());
                    currentIrrigationCalcs.RecipeName = sqlDataReader["recipeName"].ToString();
                    currentIrrigationCalcs.CropName = sqlDataReader["cropName"].ToString();
                    currentIrrigationCalcs.GrowSeasonID = Guid.Parse(sqlDataReader["growSeasonID"].ToString());
                    currentIrrigationCalcs.GrowRoomTemp = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("growRoomTemp"));
                    currentIrrigationCalcs.GrowRoomHumidity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("growRoomHumidity"));
                    currentIrrigationCalcs.ReservoirTemp = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("reservoirTemp"));
                    currentIrrigationCalcs.ReservoirVolume = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("reservoirVolume"));
                    currentIrrigationCalcs.WeatherText = sqlDataReader["weatherText"].ToString();
                    currentIrrigationCalcs.GrowStartDate = Convert.ToDateTime(sqlDataReader["growStartDate"].ToString());
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

            return currentIrrigationCalcs;
        }
        public async Task SaveGrowSeasonAsync(GrowSeason growSeason)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateGrowSeason";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", growSeason.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", growSeason.Name);
                sqlCommand.Parameters.AddWithValue("thisstartDate", growSeason.StartDate);
                sqlCommand.Parameters.AddWithValue("thisendDate", growSeason.EndDate);
                sqlCommand.Parameters.AddWithValue("thissunriseTime", growSeason.SunriseTime);
                sqlCommand.Parameters.AddWithValue("thiscropID", Guid.Parse(growSeason.CropID.ToString()));
                sqlCommand.Parameters.AddWithValue("thisrecipeID", Guid.Parse("3a03214d-cd4d-11e1-be8b-4da4c84a2644"));
                sqlCommand.Parameters.AddWithValue("thisisComplete", growSeason.IsComplete);
                sqlCommand.Parameters.AddWithValue("thischangedBy", growSeason.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", growSeason.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", growSeason.IsActive);

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

        public async Task<DateTime> GetSunriseTodayAsync()
        {
            DateTime dtTemp = DateTime.Today;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetSunriseToday";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                dtTemp = (DateTime)await sqlCommand.ExecuteScalarAsync();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return dtTemp;
        }
        
        public async Task<DateTime> GetSunsetToday()
        {
            DateTime dtTemp = DateTime.Today;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetSunsetToday";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                dtTemp = (DateTime)await sqlCommand.ExecuteScalarAsync();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return dtTemp;
        }
    }
}
