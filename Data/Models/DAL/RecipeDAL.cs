using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YARG.Models;
using YARG.Common_Types;
using YARG.Data.Models.ViewModels;
using Microsoft.Extensions.Configuration;

namespace YARG.DAL
{
    public class RecipeDAL
    {
        private readonly IConfiguration _config;

        public RecipeDAL(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void AddRecipe(Recipe recipe)
        {
            try
            {
                using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
                {
                    sqlConnection.Open();

                    using (MySqlCommand sqlCommand = new())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "spAddRecipe";
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("id", recipe.ID.ToString());
                        sqlCommand.Parameters.AddWithValue("name", recipe.Name);
                        sqlCommand.Parameters.AddWithValue("createdBy", recipe.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("createDate", recipe.CreateDate);
                        sqlCommand.Parameters.AddWithValue("changedBy", recipe.ChangedBy);
                        sqlCommand.Parameters.AddWithValue("changeDate", recipe.ChangeDate);

                        sqlCommand.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Recipe GetRecipeByID(Guid recipeID)
        {
            Recipe recipe = new();

            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetRecipeByID", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid",recipeID.ToString());

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    //{
                    //    recipe.ID = Guid.Parse(sqlDataReader["id"].ToString());
                    //    recipe.FeedingChartTypeID = Guid.Parse(sqlDataReader["feedingChartTypeID"].ToString());
                    //    recipe.FeedingChartTypeName = sqlDataReader["feedingChartTypeName"].ToString();
                    //    recipe.WeekNumber = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("weekNumber"));
                    //    recipe.LightCycleID = Guid.Parse(sqlDataReader["lightCycleID"].ToString());
                    //    recipe.LightCycleName = sqlDataReader["lightCycleName"].ToString();
                    //    recipe.ChemicalsID = Guid.Parse(sqlDataReader["chemicalsID"].ToString());
                    //    recipe.CreatedBy = sqlDataReader["createdBy"].ToString();
                    //    recipe.CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString());
                    //    recipe.ChangedBy = sqlDataReader["changedBy"].ToString();
                    //    recipe.ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString());
                    //};
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return recipe;
        }
        public void SaveRecipe(Recipe recipe)
        {
            //using (MySqlConnection sqlConnection = new MySqlConnection(_connectionStringManager.GetConnectionString()))
            //{
            //    MySqlCommand sqlCmd = new MySqlCommand("spUpdateRecipe", sqlConnection);
            //    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    sqlCmd.Parameters.AddWithValue("thisid", recipe.ID.ToString());
            //    sqlCmd.Parameters.AddWithValue("thisfeedingChartTypeID", recipe.FeedingChartTypeID.ToString());
            //    sqlCmd.Parameters.AddWithValue("thisweekNumber", recipe.WeekNumber);
            //    sqlCmd.Parameters.AddWithValue("thislightCycleID", recipe.LightCycleID);
            //    sqlCmd.Parameters.AddWithValue("thischemicalsID", recipe.ChemicalsID);
            //    sqlCmd.Parameters.AddWithValue("thischangedBy", recipe.ChangedBy);
            //    sqlCmd.Parameters.AddWithValue("thischangeDate", recipe.ChangeDate);

            //    sqlConnection.Open();
            //    sqlCmd.ExecuteNonQuery();
            //    sqlCmd.Dispose();
            //    sqlConnection.Close();
            //    sqlConnection.Dispose();
            //}
        }

        public void DeleteRecipe(Guid id)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spDeleteRecipe", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", id.ToString());

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void UpdateRecipeName(Recipe recipe)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCmd = new MySqlCommand("spUpdateRecipeName", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("thisid", recipe.ID.ToString());
                sqlCmd.Parameters.AddWithValue("thisname", recipe.Name);
                sqlCmd.Parameters.AddWithValue("thischangedBy", recipe.ChangedBy);
                sqlCmd.Parameters.AddWithValue("thischangeDate", recipe.ChangeDate);

                sqlConnection.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public void UpdateRecipeStepLimit(RecipeStepLimit recipeStepLimit)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepLimit";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStepLimit.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisvalue", recipeStepLimit.Value);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepLimit.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepLimit.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void UpdateRecipeStepIrrigation(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepIrrigation";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisirrigationEventsPerDay", recipeStep.IrrigationEventsPerDay);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void UpdateRecipeStepAmount(RecipeStepAmount recipeStepAmount)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepAmount";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStepAmount.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisamount", recipeStepAmount.Amount);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepAmount.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepAmount.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void UpdateRecipeStepSoaktime(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepSoaktime";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thissoakTime", recipeStep.SoakTime);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void UpdateRecipeStepMorningSip(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepMorningSip";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisisMorningSip", recipeStep.IsMorningSip);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void UpdateRecipeStepEveningSip(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepEveningSip";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisisEveningSip", recipeStep.IsEveningSip);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void UpdateRecipeStepLightCycle(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spUpdateRecipeStepLightCycle";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thislightCycleID", recipeStep.LightCycleID.ToString());
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void DeleteRecipeStep(Guid guid)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spDeleteRecipeStep";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void DeleteRecipeStepLimit(Guid guid)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spDeleteRecipeStepLimit";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void DeleteRecipeStepAmount(Guid guid)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spDeleteRecipeStepAmount";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void AddRecipeStepLimit(RecipeStepLimit recipeStepLimit)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spAddRecipeStepLimit";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStepLimit.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisrecipeStepID", recipeStepLimit.RecipeStepID.ToString());
                    sqlCommand.Parameters.AddWithValue("thislocationID", recipeStepLimit.LocationID.ToString());
                    sqlCommand.Parameters.AddWithValue("thismeasurementTypeID", recipeStepLimit.MeasurementTypeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thislimitTypeID", recipeStepLimit.LimitTypeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisvalue", recipeStepLimit.Value);
                    sqlCommand.Parameters.AddWithValue("thiscreatedBy", recipeStepLimit.CreatedBy);
                    sqlCommand.Parameters.AddWithValue("thiscreateDate", recipeStepLimit.CreateDate);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepLimit.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepLimit.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void AddRecipeStepAmount(RecipeStepAmount recipeStepAmount)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spAddRecipeStepAmount";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStepAmount.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisrecipeStepID", recipeStepAmount.RecipeStepID.ToString());
                    sqlCommand.Parameters.AddWithValue("thischemicalID", recipeStepAmount.ChemicalID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisamount", recipeStepAmount.Amount);
                    sqlCommand.Parameters.AddWithValue("thiscreatedBy", recipeStepAmount.CreatedBy);
                    sqlCommand.Parameters.AddWithValue("thiscreateDate", recipeStepAmount.CreateDate);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepAmount.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepAmount.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
        public void AddRecipeStep(RecipeStep recipeStep)
        {
            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spAddRecipeStep";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisrecipeID", recipeStep.RecipeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisweekNumber", recipeStep.WeekNumber);
                    sqlCommand.Parameters.AddWithValue("thislightCycleID", recipeStep.LightCycleID.ToString());
                    sqlCommand.Parameters.AddWithValue("thisirrigationEventsPerDay", recipeStep.IrrigationEventsPerDay);
                    sqlCommand.Parameters.AddWithValue("thissoakTime", recipeStep.SoakTime);
                    sqlCommand.Parameters.AddWithValue("thisisMorningSip", recipeStep.IsMorningSip);
                    sqlCommand.Parameters.AddWithValue("thisisEveningSip", recipeStep.IsEveningSip);
                    sqlCommand.Parameters.AddWithValue("thiscreatedBy", recipeStep.CreatedBy);
                    sqlCommand.Parameters.AddWithValue("thiscreateDate", recipeStep.CreateDate);
                    sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                    sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public RecipeStep GetLastRecipeStepByRecipeID(Guid guid)
        {
            RecipeStep recipeStep = new();

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetLastRecipeStepByRecipeID";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());


                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {
                            recipeStep.WeekNumber = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("weekNumber"));
                            recipeStep.LightCycleID = Guid.Parse(sqlDataReader["lightCycleID"].ToString());
                            recipeStep.IrrigationEventsPerDay = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("irrigationEventsPerDay"));
                            recipeStep.SoakTime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("soakTime"));
                            recipeStep.IsMorningSip = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isMorningSip"));
                            recipeStep.IsEveningSip = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("isEveningSip"));
                        }
                    }
                }
            }

            return recipeStep;

        }
        
        public decimal GetLastRecipeStepLimitValue(Guid recipeID, 
            Guid locationTypeID, Guid measurementTypeID, Guid limitTypeID)
        {
            decimal tmpDec = 0;

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetLastRecipeStepLimitValue";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisrecipeID", recipeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thislocationTypeID", locationTypeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thismeasurementTypeID", measurementTypeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thislimitTypeID", limitTypeID.ToString());

                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {
                            tmpDec = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("value"));
                        }
                    }
                }
            }

            return tmpDec;
        }

        public decimal GetLastRecipeStepAmountValue(Guid recipeID, Guid chemicalID)
        {
            decimal tmpDec = 0;

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetLastRecipeStepAmountValue";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisrecipeID", recipeID.ToString());
                    sqlCommand.Parameters.AddWithValue("thischemicalID", chemicalID.ToString());

                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {
                            tmpDec = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("amount"));
                        }
                    }
                }
            }

            return tmpDec;
        }

        public RecipeStepLimit GetLastRecipeStepLimitValueByRecipeID(Guid guid)
        {
            RecipeStepLimit recipeStepLimit = new();

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetLastRecipeStepLimitByRecipeID";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());


                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {


                        }
                    }
                }
            }

            return recipeStepLimit;

        }

        public IEnumerable<RecipeChemListViewModel> GetRecipeChemListViewModels()
        {
            List<RecipeChemListViewModel> lstream = new();

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetRecipeChemListViewModels";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {
                            RecipeChemListViewModel recipeChemListViewModel = new()
                            {
                                RecipeID = Guid.Parse(sqlDataReader["recipeID"].ToString()),
                                ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString()),
                                ChemicalTypeID = Guid.Parse(sqlDataReader["chemicalTypeID"].ToString()),
                                ChemicalTypeName = sqlDataReader["chemicalTypeName"].ToString(),
                                ChemicalName = sqlDataReader["chemicalName"].ToString(),
                                MixPriority = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixPriority")),
                                MixTime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixtime"))
                            };

                            lstream.Add(recipeChemListViewModel);
                        }
                    }
                }
            }

            return lstream;
        }
        public IEnumerable<Recipe> GetRecipes()
        {
            List<Recipe> lstream = new();
            using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            {
                MySqlCommand sqlCommand = new MySqlCommand("spGetRecipes", sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                sqlCommand.Dispose();

                while (sqlDataReader.Read())
                {
                    Recipe recipe = new Recipe
                    {
                        ID = Guid.Parse(sqlDataReader["id"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        CreatedBy = sqlDataReader["createdBy"].ToString(),
                        CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                        ChangedBy = sqlDataReader["changedBy"].ToString(),
                        ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString())
                    };

                    List<RecipeChemList> recipeChemLists = new();
                    MySqlConnection sqlRecipeChemListConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                    MySqlCommand sqlRecipeChemListCommand = new MySqlCommand("spGetRecipeChemListByRecipeID", sqlRecipeChemListConnection);
                    sqlRecipeChemListCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlRecipeChemListCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

                    sqlRecipeChemListConnection.Open();
                    MySqlDataReader sqlRecipeChemListReader = sqlRecipeChemListCommand.ExecuteReader();
                    sqlRecipeChemListCommand.Dispose();

                    while (sqlRecipeChemListReader.Read())
                    {
                        RecipeChemList recipeChemList = new RecipeChemList
                        {
                            ID = Guid.Parse(sqlRecipeChemListReader["id"].ToString()),
                            RecipeID = Guid.Parse(sqlRecipeChemListReader["recipeID"].ToString()),
                            ChemicalID = Guid.Parse(sqlRecipeChemListReader["chemicalID"].ToString()),
                            ChemicalName = sqlRecipeChemListReader["chemicalName"].ToString(),
                            Mixtime = sqlRecipeChemListReader.GetInt16(sqlRecipeChemListReader.GetOrdinal("mixtime")),
                            //Priority = sqlRecipeChemListReader.GetInt16(sqlRecipeChemListReader.GetOrdinal("priority")),
                            CreatedBy = sqlRecipeChemListReader["createdBy"].ToString(),
                            CreateDate = Convert.ToDateTime(sqlRecipeChemListReader["createDate"].ToString()),
                            ChangedBy = sqlRecipeChemListReader["changedBy"].ToString(),
                            ChangeDate = Convert.ToDateTime(sqlRecipeChemListReader["changeDate"].ToString())

                        };
                        
                        recipeChemLists.Add(recipeChemList);
                   }

                    sqlRecipeChemListReader.Close();
                    sqlRecipeChemListReader.Dispose();

                    sqlRecipeChemListConnection.Close();
                    sqlRecipeChemListConnection.Dispose();

                    recipe.RecipeChems = recipeChemLists;

                    List<RecipeStep> recipeSteps = new();
                    MySqlConnection sqlRecipeStepConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                    MySqlCommand sqlRecipeStepCommand = new MySqlCommand("spGetRecipeStepsByRecipeID", sqlRecipeStepConnection);
                    sqlRecipeStepCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlRecipeStepCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

                    sqlRecipeStepConnection.Open();
                    MySqlDataReader sqlRecipeStepReader = sqlRecipeStepCommand.ExecuteReader();
                    sqlRecipeStepCommand.Dispose();

                    while (sqlRecipeStepReader.Read())
                    {
                        RecipeStep recipeStep = new RecipeStep
                        {
                            ID = Guid.Parse(sqlRecipeStepReader["id"].ToString()),
                            RecipeID = Guid.Parse(sqlRecipeStepReader["recipeID"].ToString()),
                            WeekNumber = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("weekNumber")),
                            LightCycleID = Guid.Parse(sqlRecipeStepReader["lightCycleID"].ToString()),
                            LightCycleName = sqlRecipeStepReader["lightCycleName"].ToString(),
                            IrrigationEventsPerDay = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("irrigationEventsPerDay")),
                            SoakTime = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("soakTime")),
                            IsMorningSip = sqlRecipeStepReader.GetBoolean(sqlRecipeStepReader.GetOrdinal("isMorningSip")),
                            IsEveningSip = sqlRecipeStepReader.GetBoolean(sqlRecipeStepReader.GetOrdinal("isEveningSip")),
                            CreatedBy = sqlRecipeStepReader["createdBy"].ToString(),
                            CreateDate = Convert.ToDateTime(sqlRecipeStepReader["createDate"].ToString()),
                            ChangedBy = sqlRecipeStepReader["changedBy"].ToString(),
                            ChangeDate = Convert.ToDateTime(sqlRecipeStepReader["changeDate"].ToString())
                        };

                        List<RecipeStepLimit> recipeStepLimits = new();
                        MySqlConnection sqlRecipeStepLimitConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                        MySqlCommand sqlRecipeStepLimitCommand = new MySqlCommand("spGetRecipeStepLimitsByRecipeStepID", sqlRecipeStepLimitConnection);
                        sqlRecipeStepLimitCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlRecipeStepLimitCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

                        sqlRecipeStepLimitConnection.Open();
                        MySqlDataReader sqlRecipeStepLimitReader = sqlRecipeStepLimitCommand.ExecuteReader();
                        sqlRecipeStepLimitCommand.Dispose();

                        while (sqlRecipeStepLimitReader.Read())
                        {
                            RecipeStepLimit recipeStepLimit = new RecipeStepLimit
                            {
                                ID = Guid.Parse(sqlRecipeStepLimitReader["id"].ToString()),
                                RecipeStepID = Guid.Parse(sqlRecipeStepLimitReader["recipeStepID"].ToString()),
                                //WeekNumber = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("weekNumber")),
                                LocationID = Guid.Parse(sqlRecipeStepLimitReader["locationID"].ToString()),
                                LocationName = sqlRecipeStepLimitReader["locationName"].ToString(),
                                LocationSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("locationSort")),
                                MeasurementTypeID = Guid.Parse(sqlRecipeStepLimitReader["measurementTypeID"].ToString()),
                                MeasurementTypeName = sqlRecipeStepLimitReader["measurementTypeName"].ToString(),
                                MeasurementSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("measurementSort")),
                                LimitTypeID = Guid.Parse(sqlRecipeStepLimitReader["limitTypeID"].ToString()),
                                LimitTypeName = sqlRecipeStepLimitReader["limitTypeName"].ToString(),
                                LimitSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("limitSort")),
                                Value = sqlRecipeStepLimitReader.GetDecimal(sqlRecipeStepLimitReader.GetOrdinal("value")),
                                CreatedBy = sqlRecipeStepLimitReader["createdBy"].ToString(),
                                CreateDate = Convert.ToDateTime(sqlRecipeStepLimitReader["createDate"].ToString()),
                                ChangedBy = sqlRecipeStepLimitReader["changedBy"].ToString(),
                                ChangeDate = Convert.ToDateTime(sqlRecipeStepLimitReader["changeDate"].ToString())
                            };

                            recipeStepLimits.Add(recipeStepLimit);
                        }

                        sqlRecipeStepLimitReader.Close();
                        sqlRecipeStepLimitReader.Dispose();

                        sqlRecipeStepLimitConnection.Close();
                        sqlRecipeStepLimitConnection.Dispose();

                        recipeStep.RecipeStepLimits = recipeStepLimits;

                        List<RecipeStepAmount> recipeStepAmounts = new();
                        MySqlConnection sqlRecipeStepAmountConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                        MySqlCommand sqlRecipeStepAmountCommand = new MySqlCommand("spGetRecipeStepAmountsByRecipeStepID", sqlRecipeStepAmountConnection);
                        sqlRecipeStepAmountCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlRecipeStepAmountCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

                        sqlRecipeStepAmountConnection.Open();
                        MySqlDataReader sqlRecipeStepAmountReader = sqlRecipeStepAmountCommand.ExecuteReader();
                        sqlRecipeStepAmountCommand.Dispose();

                        while (sqlRecipeStepAmountReader.Read())
                        {
                            RecipeStepAmount recipeStepAmount = new RecipeStepAmount
                            {
                                ID = Guid.Parse(sqlRecipeStepAmountReader["id"].ToString()),
                                RecipeStepID = Guid.Parse(sqlRecipeStepAmountReader["recipeStepID"].ToString()),
                                //WeekNumber = sqlRecipeStepAmountReader.GetInt16(sqlRecipeStepAmountReader.GetOrdinal("weekNumber")),
                                ChemicalID = Guid.Parse(sqlRecipeStepAmountReader["chemicalID"].ToString()),
                                ChemicalName = sqlRecipeStepAmountReader["chemicalName"].ToString(),
                                ChemicalTypeID = Guid.Parse(sqlRecipeStepAmountReader["ChemicalTypeID"].ToString()),
                                Amount = sqlRecipeStepAmountReader.GetDecimal(sqlRecipeStepAmountReader.GetOrdinal("amount")),
                                CreatedBy = sqlRecipeStepAmountReader["createdBy"].ToString(),
                                CreateDate = Convert.ToDateTime(sqlRecipeStepAmountReader["createDate"].ToString()),
                                ChangedBy = sqlRecipeStepAmountReader["changedBy"].ToString(),
                                ChangeDate = Convert.ToDateTime(sqlRecipeStepAmountReader["changeDate"].ToString())
                            };

                            recipeStepAmounts.Add(recipeStepAmount);
                        }

                        sqlRecipeStepAmountReader.Close();
                        sqlRecipeStepAmountReader.Dispose();

                        sqlRecipeStepAmountConnection.Close();
                        sqlRecipeStepAmountConnection.Dispose();

                        recipeStep.RecipeStepAmounts = recipeStepAmounts;

                        recipeSteps.Add(recipeStep);
                     }

                    sqlRecipeStepReader.Close();
                    sqlRecipeStepReader.Dispose();

                    sqlRecipeStepConnection.Close();
                    sqlRecipeStepConnection.Dispose();

                    recipe.RecipeSteps = recipeSteps;

                    lstream.Add(recipe);
                }

                sqlDataReader.Close();
                sqlDataReader.Dispose();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return lstream;
        }

        public IEnumerable<RecipeChemList> GetRecipeChemListByRecipeID(Guid guid)
        {
            List<RecipeChemList> lstream = new();

            using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            {
                sqlConnection.Open();

                using (MySqlCommand sqlCommand = new())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "spGetRecipeChemListByRecipeID";
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("thisid", guid);

                    using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (sqlDataReader.Read())
                        {
                            RecipeChemList recipeChemList = new()
                            {
                                ID = Guid.Parse(sqlDataReader["id"].ToString()),
                                RecipeID = Guid.Parse(sqlDataReader["recipeID"].ToString()),
                                ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString()),
                                Mixtime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixtime")),
                                CreatedBy = sqlDataReader["createdBy"].ToString(),
                                CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
                                ChangedBy = sqlDataReader["changedBy"].ToString(),
                                ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
                            };

                            lstream.Add(recipeChemList);
                        }
                    }
                }
            }

            return lstream;
        }
    }
}
