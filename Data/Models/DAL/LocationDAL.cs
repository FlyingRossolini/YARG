using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class LocationDAL
    {
        private readonly IConfiguration _config;

        public LocationDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            List<Location> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocations";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task AddLocationAsync(Location location)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLocation";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", location.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", location.Name);
                sqlCommand.Parameters.AddWithValue("sorting", location.Sorting);
                sqlCommand.Parameters.AddWithValue("isShowOnLandingPage", location.IsShowOnLandingPage);
                sqlCommand.Parameters.AddWithValue("createdBy", location.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", location.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", location.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", location.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", location.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Location> GetLocationByIDAsync(Guid id)
        {
            Location location = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return location;
        }

        public async Task SaveLocationAsync(Location location)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateLocation";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", location.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thissorting", location.Sorting);
                sqlCommand.Parameters.AddWithValue("thisisShowOnLandingPage", location.IsShowOnLandingPage);
                sqlCommand.Parameters.AddWithValue("thisname", location.Name);
                sqlCommand.Parameters.AddWithValue("thischangedBy", location.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", location.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", location.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteLocation";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<IEnumerable<Location>> GetLocationsForRecipeAsync()
        {
            List<Location> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationsForRecipe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    Location location = new Location
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"))
                    };

                    lstream.Add(location);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }

    }
}
