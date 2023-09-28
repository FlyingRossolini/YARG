using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class LimitDAL
    {
        private readonly string _connectionString;

        public LimitDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<Limit>> GetLimitsByParentIdAsync(Guid parentId)
        {
            List<Limit> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLimitsByParentId";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", parentId.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    Limit limit = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        ParentID = Guid.Parse(sqlDataReader["parentId"].ToString()),
                        MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeId"].ToString()),
                        MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString(),
                        LimitTypeID = Guid.Parse(sqlDataReader["limitTypeId"].ToString()),
                        LimitTypeName = sqlDataReader["limitTypeName"].ToString(),
                        LimitValue = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("limitValue")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                    };

                    lstream.Add(limit);
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
        
        public async Task AddLimitAsync(Limit limit)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLimit";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", limit.ID.ToString());
                sqlCommand.Parameters.AddWithValue("parentId", limit.ParentID.ToString());
                sqlCommand.Parameters.AddWithValue("measurementTypeId", limit.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("limitTypeId", limit.LimitTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("limitValue", limit.LimitValue.ToString());
                sqlCommand.Parameters.AddWithValue("createdBy", limit.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", limit.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", limit.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", limit.ChangeDate);

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

        public async Task SaveLimitAsync(Limit limit)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateLimit";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", limit.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thismeasurementTypeId", limit.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("thislimitValue", limit.LimitValue.ToString());
                sqlCommand.Parameters.AddWithValue("thischangedBy", limit.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", limit.ChangeDate);

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

        public async Task DeleteLimitAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteLimit";
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
