using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class LimitTypeDAL
    {
        private readonly string _connectionString;

        public LimitTypeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<LimitType>> GetLimitTypesAsync()
        {
            List<LimitType> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLimitTypes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    LimitType limitType = new()
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
        
        public async Task AddLimitTypeAsync(LimitType limitType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLimitType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", limitType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", limitType.Name);
                sqlCommand.Parameters.AddWithValue("sorting", limitType.Sorting);
                sqlCommand.Parameters.AddWithValue("createdBy", limitType.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", limitType.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", limitType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", limitType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", limitType.IsActive);

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

        public async Task<LimitType> GetLimitTypeByIDAsync(Guid id)
        {
            LimitType limitType = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLimitTypeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return limitType;
        }

        public async Task SaveLimitTypeAsync(LimitType limitType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateLimitType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", limitType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", limitType.Name);
                sqlCommand.Parameters.AddWithValue("thissorting", limitType.Sorting);
                sqlCommand.Parameters.AddWithValue("thischangedBy", limitType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", limitType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", limitType.IsActive);

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

        public async Task DeleteLimitTypeAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteLimitType";
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
