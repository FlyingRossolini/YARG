using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.DAL
{
    public class ChemicalTypeDAL
    {
        private readonly string _connectionString;

        public ChemicalTypeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IEnumerable<ChemicalType>> GetChemicalTypesAsync()
        {
            List<ChemicalType> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetChemicalTypes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    ChemicalType chemicalType = new()
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        IsH2O2 = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isH2O2")),
                        IsPhUp = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isPhUp")),
                        IsPhDown = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isPhDown")),
                        Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(chemicalType);
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
        
        public async Task AddChemicalTypeAsync(ChemicalType chemicalType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddChemicalType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", chemicalType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", chemicalType.Name);
                sqlCommand.Parameters.AddWithValue("isH2O2", chemicalType.IsH2O2);
                sqlCommand.Parameters.AddWithValue("isPhUp", chemicalType.IsPhUp);
                sqlCommand.Parameters.AddWithValue("isPhDown", chemicalType.IsPhDown);
                sqlCommand.Parameters.AddWithValue("sorting", chemicalType.Sorting);
                sqlCommand.Parameters.AddWithValue("createdBy", chemicalType.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", chemicalType.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", chemicalType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", chemicalType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", chemicalType.IsActive);

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

        public async Task<ChemicalType> GetChemicalTypeByIDAsync(Guid id)
        {
            ChemicalType chemicalType = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetChemicalTypeByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    chemicalType.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    chemicalType.Name = sqlDataReader["name"].ToString();
                    chemicalType.IsH2O2 = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isH2O2"));
                    chemicalType.IsPhUp = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isPhUp"));
                    chemicalType.IsPhDown = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isPhDown"));
                    chemicalType.Sorting = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("sorting"));
                    chemicalType.CreatedBy = sqlDataReader["createdBy"].ToString();
                    chemicalType.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    chemicalType.ChangedBy = sqlDataReader["changedBy"].ToString();
                    chemicalType.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    chemicalType.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
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

            return chemicalType;
        }

        public async Task SaveChemicalTypeAsync(ChemicalType chemicalType)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateChemicalType";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", chemicalType.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisisH2O2", chemicalType.IsH2O2);
                sqlCommand.Parameters.AddWithValue("thisisPhUp", chemicalType.IsPhUp);
                sqlCommand.Parameters.AddWithValue("thisisPhDown", chemicalType.IsPhDown);
                sqlCommand.Parameters.AddWithValue("thissorting", chemicalType.Sorting);
                sqlCommand.Parameters.AddWithValue("thisname", chemicalType.Name);
                sqlCommand.Parameters.AddWithValue("thischangedBy", chemicalType.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", chemicalType.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", chemicalType.IsActive);

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

        public async Task DeleteChemicalType(Guid id)
        {
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteChemicalType";
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
