using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using GardenMVC.Models;

namespace GardenMVC.DAL
{
    public class ChemicalTypeDAL
    {
        private readonly ConnectionStringManager _connectionStringManager;

        public ChemicalTypeDAL()
        {
            _connectionStringManager = new();
        }

        public IEnumerable<ChemicalType> GetChemicalTypes()
        {
            List<ChemicalType> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetChemicalTypes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    ChemicalType chemicalType = new ChemicalType
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        
        public void AddChemicalType(ChemicalType chemicalType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddChemicalType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", chemicalType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", chemicalType.Name);
                sqlCmd.Parameters.AddWithValue("isH2O2", chemicalType.IsH2O2);
                sqlCmd.Parameters.AddWithValue("isPhUp", chemicalType.IsPhUp);
                sqlCmd.Parameters.AddWithValue("isPhDown", chemicalType.IsPhDown);
                sqlCmd.Parameters.AddWithValue("sorting", chemicalType.Sorting);
                sqlCmd.Parameters.AddWithValue("createdBy", chemicalType.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", chemicalType.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", chemicalType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", chemicalType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", chemicalType.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close(); 
                sqlConnection.Dispose();
            }

        }

        public ChemicalType GetChemicalTypeByID(Guid id)
        {
            ChemicalType chemicalType = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetChemicalTypeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
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

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return chemicalType;
        }

        public void SaveChemicalType(ChemicalType chemicalType)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateChemicalType", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", chemicalType.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisisH2O2", chemicalType.IsH2O2);
                sqlCmd.Parameters.AddWithValue("thisisPhUp", chemicalType.IsPhUp);
                sqlCmd.Parameters.AddWithValue("thisisPhDown", chemicalType.IsPhDown);
                sqlCmd.Parameters.AddWithValue("thissorting", chemicalType.Sorting);
                sqlCmd.Parameters.AddWithValue("thisname", chemicalType.Name);
                sqlCmd.Parameters.AddWithValue("thischangedBy", chemicalType.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", chemicalType.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", chemicalType.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void DeleteChemicalType(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteChemicalType", sqlConnection);
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
