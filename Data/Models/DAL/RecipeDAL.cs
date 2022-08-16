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

        public async Task AddRecipeAsync(Recipe recipe)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddRecipe";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("id", recipe.ID.ToString());
                sqlCommand.Parameters.AddWithValue("name", recipe.Name);
                sqlCommand.Parameters.AddWithValue("createdBy", recipe.CreatedBy);
                sqlCommand.Parameters.AddWithValue("createDate", recipe.CreateDate);
                sqlCommand.Parameters.AddWithValue("changedBy", recipe.ChangedBy);
                sqlCommand.Parameters.AddWithValue("changeDate", recipe.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
        public async Task DeleteRecipeAsync(Guid id)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRecipe";
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

        public async Task UpdateRecipeNameAsync(Recipe recipe)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeName";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisname", recipe.Name);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipe.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipe.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepLimitAsync(RecipeStepLimit recipeStepLimit)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepLimit";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStepLimit.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisvalue", recipeStepLimit.Value);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepLimit.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepLimit.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepIrrigationAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepIrrigation";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisirrigationEventsPerDay", recipeStep.IrrigationEventsPerDay);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepAmountAsync(RecipeStepAmount recipeStepAmount)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepAmount";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStepAmount.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisamount", recipeStepAmount.Amount);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStepAmount.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStepAmount.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepSoaktimeAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepSoaktime";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thissoakTime", recipeStep.SoakTime);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepMorningSipAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepMorningSip";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisisMorningSip", recipeStep.IsMorningSip);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepEveningSipAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepEveningSip";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thisisEveningSip", recipeStep.IsEveningSip);
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateRecipeStepLightCycleAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRecipeStepLightCycle";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());
                sqlCommand.Parameters.AddWithValue("thislightCycleID", recipeStep.LightCycleID.ToString());
                sqlCommand.Parameters.AddWithValue("thischangedBy", recipeStep.ChangedBy);
                sqlCommand.Parameters.AddWithValue("thischangeDate", recipeStep.ChangeDate);

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteRecipeStepAsync(Guid guid)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRecipeStep";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteRecipeStepLimitAsync(Guid guid)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRecipeStepLimit";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteRecipeStepAmountAsync(Guid guid)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRecipeStepAmount";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddRecipeStepLimitAsync(RecipeStepLimit recipeStepLimit)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
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

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddRecipeStepAmountAsync(RecipeStepAmount recipeStepAmount)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
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

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddRecipeStepAsync(RecipeStep recipeStep)
        {
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
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

                await sqlConnection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<RecipeStep> GetLastRecipeStepByRecipeIDAsync(Guid guid)
        {
            RecipeStep recipeStep = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLastRecipeStepByRecipeID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", guid.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return recipeStep;
        }
        
        public async Task<decimal> GetLastRecipeStepLimitValueAsync(Guid recipeID, 
            Guid locationTypeID, Guid measurementTypeID, Guid limitTypeID)
        {
            decimal tmpDec = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLastRecipeStepLimitValue";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisrecipeID", recipeID.ToString());
                sqlCommand.Parameters.AddWithValue("thislocationTypeID", locationTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("thismeasurementTypeID", measurementTypeID.ToString());
                sqlCommand.Parameters.AddWithValue("thislimitTypeID", limitTypeID.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        tmpDec = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("value"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tmpDec;
        }

        public async Task<decimal> GetLastRecipeStepAmountValueAsync(Guid recipeID, Guid chemicalID)
        {
            decimal tmpDec = 0;
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetLastRecipeStepAmountValue";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisrecipeID", recipeID.ToString());
                sqlCommand.Parameters.AddWithValue("thischemicalID", chemicalID.ToString());

                await sqlConnection.OpenAsync();

                await using (MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync())
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        tmpDec = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("amount"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return tmpDec;
        }

        public async Task<IEnumerable<RecipeChemListViewModel>> GetRecipeChemListViewModelsAsync()
        {
            List<RecipeChemListViewModel> lstream = new();
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRecipeChemListViewModels";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
        
        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            List<Recipe> lstream = new();
            //using (MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection")))
            //{
            //    MySqlCommand sqlCommand = new MySqlCommand("spGetRecipes", sqlConnection);
            //    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //    sqlConnection.Open();
            //    MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            //    sqlCommand.Dispose();

            //    while (sqlDataReader.Read())
            //    {
            //        Recipe recipe = new Recipe
            //        {
            //            ID = Guid.Parse(sqlDataReader["id"].ToString()),
            //            Name = sqlDataReader["name"].ToString(),
            //            CreatedBy = sqlDataReader["createdBy"].ToString(),
            //            CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
            //            ChangedBy = sqlDataReader["changedBy"].ToString(),
            //            ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString())
            //        };

            //        List<RecipeChemList> recipeChemLists = new();
            //        MySqlConnection sqlRecipeChemListConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
            //        MySqlCommand sqlRecipeChemListCommand = new MySqlCommand("spGetRecipeChemListByRecipeID", sqlRecipeChemListConnection);
            //        sqlRecipeChemListCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlRecipeChemListCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

            //        sqlRecipeChemListConnection.Open();
            //        MySqlDataReader sqlRecipeChemListReader = sqlRecipeChemListCommand.ExecuteReader();
            //        sqlRecipeChemListCommand.Dispose();

            //        while (sqlRecipeChemListReader.Read())
            //        {
            //            RecipeChemList recipeChemList = new RecipeChemList
            //            {
            //                ID = Guid.Parse(sqlRecipeChemListReader["id"].ToString()),
            //                RecipeID = Guid.Parse(sqlRecipeChemListReader["recipeID"].ToString()),
            //                ChemicalID = Guid.Parse(sqlRecipeChemListReader["chemicalID"].ToString()),
            //                ChemicalName = sqlRecipeChemListReader["chemicalName"].ToString(),
            //                Mixtime = sqlRecipeChemListReader.GetInt16(sqlRecipeChemListReader.GetOrdinal("mixtime")),
            //                //Priority = sqlRecipeChemListReader.GetInt16(sqlRecipeChemListReader.GetOrdinal("priority")),
            //                CreatedBy = sqlRecipeChemListReader["createdBy"].ToString(),
            //                CreateDate = Convert.ToDateTime(sqlRecipeChemListReader["createDate"].ToString()),
            //                ChangedBy = sqlRecipeChemListReader["changedBy"].ToString(),
            //                ChangeDate = Convert.ToDateTime(sqlRecipeChemListReader["changeDate"].ToString())

            //            };

            //            recipeChemLists.Add(recipeChemList);
            //        }

            //        sqlRecipeChemListReader.Close();
            //        sqlRecipeChemListReader.Dispose();

            //        sqlRecipeChemListConnection.Close();
            //        sqlRecipeChemListConnection.Dispose();

            //        recipe.RecipeChems = recipeChemLists;

            //        List<RecipeStep> recipeSteps = new();
            //        MySqlConnection sqlRecipeStepConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
            //        MySqlCommand sqlRecipeStepCommand = new MySqlCommand("spGetRecipeStepsByRecipeID", sqlRecipeStepConnection);
            //        sqlRecipeStepCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlRecipeStepCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

            //        sqlRecipeStepConnection.Open();
            //        MySqlDataReader sqlRecipeStepReader = sqlRecipeStepCommand.ExecuteReader();
            //        sqlRecipeStepCommand.Dispose();

            //        while (sqlRecipeStepReader.Read())
            //        {
            //            RecipeStep recipeStep = new RecipeStep
            //            {
            //                ID = Guid.Parse(sqlRecipeStepReader["id"].ToString()),
            //                RecipeID = Guid.Parse(sqlRecipeStepReader["recipeID"].ToString()),
            //                WeekNumber = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("weekNumber")),
            //                LightCycleID = Guid.Parse(sqlRecipeStepReader["lightCycleID"].ToString()),
            //                LightCycleName = sqlRecipeStepReader["lightCycleName"].ToString(),
            //                IrrigationEventsPerDay = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("irrigationEventsPerDay")),
            //                SoakTime = sqlRecipeStepReader.GetInt16(sqlRecipeStepReader.GetOrdinal("soakTime")),
            //                IsMorningSip = sqlRecipeStepReader.GetBoolean(sqlRecipeStepReader.GetOrdinal("isMorningSip")),
            //                IsEveningSip = sqlRecipeStepReader.GetBoolean(sqlRecipeStepReader.GetOrdinal("isEveningSip")),
            //                CreatedBy = sqlRecipeStepReader["createdBy"].ToString(),
            //                CreateDate = Convert.ToDateTime(sqlRecipeStepReader["createDate"].ToString()),
            //                ChangedBy = sqlRecipeStepReader["changedBy"].ToString(),
            //                ChangeDate = Convert.ToDateTime(sqlRecipeStepReader["changeDate"].ToString())
            //            };

            //            List<RecipeStepLimit> recipeStepLimits = new();
            //            MySqlConnection sqlRecipeStepLimitConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
            //            MySqlCommand sqlRecipeStepLimitCommand = new MySqlCommand("spGetRecipeStepLimitsByRecipeStepID", sqlRecipeStepLimitConnection);
            //            sqlRecipeStepLimitCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //            sqlRecipeStepLimitCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

            //            sqlRecipeStepLimitConnection.Open();
            //            MySqlDataReader sqlRecipeStepLimitReader = sqlRecipeStepLimitCommand.ExecuteReader();
            //            sqlRecipeStepLimitCommand.Dispose();

            //            while (sqlRecipeStepLimitReader.Read())
            //            {
            //                RecipeStepLimit recipeStepLimit = new RecipeStepLimit
            //                {
            //                    ID = Guid.Parse(sqlRecipeStepLimitReader["id"].ToString()),
            //                    RecipeStepID = Guid.Parse(sqlRecipeStepLimitReader["recipeStepID"].ToString()),
            //                    //WeekNumber = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("weekNumber")),
            //                    LocationID = Guid.Parse(sqlRecipeStepLimitReader["locationID"].ToString()),
            //                    LocationName = sqlRecipeStepLimitReader["locationName"].ToString(),
            //                    LocationSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("locationSort")),
            //                    MeasurementTypeID = Guid.Parse(sqlRecipeStepLimitReader["measurementTypeID"].ToString()),
            //                    MeasurementTypeName = sqlRecipeStepLimitReader["measurementTypeName"].ToString(),
            //                    MeasurementSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("measurementSort")),
            //                    LimitTypeID = Guid.Parse(sqlRecipeStepLimitReader["limitTypeID"].ToString()),
            //                    LimitTypeName = sqlRecipeStepLimitReader["limitTypeName"].ToString(),
            //                    LimitSort = sqlRecipeStepLimitReader.GetInt16(sqlRecipeStepLimitReader.GetOrdinal("limitSort")),
            //                    Value = sqlRecipeStepLimitReader.GetDecimal(sqlRecipeStepLimitReader.GetOrdinal("value")),
            //                    CreatedBy = sqlRecipeStepLimitReader["createdBy"].ToString(),
            //                    CreateDate = Convert.ToDateTime(sqlRecipeStepLimitReader["createDate"].ToString()),
            //                    ChangedBy = sqlRecipeStepLimitReader["changedBy"].ToString(),
            //                    ChangeDate = Convert.ToDateTime(sqlRecipeStepLimitReader["changeDate"].ToString())
            //                };

            //                recipeStepLimits.Add(recipeStepLimit);
            //            }

            //            sqlRecipeStepLimitReader.Close();
            //            sqlRecipeStepLimitReader.Dispose();

            //            sqlRecipeStepLimitConnection.Close();
            //            sqlRecipeStepLimitConnection.Dispose();

            //            recipeStep.RecipeStepLimits = recipeStepLimits;

            //            List<RecipeStepAmount> recipeStepAmounts = new();
            //            MySqlConnection sqlRecipeStepAmountConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
            //            MySqlCommand sqlRecipeStepAmountCommand = new MySqlCommand("spGetRecipeStepAmountsByRecipeStepID", sqlRecipeStepAmountConnection);
            //            sqlRecipeStepAmountCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //            sqlRecipeStepAmountCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

            //            sqlRecipeStepAmountConnection.Open();
            //            MySqlDataReader sqlRecipeStepAmountReader = sqlRecipeStepAmountCommand.ExecuteReader();
            //            sqlRecipeStepAmountCommand.Dispose();

            //            while (sqlRecipeStepAmountReader.Read())
            //            {
            //                RecipeStepAmount recipeStepAmount = new RecipeStepAmount
            //                {
            //                    ID = Guid.Parse(sqlRecipeStepAmountReader["id"].ToString()),
            //                    RecipeStepID = Guid.Parse(sqlRecipeStepAmountReader["recipeStepID"].ToString()),
            //                    //WeekNumber = sqlRecipeStepAmountReader.GetInt16(sqlRecipeStepAmountReader.GetOrdinal("weekNumber")),
            //                    ChemicalID = Guid.Parse(sqlRecipeStepAmountReader["chemicalID"].ToString()),
            //                    ChemicalName = sqlRecipeStepAmountReader["chemicalName"].ToString(),
            //                    ChemicalTypeID = Guid.Parse(sqlRecipeStepAmountReader["ChemicalTypeID"].ToString()),
            //                    Amount = sqlRecipeStepAmountReader.GetDecimal(sqlRecipeStepAmountReader.GetOrdinal("amount")),
            //                    CreatedBy = sqlRecipeStepAmountReader["createdBy"].ToString(),
            //                    CreateDate = Convert.ToDateTime(sqlRecipeStepAmountReader["createDate"].ToString()),
            //                    ChangedBy = sqlRecipeStepAmountReader["changedBy"].ToString(),
            //                    ChangeDate = Convert.ToDateTime(sqlRecipeStepAmountReader["changeDate"].ToString())
            //                };

            //                recipeStepAmounts.Add(recipeStepAmount);
            //            }

            //            sqlRecipeStepAmountReader.Close();
            //            sqlRecipeStepAmountReader.Dispose();

            //            sqlRecipeStepAmountConnection.Close();
            //            sqlRecipeStepAmountConnection.Dispose();

            //            recipeStep.RecipeStepAmounts = recipeStepAmounts;

            //            recipeSteps.Add(recipeStep);
            //        }

            //        sqlRecipeStepReader.Close();
            //        sqlRecipeStepReader.Dispose();

            //        sqlRecipeStepConnection.Close();
            //        sqlRecipeStepConnection.Dispose();

            //        recipe.RecipeSteps = recipeSteps;

            //        lstream.Add(recipe);
            //    }

            //    sqlDataReader.Close();
            //    sqlDataReader.Dispose();

            //    sqlConnection.Close();
            //    sqlConnection.Dispose();
            //}
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRecipes";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
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

                    using MySqlConnection sqlRecipeChemListConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                    using MySqlCommand sqlRecipeChemListCommand = new();
                    sqlRecipeChemListCommand.Connection = sqlRecipeChemListConnection;
                    sqlRecipeChemListCommand.CommandText = "spGetRecipeChemListByRecipeID";
                    sqlRecipeChemListCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlRecipeChemListCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

                    await sqlRecipeChemListConnection.OpenAsync();

                    await using MySqlDataReader sqlRecipeChemListReader = (MySqlDataReader)await sqlRecipeChemListCommand.ExecuteReaderAsync();
                    while (await sqlRecipeChemListReader.ReadAsync())
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

                    recipe.RecipeChems = recipeChemLists;

                    List<RecipeStep> recipeSteps = new();
                    using MySqlConnection sqlRecipeStepConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                    using MySqlCommand sqlRecipeStepCommand = new();
                    sqlRecipeStepCommand.Connection = sqlRecipeStepConnection;
                    sqlRecipeStepCommand.CommandText = "spGetRecipeStepsByRecipeID";
                    sqlRecipeStepCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlRecipeStepCommand.Parameters.AddWithValue("thisid", recipe.ID.ToString());

                    await sqlRecipeStepConnection.OpenAsync();

                    await using MySqlDataReader sqlRecipeStepReader = (MySqlDataReader)await sqlRecipeStepCommand.ExecuteReaderAsync();
                    while (await sqlRecipeStepReader.ReadAsync())
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
                        using MySqlConnection sqlRecipeStepLimitConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                        using MySqlCommand sqlRecipeStepLimitCommand = new();
                        sqlRecipeStepLimitCommand.Connection = sqlRecipeStepLimitConnection;
                        sqlRecipeStepLimitCommand.CommandText = "spGetRecipeStepLimitsByRecipeStepID";
                        sqlRecipeStepLimitCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlRecipeStepLimitCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

                        await sqlRecipeStepLimitConnection.OpenAsync();

                        await using MySqlDataReader sqlRecipeStepLimitReader = (MySqlDataReader)await sqlRecipeStepLimitCommand.ExecuteReaderAsync();
                        while (await sqlRecipeStepLimitReader.ReadAsync())
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

                        recipeStep.RecipeStepLimits = recipeStepLimits;

                        List<RecipeStepAmount> recipeStepAmounts = new();
                        using MySqlConnection sqlRecipeStepAmountConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                        using MySqlCommand sqlRecipeStepAmountCommand = new();
                        sqlRecipeStepAmountCommand.Connection = sqlRecipeStepAmountConnection;
                        sqlRecipeStepAmountCommand.CommandText = "spGetRecipeStepAmountsByRecipeStepID";
                        sqlRecipeStepAmountCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlRecipeStepAmountCommand.Parameters.AddWithValue("thisid", recipeStep.ID.ToString());

                        await sqlRecipeStepAmountConnection.OpenAsync();

                        await using MySqlDataReader sqlRecipeStepAmountReader = (MySqlDataReader)await sqlRecipeStepAmountCommand.ExecuteReaderAsync();
                        while (await sqlRecipeStepAmountReader.ReadAsync())
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

                        recipeStep.RecipeStepAmounts = recipeStepAmounts;

                        recipeSteps.Add(recipeStep);
                    }

                    recipe.RecipeSteps = recipeSteps;

                    lstream.Add(recipe);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lstream;
        }

        public async Task<IEnumerable<RecipeChemList>> GetRecipeChemListByRecipeIDAsync(Guid guid)
        {
            List<RecipeChemList> lstream = new();

            //using (MySqlConnection sqlConnection = new(_config.GetConnectionString("GardenConnection")))
            //{
            //    sqlConnection.Open();

            //    using (MySqlCommand sqlCommand = new())
            //    {
            //        sqlCommand.Connection = sqlConnection;
            //        sqlCommand.CommandText = "spGetRecipeChemListByRecipeID";
            //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //        sqlCommand.Parameters.AddWithValue("thisid", guid);

            //        using (MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            //        {
            //            while (sqlDataReader.Read())
            //            {
            //                RecipeChemList recipeChemList = new()
            //                {
            //                    ID = Guid.Parse(sqlDataReader["id"].ToString()),
            //                    RecipeID = Guid.Parse(sqlDataReader["recipeID"].ToString()),
            //                    ChemicalID = Guid.Parse(sqlDataReader["chemicalID"].ToString()),
            //                    Mixtime = sqlDataReader.GetInt16(sqlDataReader.GetOrdinal("mixtime")),
            //                    CreatedBy = sqlDataReader["createdBy"].ToString(),
            //                    CreateDate = Convert.ToDateTime(sqlDataReader["createDate"].ToString()),
            //                    ChangedBy = sqlDataReader["changedBy"].ToString(),
            //                    ChangeDate = Convert.ToDateTime(sqlDataReader["changeDate"].ToString()),
            //                };

            //                lstream.Add(recipeChemList);
            //            }
            //        }
            //    }
            //}
            try
            {
                using MySqlConnection sqlConnection = new MySqlConnection(_config.GetConnectionString("GardenConnection"));
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRecipeChemListByRecipeID";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", guid);

                await sqlConnection.OpenAsync();

                await using MySqlDataReader sqlDataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }
    }
}
