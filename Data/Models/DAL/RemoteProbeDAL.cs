using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class RemoteProbeDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public RemoteProbeDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<RemoteProbe> GetRemoteProbes()
        {
            List<RemoteProbe> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetRemoteProbes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddRemoteProbe(RemoteProbe remoteProbe)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
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

        public RemoteProbe GetRemoteProbeByID(Guid id)
        {
            RemoteProbe remoteProbe = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetRemoteProbeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return remoteProbe;
        }

        public void SaveRemoteProbe(RemoteProbe remoteProbe)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateRemoteProbe", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", remoteProbe.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thislocationID", remoteProbe.LocationID.ToString());
                sqlCmd.Parameters.AddWithValue("thismeasurementTypeID", remoteProbe.MeasurementTypeID.ToString());

                sqlCmd.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbe.RemoteProbeAddress);
                sqlCmd.Parameters.AddWithValue("thischangedBy", remoteProbe.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", remoteProbe.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", remoteProbe.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteRemoteProbe(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteRemoteProbe", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public Guid GetLocationIDByRemoteProbe(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetLocationByRemoteProbeAddress", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    id = Guid.Parse(sqlDataReader["locationID"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return id;
        }
        public Guid GetMeasurementTypeIDByRemoteProbe(string remoteProbeAddress)
        {
            Guid id = Guid.Empty;

            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetMeasurementTypeByRemoteProbeAddress", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisremoteProbeAddress", remoteProbeAddress);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    id = Guid.Parse(sqlDataReader["measurementTypeID"].ToString());
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return id;
        }
    }
}
