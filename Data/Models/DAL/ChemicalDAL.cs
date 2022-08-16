using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace YARG.DAL
{
    public class ChemicalDAL
    {
        private readonly IConfiguration _config;

        public ChemicalDAL(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Chemical>> GetChemicalsAsync()
        {
            List<Chemical> lstream = new();

            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetChemicals";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        Chemical chemical = new Chemical
                        {
                            ID = Guid.Parse(sqlDataReader["id"].ToString()),
                            Name = sqlDataReader["name"].ToString(),
                            Manufacturer = sqlDataReader["manufacturer"].ToString(),
                            ChemicalTypeID = Guid.Parse(sqlDataReader["chemicalTypeID"].ToString()),
                            ChemicalTypeName = sqlDataReader["chemicalTypeName"].ToString(),
                            MixPriority = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixPriority")),
                            MixTime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTime")),
                            PricePerL = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("pricePerL")),
                            InStockAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("inStockAmount")),
                            MinReorderPoint = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("minReorderPoint")),
                            CreatedBy = sqlDataReader["createdBy"].ToString(),
                            CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                            ChangedBy = sqlDataReader["changedBy"].ToString(),
                            ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                            IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                        };

                        lstream.Add(chemical);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task AddChemicalAsync(Chemical chemical)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddChemical";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", chemical.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", chemical.Name);
                sqlCommand.Parameters.AddWithValue("manufacturer", chemical.Manufacturer);
                sqlCommand.Parameters.AddWithValue("chemicalTypeID", chemical.ChemicalTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("mixPriority", chemical.MixPriority);
                sqlCommand.Parameters.AddWithValue("mixTime", chemical.MixTime);
                sqlCommand.Parameters.AddWithValue("pricePerL", chemical.PricePerL);
                sqlCommand.Parameters.AddWithValue("inStockAmount", chemical.InStockAmount);
                sqlCommand.Parameters.AddWithValue("minReorderPoint", chemical.MinReorderPoint);
                sqlCommand.Parameters.AddWithValue("createdBy", chemical.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", chemical.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", chemical.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", chemical.ChangeDate);
                sqlCommand.Parameters.AddWithValue("isActive", chemical.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Chemical> GetChemicalByIDAsync(Guid id)
        {
            Chemical chemical = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetChemicalByID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        chemical.ID = Guid.Parse(sqlDataReader["id"].ToString());
                        chemical.Name = sqlDataReader["name"].ToString();
                        chemical.Manufacturer = sqlDataReader["manufacturer"].ToString();
                        chemical.ChemicalTypeID = Guid.Parse(sqlDataReader["chemicalTypeID"].ToString());
                        chemical.ChemicalTypeName = sqlDataReader["chemicalTypeName"].ToString();
                        chemical.MixPriority = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixPriority"));
                        chemical.MixTime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTime"));
                        chemical.PricePerL = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("pricePerL"));
                        chemical.InStockAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("inStockAmount"));
                        chemical.MinReorderPoint = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("minReorderPoint"));
                        chemical.CreatedBy = sqlDataReader["createdBy"].ToString();
                        chemical.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                        chemical.ChangedBy = sqlDataReader["changedBy"].ToString();
                        chemical.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                        chemical.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
 
            return chemical;
        }

        public async Task SaveChemicalAsync(Chemical chemical)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateChemical";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", chemical.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", chemical.Name);
                sqlCommand.Parameters.AddWithValue("thismanufacturer", chemical.Manufacturer);
                sqlCommand.Parameters.AddWithValue("thischemicalTypeID", chemical.ChemicalTypeID);
                sqlCommand.Parameters.AddWithValue("thismixPriority", chemical.MixPriority);
                sqlCommand.Parameters.AddWithValue("thismixTime", chemical.MixTime);
                sqlCommand.Parameters.AddWithValue("thispricePerL", chemical.PricePerL);
                sqlCommand.Parameters.AddWithValue("thisinStockAmount", chemical.InStockAmount);
                sqlCommand.Parameters.AddWithValue("thisminReorderPoint", chemical.MinReorderPoint);
                sqlCommand.Parameters.AddWithValue("thischangedBy", chemical.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", chemical.ChangeDate);
                sqlCommand.Parameters.AddWithValue("thisisActive", chemical.IsActive);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteChemicalAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteChemical";
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
    }
}
