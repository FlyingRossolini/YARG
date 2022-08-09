using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using YARG.Models;
using Microsoft.Extensions.Configuration;

namespace YARG.DAL
{
    public class JarDAL
    {
        private readonly IConfiguration _config;

        public JarDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<Jar> GetJars()
        {
            List<Jar> lstream = new();
            try
            {
                using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
                {
                    sqlConnection.Open();

                    using (MySqlCommand sqlCommand = new())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "spGetJars";
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                        {
                            while (sqlDataReader.Read())
                            {
                                Jar jar = new()
                                {
                                    ID = Guid.Parse(sqlDataReader["id"].ToString()),
                                    MixFanPosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanPosition")),
                                    ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString()),
                                    ChemicalName = sqlDataReader["chemicalName"].ToString(),
                                    MixTimesPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTimesPerDay")),
                                    Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("duration")),
                                    Capacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("capacity")),
                                    CurrentAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentAmount")),
                                    MixFanOverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed")),
                                    CreatedBy = sqlDataReader["createdBy"].ToString(),
                                    CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                                    ChangedBy = sqlDataReader["changedBy"].ToString(),
                                    ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                                    IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                                };

                                lstream.Add(jar);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public void AddJar(Jar jar)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddJar", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", jar.ID.ToString());
                sqlCmd.Parameters.AddWithValue("mixFanPosition", jar.MixFanPosition);
                sqlCmd.Parameters.AddWithValue("chemicalID", jar.ChemicalID.ToString());
                sqlCmd.Parameters.AddWithValue("mixTimesPerDay", jar.MixTimesPerDay);
                sqlCmd.Parameters.AddWithValue("duration", jar.Duration);
                sqlCmd.Parameters.AddWithValue("capacity", jar.Capacity);
                sqlCmd.Parameters.AddWithValue("currentAmount", jar.CurrentAmount);
                sqlCmd.Parameters.AddWithValue("mixFanOverSpeed", jar.MixFanOverSpeed);
                sqlCmd.Parameters.AddWithValue("createdBy", jar.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", jar.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", jar.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", jar.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", jar.IsActive);
                
                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

        }

        public Jar GetJarByID(Guid id)
        {
            Jar jar = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetJarByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    jar.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    jar.MixFanPosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanPosition"));
                    jar.ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString());
                    jar.ChemicalName = sqlDataReader["chemicalName"].ToString();
                    jar.MixTimesPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixTimesPerDay"));
                    jar.Duration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("duration"));
                    jar.Capacity = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("capacity"));
                    jar.CurrentAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("currentAmount"));
                    jar.MixFanOverSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixFanOverSpeed"));
                    jar.CreatedBy = sqlDataReader["createdBy"].ToString();
                    jar.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    jar.ChangedBy = sqlDataReader["changedBy"].ToString();
                    jar.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    jar.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return jar;
        }

        public bool SaveJar(Jar jar)
        {
            Jar j = GetJarByID(jar.ID);
            bool flgUpdateMixingFanSchedule = false;

            if (j.IsActive != jar.IsActive ||
                j.MixTimesPerDay != jar.MixTimesPerDay ||
                j.Duration != jar.Duration)
            {
                flgUpdateMixingFanSchedule = true;
            }


            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateJar", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", jar.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thismixFanPosition", jar.MixFanPosition);
                sqlCmd.Parameters.AddWithValue("thischemicalID", jar.ChemicalID.ToString());
                sqlCmd.Parameters.AddWithValue("thismixTimesPerDay", jar.MixTimesPerDay);
                sqlCmd.Parameters.AddWithValue("thisduration", jar.Duration);
                sqlCmd.Parameters.AddWithValue("thiscapacity", jar.Capacity);
                sqlCmd.Parameters.AddWithValue("thiscurrentAmount", jar.CurrentAmount);
                sqlCmd.Parameters.AddWithValue("thismixFanOverSpeed", jar.MixFanOverSpeed);
                sqlCmd.Parameters.AddWithValue("thischangedBy", jar.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", jar.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", jar.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return flgUpdateMixingFanSchedule;
        }

        public void DeleteJar(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteJar", sqlConnection);
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
