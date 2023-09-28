using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Data.Models.MqttTopics;
using YARG.Models;

namespace YARG.DAL
{
    public class RemoteProbeDAL
    {
        private readonly string _connectionString;

        public RemoteProbeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<RemoteProbe>> GetRemoteProbesAsync()
        {
            List<RemoteProbe> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRemoteProbes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    RemoteProbe remoteProbe = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        LocationID = Guid.Parse(sqlDataReader["locationID"].ToString()),
                        LocationName = sqlDataReader["locationName"].ToString(),
                        MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString()),
                        MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString(),
                        RemoteProbeAddress = sqlDataReader["remoteProbeAddress"].ToString(),
                        LtLCLCommand = sqlDataReader["ltLCLCommand"].ToString(),
                        GtUCLCommand = sqlDataReader["gtUCLCommand"].ToString(),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(remoteProbe);
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
        
        public async Task AddRemoteProbeAsync(RemoteProbe remoteProbe)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddRemoteProbe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", remoteProbe.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_locationID", remoteProbe.LocationID.ToString());
                sqlCommand.Parameters.AddWithValue("_measurementTypeID", remoteProbe.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("_remoteProbeAddress", remoteProbe.RemoteProbeAddress);
                sqlCommand.Parameters.AddWithValue("_ltLCLCommand", remoteProbe.LtLCLCommand);
                sqlCommand.Parameters.AddWithValue("_gtUCLCommand", remoteProbe.GtUCLCommand);
                sqlCommand.Parameters.AddWithValue("createdBy", remoteProbe.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", remoteProbe.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", remoteProbe.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", remoteProbe.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", remoteProbe.IsActive);

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

        public async Task<RemoteProbe> GetRemoteProbeByIDAsync(Guid id)
        {
            RemoteProbe remoteProbe = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRemoteProbeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                
                while (await sqlDataReader.ReadAsync())
                {
                    remoteProbe.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    remoteProbe.LocationID = Guid.Parse(sqlDataReader["locationID"].ToString());
                    remoteProbe.LocationName = sqlDataReader["locationName"].ToString();
                    remoteProbe.MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString());
                    remoteProbe.MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString();
                    remoteProbe.RemoteProbeAddress = sqlDataReader["remoteProbeAddress"].ToString();
                    remoteProbe.LtLCLCommand = sqlDataReader["ltLCLCommand"].ToString();
                    remoteProbe.GtUCLCommand = sqlDataReader["gtUCLCommand"].ToString();
                    remoteProbe.CreatedBy = sqlDataReader["createdBy"].ToString();
                    remoteProbe.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    remoteProbe.ChangedBy = sqlDataReader["changedBy"].ToString();
                    remoteProbe.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    remoteProbe.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return remoteProbe;
        }

        public async Task SaveRemoteProbeAsync(RemoteProbe remoteProbe)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRemoteProbe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", remoteProbe.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_locationID", remoteProbe.LocationID.ToString());
                sqlCommand.Parameters.AddWithValue("_measurementTypeID", remoteProbe.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("_remoteProbeAddress", remoteProbe.RemoteProbeAddress);
                sqlCommand.Parameters.AddWithValue("_ltLCLCommand", remoteProbe.LtLCLCommand);
                sqlCommand.Parameters.AddWithValue("_gtUCLCommand", remoteProbe.GtUCLCommand);
                sqlCommand.Parameters.AddWithValue("_changedBy", remoteProbe.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", remoteProbe.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_isActive", remoteProbe.IsActive);

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

        public async Task DeleteRemoteProbeAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRemoteProbe";
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

        public async Task<Guid> GetLocationIDByRemoteProbeAsync(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationByRemoteProbeAddress";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

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
        
        public async Task<Guid> GetMeasurementTypeIDByRemoteProbeAsync(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurementTypeByRemoteProbeAddress";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

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
    
    }
}
