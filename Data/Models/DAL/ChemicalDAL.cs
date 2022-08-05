using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;
using Microsoft.Extensions.Configuration;

namespace GardenMVC.DAL
{
    public class ChemicalDAL
    {
        private readonly IConfiguration _config;

        public ChemicalDAL(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<Chemical> GetChemicals()
        {
            List<Chemical> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetChemicals", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddChemical(Chemical chemical)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddChemical", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", chemical.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", chemical.Name);
                sqlCmd.Parameters.AddWithValue("manufacturer", chemical.Manufacturer);
                sqlCmd.Parameters.AddWithValue("chemicalTypeID", chemical.ChemicalTypeID.ToString());
                sqlCmd.Parameters.AddWithValue("mixPriority", chemical.MixPriority);
                sqlCmd.Parameters.AddWithValue("mixTime", chemical.MixTime);
                sqlCmd.Parameters.AddWithValue("pricePerL", chemical.PricePerL);
                sqlCmd.Parameters.AddWithValue("inStockAmount", chemical.InStockAmount);
                sqlCmd.Parameters.AddWithValue("minReorderPoint", chemical.MinReorderPoint);                
                sqlCmd.Parameters.AddWithValue("createdBy", chemical.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", chemical.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", chemical.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", chemical.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", chemical.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public Chemical GetChemicalByID(Guid id)
        {
            Chemical chemical = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetChemicalByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return chemical;
        }

        public void SaveChemical(Chemical chemical)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateChemical", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", chemical.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", chemical.Name);
                sqlCmd.Parameters.AddWithValue("thismanufacturer", chemical.Manufacturer);
                sqlCmd.Parameters.AddWithValue("thischemicalTypeID", chemical.ChemicalTypeID);
                sqlCmd.Parameters.AddWithValue("thismixPriority", chemical.MixPriority);
                sqlCmd.Parameters.AddWithValue("thismixTime", chemical.MixTime);
                sqlCmd.Parameters.AddWithValue("thispricePerL", chemical.PricePerL);
                sqlCmd.Parameters.AddWithValue("thisinStockAmount", chemical.InStockAmount);
                sqlCmd.Parameters.AddWithValue("thisminReorderPoint", chemical.MinReorderPoint);
                sqlCmd.Parameters.AddWithValue("thischangedBy", chemical.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", chemical.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", chemical.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteChemical(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteChemical", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}
