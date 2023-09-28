using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class PumpDAL
    {
        private readonly string _connectionString;

        public PumpDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<Pump>> GetPumpsAsync()
        {
            List<Pump> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPumps";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    Pump pump = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        PotID = Guid.Parse(sqlDataReader["potID"].ToString()),
                        Make = sqlDataReader["make"].ToString(),
                        Model = sqlDataReader["model"].ToString(),
                        SerialNumber = sqlDataReader["serialNumber"].ToString(),
                        Vendor = sqlDataReader["vendor"].ToString(),
                        Price = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("price")),
                        PulsesPerLiter = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("pulsesPerLiter")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    if (sqlDataReader["purchaseDate"].ToString() != "")
                    {
                        pump.PurchaseDate = Convert.ToDateTime(sqlDataReader["purchaseDate"].ToString());
                    };

                    if (sqlDataReader["installDate"].ToString() != "")
                    {
                        pump.InstallDate = Convert.ToDateTime(sqlDataReader["installDate"].ToString());
                    };

                    lstream.Add(pump);
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

        public async Task AddPumpAsync(Pump pump)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddPump";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", pump.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_potID", pump.PotID.ToString());
                sqlCommand.Parameters.AddWithValue("_make", pump.Make);
                sqlCommand.Parameters.AddWithValue("_model", pump.Model);
                sqlCommand.Parameters.AddWithValue("_serialNumber", pump.SerialNumber);
                sqlCommand.Parameters.AddWithValue("_vendor", pump.Vendor);
                sqlCommand.Parameters.AddWithValue("_price", pump.Price);
                sqlCommand.Parameters.AddWithValue("_pulsesPerLiter", pump.PulsesPerLiter);
                if (pump.InstallDate != DateTime.MinValue)
                {
                    sqlCommand.Parameters.AddWithValue("_installDate", pump.InstallDate);
                }
                if (pump.PurchaseDate != DateTime.MinValue)
                {
                    sqlCommand.Parameters.AddWithValue("_purchaseDate", pump.PurchaseDate);
                }
                sqlCommand.Parameters.AddWithValue("_createdBy", pump.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", pump.CreateDate);
                sqlCommand.Parameters.AddWithValue("_changedBy", pump.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", pump.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_isActive", pump.IsActive);

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

        public async Task<Pump> GetPumpByIDAsync(Guid id)
        {
            Pump pump = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetPumpByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", id.ToString());

                await sqlConnection.OpenAsync();
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    pump.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    pump.PotID = Guid.Parse(sqlDataReader["potID"].ToString());
                    pump.Make = sqlDataReader["make"].ToString();
                    pump.Model = sqlDataReader["model"].ToString();
                    pump.SerialNumber = sqlDataReader["serialNumber"].ToString();
                    pump.Vendor = sqlDataReader["vendor"].ToString();
                    pump.Price = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("price"));
                    pump.PulsesPerLiter = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("pulsesPerLiter"));
                    if (sqlDataReader["purchaseDate"].ToString() != "")
                    {
                        pump.PurchaseDate = Convert.ToDateTime(sqlDataReader["purchaseDate"].ToString());
                    }
                    {
                        pump.InstallDate = Convert.ToDateTime(sqlDataReader["installDate"].ToString());
                    }
                    pump.CreatedBy = sqlDataReader["createdBy"].ToString();
                    pump.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    pump.ChangedBy = sqlDataReader["changedBy"].ToString();
                    pump.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    pump.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return pump;
        }

        public async Task SavePumpAsync(Pump pump)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdatePump";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", pump.ID.ToString());
                sqlCommand.Parameters.AddWithValue("_potID", pump.PotID.ToString());
                sqlCommand.Parameters.AddWithValue("_make", pump.Make);
                sqlCommand.Parameters.AddWithValue("_model", pump.Model);
                sqlCommand.Parameters.AddWithValue("_serialNumber", pump.SerialNumber);
                sqlCommand.Parameters.AddWithValue("_vendor", pump.Vendor);
                sqlCommand.Parameters.AddWithValue("_price", pump.Price);
                sqlCommand.Parameters.AddWithValue("_pulsesPerLiter", pump.PulsesPerLiter);
                if (pump.InstallDate != null)
                {
                    sqlCommand.Parameters.AddWithValue("_installDate", pump.InstallDate);
                }
                if (pump.PurchaseDate != null)
                {
                    sqlCommand.Parameters.AddWithValue("_purchaseDate", pump.PurchaseDate);
                }
                sqlCommand.Parameters.AddWithValue("_createdBy", pump.CreatedBy);
                sqlCommand.Parameters.AddWithValue("_createDate", pump.CreateDate);
                sqlCommand.Parameters.AddWithValue("_changedBy", pump.ChangedBy);
                sqlCommand.Parameters.AddWithValue("_changeDate", pump.ChangeDate);
                sqlCommand.Parameters.AddWithValue("_isActive", pump.IsActive);

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

        public async Task DeletePumpAsync(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeletePump";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("_id", id.ToString());

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
