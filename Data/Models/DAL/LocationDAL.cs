using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class LocationDAL
    {
        private readonly string _connectionString;

        public LocationDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            List<Location> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocations";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    Location location = new()
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
        
        public async Task AddLocationAsync(Location location)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }
        public async Task<Guid> GetLocationIDByProbeAddressAsync(string probeAddress)
        {
            Guid guid = new();

            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationByProbeAddress";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_probeAddress", probeAddress);

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    guid = Guid.Parse(sqlDataReader["id"].ToString());
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

            return guid;
        }

        public async Task<Location> GetLocationByIDAsync(Guid id)
        {
            Location location = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return location;
        }

        public async Task SaveLocationAsync(Location location)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
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
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }

        public async Task DeleteLocationAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteLocation";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

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

        public async Task<IEnumerable<Location>> GetLocationsForRecipeAsync()
        {
            List<Location> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationsForRecipe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    Location location = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"))
                    };

                    lstream.Add(location);
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

    }
}
