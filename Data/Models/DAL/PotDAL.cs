using YARG.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace YARG.DAL
{
    public class PotDAL
    {
        private readonly IConfiguration _config;
        public PotDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IEnumerable<Pot> GetPots()
        {
            List<Pot> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetPots", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Pot pot = new Pot
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        QueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePosition")),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed")),
                        FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    lstream.Add(pot);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }
        public IEnumerable<Pot> GetActivePots()
        {
            List<Pot> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetPots", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Pot pot = new Pot
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        QueuePosition = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePosition")),
                        EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration")),
                        EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount")),
                        EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed")),
                        FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed")),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                        IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"))
                    };

                    if (pot.IsActive)
                    {
                        lstream.Add(pot);
                    }
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }

        public void AddPot(Pot pot)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spAddPot", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("id", pot.ID.ToString());
                sqlCmd.Parameters.AddWithValue("name", pot.Name);
                sqlCmd.Parameters.AddWithValue("queuePosition", pot.QueuePosition);
                sqlCmd.Parameters.AddWithValue("efDuration", pot.EFDuration);
                sqlCmd.Parameters.AddWithValue("efAmount", pot.EFAmount);
                sqlCmd.Parameters.AddWithValue("ebbSpeed", pot.EbbSpeed);
                sqlCmd.Parameters.AddWithValue("flowSpeed", pot.FlowSpeed);
                sqlCmd.Parameters.AddWithValue("createdBy", pot.CreatedBy);
                sqlCmd.Parameters.AddWithValue("createDate", pot.CreateDate);
                sqlCmd.Parameters.AddWithValue("changedBy", pot.ChangedBy);
                sqlCmd.Parameters.AddWithValue("changeDate", pot.ChangeDate);
                sqlCmd.Parameters.AddWithValue("isActive", pot.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public Pot GetPotByID(Guid id)
        {
            Pot pot = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetPotByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    pot.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    pot.Name = sqlDataReader["name"].ToString();
                    //pot.QueuePosition = sqlDataReader.GetInt16(sqlDataReader["queuePosition"].ToString());
                    pot.EFDuration = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("efDuration"));
                    pot.EFAmount = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("efAmount"));
                    pot.EbbSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed"));
                    pot.FlowSpeed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed"));
                    pot.CreatedBy = sqlDataReader["createdBy"].ToString();
                    pot.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    pot.ChangedBy = sqlDataReader["changedBy"].ToString();
                    pot.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    pot.IsActive = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isActive"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return pot;
        }

        public short PotCount()
        {
            short count = 0;
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spPotCount", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    count = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("cntPots"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return count;
        }
        public bool SavePot(Pot pot)
        {
            Pot p = GetPotByID(pot.ID);
            bool flgUpdateWateringSchedule = false;

            if(p.IsActive != pot.IsActive ||
                p.EFAmount != pot.EFAmount ||
                p.EFDuration != pot.EFDuration)
            {
                flgUpdateWateringSchedule = true;
            }

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdatePot", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", pot.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", pot.Name);
                sqlCmd.Parameters.AddWithValue("thisefDuration", pot.EFDuration);
                sqlCmd.Parameters.AddWithValue("thisefAmount", pot.EFAmount);
                sqlCmd.Parameters.AddWithValue("thisebbSpeed", pot.EbbSpeed);
                sqlCmd.Parameters.AddWithValue("thisflowSpeed", pot.FlowSpeed);
                sqlCmd.Parameters.AddWithValue("thischangedBy", pot.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", pot.ChangeDate);
                sqlCmd.Parameters.AddWithValue("thisisActive", pot.IsActive);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return flgUpdateWateringSchedule;
        }

        public void DeletePot(Guid id)
        {

            // delete this pot from watering schedule
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeletePotsFromWateringSchedule", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeletePot", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            // fix up queue positions of all pots here
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spFixPotQueuePositions", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }



        }

        public short GetNextQueuePosition()
        {
            short r = 0;
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetNextPotQueuePosition", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    r = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("queuePos"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return r;
        }

        public short GetEbbSpeedFromQueuePosition(short queuePos)
        {
            short speed = 0;

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spGetEbbSpeedFromQueuePosition", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisqueuePosition", queuePos);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCmd.ExecuteReader();
                sqlCmd.Dispose();

                while (sqlDataReader.Read())
                {
                    speed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("ebbSpeed"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return speed;
        }
        public short GetFlowSpeedFromQueuePosition(short queuePos)
        {
            short speed = 0;

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spGetFlowSpeedFromQueuePosition", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisqueuePosition", queuePos);

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCmd.ExecuteReader();
                sqlCmd.Dispose();

                while (sqlDataReader.Read())
                {
                    speed = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("flowSpeed"));
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return speed;
        }
    }
}
