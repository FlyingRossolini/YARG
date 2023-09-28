using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class FeedingChartTypeDAL
    {
        private readonly string _connectionString;

        public FeedingChartTypeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<FeedingChartType>> GetFeedingChartTypesAsync()
        {
            List<FeedingChartType> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetFeedingChartTypes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    FeedingChartType feedingChartType = new()
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
        
        public async Task AddFeedingChartTypeAsync(FeedingChartType feedingChartType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddFeedingChartType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", feedingChartType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", feedingChartType.Name);
                sqlCommand.Parameters.AddWithValue("sorting", feedingChartType.Sorting);
                sqlCommand.Parameters.AddWithValue("createdBy", feedingChartType.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", feedingChartType.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", feedingChartType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", feedingChartType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", feedingChartType.IsActive);

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

        public async Task<FeedingChartType> GetFeedingChartTypeByIDAsync(Guid id)
        {
            FeedingChartType feedingChartType = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetFeedingChartTypeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return feedingChartType;
        }

        public async Task SaveFeedingChartTypeAsync(FeedingChartType feedingChartType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateFeedingChartType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", feedingChartType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", feedingChartType.Name);
                sqlCommand.Parameters.AddWithValue("thissorting", feedingChartType.Sorting);
                sqlCommand.Parameters.AddWithValue("thischangedBy", feedingChartType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", feedingChartType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", feedingChartType.IsActive);

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

        public async Task DeleteFeedingChartTypeAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteFeedingChartType";
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
    }
}
