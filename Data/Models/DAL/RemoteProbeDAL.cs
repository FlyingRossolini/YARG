using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class RemoteProbeDAL
    {
        private readonly IConfiguration _config;

        public RemoteProbeDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IEnumerable<RemoteProbe>> GetRemoteProbesAsync()
        {
            List<RemoteProbe> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRemoteProbes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    RemoteProbe remoteProbe = new RemoteProbe
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        LocationID = Guid.Parse(sqlDataReader["locationID"].ToString()),
                        LocationName = sqlDataReader["locationName"].ToString(),
                        MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString()),
                        MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString(),
                        RemoteProbeAddress = sqlDataReader["remoteProbeAddress"].ToString(),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(remoteProbe);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task AddRemoteProbeAsync(RemoteProbe remoteProbe)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddRemoteProbe", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", remoteProbe.ID.ToString());
                sqlCmd.Parameters.AddWithValue("locationID", remoteProbe.LocationID.ToString());
                sqlCmd.Parameters.AddWithValue("measurementTypeID", remoteProbe.MeasurementTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("remoteProbeAddress", remoteProbe.RemoteProbeAddress);
                sqlCmd.Parameters.AddWithValue("createdBy", remoteProbe.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", remoteProbe.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", remoteProbe.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", remoteProbe.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", remoteProbe.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public async Task<RemoteProbe> GetRemoteProbeByIDAsync(Guid id)
        {
            RemoteProbe remoteProbe = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRemoteProbeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        remoteProbe.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        remoteProbe.LocationID = Guid.Parse(sqlDataReader["locationID"].ToString());
                        remoteProbe.LocationName = sqlDataReader["locationName"].ToString();
                        remoteProbe.MeasurementTypeID = Guid.Parse(sqlDataReader["measurementTypeID"].ToString());
                        remoteProbe.MeasurementTypeName = sqlDataReader["measurementTypeName"].ToString();
                        remoteProbe.RemoteProbeAddress = sqlDataReader["remoteProbeAddress"].ToString();
                        remoteProbe.CreatedBy = sqlDataReader["createdBy"].ToString();
                        remoteProbe.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        remoteProbe.ChangedBy = sqlDataReader["changedBy"].ToString();
                        remoteProbe.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        remoteProbe.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return remoteProbe;
        }

        public async Task SaveRemoteProbeAsync(RemoteProbe remoteProbe)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRemoteProbe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", remoteProbe.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thislocationID", remoteProbe.LocationID.ToString());
                sqlCommand.Parameters.AddWithValue("thismeasurementTypeID", remoteProbe.MeasurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbe.RemoteProbeAddress);
                sqlCommand.Parameters.AddWithValue("thischangedBy", remoteProbe.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", remoteProbe.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", remoteProbe.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteRemoteProbeAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRemoteProbe";
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

        public async Task<Guid> GetLocationIDByRemoteProbeAsync(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLocationByRemoteProbeAddress";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        id = Guid.Parse(sqlDataReader["locationID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }
        
        public async Task<Guid> GetMeasurementTypeIDByRemoteProbeAsync(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetMeasurementTypeByRemoteProbeAddress";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        id = Guid.Parse(sqlDataReader["locationID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }
    }
}
