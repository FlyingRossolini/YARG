using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using YARG.Models;

namespace YARG.Data
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserLoginStore<ApplicationUser>
    {
        private readonly string _connectionString;

        public UserStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("IdentityConnection");
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddUser";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisUserName", user.UserName);
                sqlCommand.Parameters.AddWithValue("thisNormalizedUserName", user.NormalizedUserName);
                sqlCommand.Parameters.AddWithValue("thisEmail", user.Email);
                sqlCommand.Parameters.AddWithValue("thisNormalizedEmail", user.NormalizedEmail);
                sqlCommand.Parameters.AddWithValue("thisEmailConfirmed", user.EmailConfirmed);
                sqlCommand.Parameters.AddWithValue("thisPasswordHash", user.PasswordHash);
                sqlCommand.Parameters.AddWithValue("thisPhoneNumber", user.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("thisPhoneNumberConfirmed", user.PhoneNumberConfirmed);
                sqlCommand.Parameters.AddWithValue("thisTwoFactorEnabled", user.TwoFactorEnabled);
                sqlCommand.Parameters.Add("LID", MySqlDbType.Int32);
                sqlCommand.Parameters["LID"].Direction = System.Data.ParameterDirection.Output;

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

                user.Id = (int)sqlCommand.Parameters["LID"].Value;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteUser";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUser applicationUser = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindUserById";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", userId);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationUser.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationUser.UserName = sqlDataReader["UserName"].ToString();
                    applicationUser.NormalizedUserName = sqlDataReader["NormalizedUserName"].ToString();
                    applicationUser.Email = sqlDataReader["Email"].ToString();
                    applicationUser.NormalizedEmail = sqlDataReader["NormalizedEmail"].ToString();
                    applicationUser.EmailConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("EmailConfirmed"));

                    if (sqlDataReader["PasswordHash"] != DBNull.Value)
                    {
                        applicationUser.PasswordHash = sqlDataReader["PasswordHash"].ToString();
                    }

                    applicationUser.PhoneNumber = sqlDataReader["PhoneNumber"].ToString();
                    applicationUser.PhoneNumberConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("PhoneNumberConfirmed"));
                    applicationUser.TwoFactorEnabled = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("TwoFactorEnabled"));
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

            return applicationUser;
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUser applicationUser = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindUserByName";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisname", normalizedUserName);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationUser.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationUser.UserName = sqlDataReader["UserName"].ToString();
                    applicationUser.NormalizedUserName = sqlDataReader["NormalizedUserName"].ToString();
                    applicationUser.Email = sqlDataReader["Email"].ToString();
                    applicationUser.NormalizedEmail = sqlDataReader["NormalizedEmail"].ToString();
                    applicationUser.EmailConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("EmailConfirmed"));

                    if (sqlDataReader["PasswordHash"] != DBNull.Value)
                    {
                        applicationUser.PasswordHash = sqlDataReader["PasswordHash"].ToString();
                    }

                    applicationUser.PhoneNumber = sqlDataReader["PhoneNumber"].ToString();
                    applicationUser.PhoneNumberConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("PhoneNumberConfirmed"));
                    applicationUser.TwoFactorEnabled = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("TwoFactorEnabled"));
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

            return applicationUser;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateUser";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);
                sqlCommand.Parameters.AddWithValue("thisUserName", user.UserName);
                sqlCommand.Parameters.AddWithValue("thisNormalizedUserName", user.NormalizedUserName);
                sqlCommand.Parameters.AddWithValue("thisEmail", user.Email);
                sqlCommand.Parameters.AddWithValue("thisNormalizedEmail", user.NormalizedEmail);
                sqlCommand.Parameters.AddWithValue("thisEmailConfirmed", user.EmailConfirmed);
                sqlCommand.Parameters.AddWithValue("thisPasswordHash", user.PasswordHash);
                sqlCommand.Parameters.AddWithValue("thisPhoneNumber", user.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("thisPhoneNumberConfirmed", user.PhoneNumberConfirmed);
                sqlCommand.Parameters.AddWithValue("thisTwoFactorEnabled", user.TwoFactorEnabled);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            return IdentityResult.Success;
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUser applicationUser = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindByEmail";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisNormalizedEmail", normalizedEmail);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationUser.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationUser.UserName = sqlDataReader["UserName"].ToString();
                    applicationUser.NormalizedUserName = sqlDataReader["NormalizedUserName"].ToString();
                    applicationUser.Email = sqlDataReader["Email"].ToString();
                    applicationUser.NormalizedEmail = sqlDataReader["NormalizedEmail"].ToString();
                    applicationUser.EmailConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("EmailConfirmed"));

                    if (sqlDataReader["PasswordHash"] != DBNull.Value)
                    {
                        applicationUser.PasswordHash = sqlDataReader["PasswordHash"].ToString();
                    }

                    applicationUser.PhoneNumber = sqlDataReader["PhoneNumber"].ToString();
                    applicationUser.PhoneNumberConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("PhoneNumberConfirmed"));
                    applicationUser.TwoFactorEnabled = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("TwoFactorEnabled"));
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

            return applicationUser;
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var normalizedName = roleName.ToUpper();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRoleIdFromNormalizedName";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisNormalizedName", normalizedName);

                await sqlConnection.OpenAsync(cancellationToken);
                var roleId = (int?)await sqlCommand.ExecuteScalarAsync(cancellationToken);

                if (!roleId.HasValue)
                {
                    using MySqlConnection mySqlConnectionNewRole = new(_connectionString);
                    using MySqlCommand mySqlCommandNewRole = new();
                    mySqlCommandNewRole.Connection = mySqlConnectionNewRole;
                    mySqlCommandNewRole.CommandText = "spAddRole";
                    mySqlCommandNewRole.CommandType = System.Data.CommandType.StoredProcedure;
                    mySqlCommandNewRole.Parameters.AddWithValue("thisName", roleName);
                    mySqlCommandNewRole.Parameters.AddWithValue("thisNormalizedName", roleName.ToUpper());
                    mySqlCommandNewRole.Parameters.Add("LID", MySqlDbType.Int32);
                    mySqlCommandNewRole.Parameters["LID"].Direction = System.Data.ParameterDirection.Output;

                    await mySqlConnectionNewRole.OpenAsync(cancellationToken);
                    await mySqlCommandNewRole.ExecuteNonQueryAsync(cancellationToken);

                    roleId = (int)mySqlCommandNewRole.Parameters["LID"].Value;
                }

                using MySqlConnection sqlConnectionUserIdFromEmail = new(_connectionString);
                using MySqlCommand sqlCommandUserIdFromEmail = new();
                sqlCommandUserIdFromEmail.Connection = sqlConnectionUserIdFromEmail;
                sqlCommandUserIdFromEmail.CommandText = "spGetUserIdFromEmail";
                sqlCommandUserIdFromEmail.Parameters.AddWithValue("thisEmail", user.Email);
                sqlCommandUserIdFromEmail.CommandType = System.Data.CommandType.StoredProcedure;

                await sqlConnectionUserIdFromEmail.OpenAsync(cancellationToken);
                var userId = (int?)await sqlCommandUserIdFromEmail.ExecuteScalarAsync(cancellationToken);


                if (userId.HasValue)
                {
                    using MySqlConnection mySqlConnectionAddToRole = new(_connectionString);
                    using MySqlCommand sqlCommandAddToRole = new();
                    sqlCommandAddToRole.Connection = mySqlConnectionAddToRole;
                    sqlCommandAddToRole.CommandText = "spAddToRole";
                    sqlCommandAddToRole.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommandAddToRole.Parameters.AddWithValue("thisUserId", userId);
                    sqlCommandAddToRole.Parameters.AddWithValue("thisRoleId", roleId);

                    await mySqlConnectionAddToRole.OpenAsync(cancellationToken);
                    await sqlCommandAddToRole.ExecuteNonQueryAsync(cancellationToken);

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
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            //UNTESTED SINCE MOVE TO DEDICATED IDENTITY DB

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteUserRole";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisUserId", user.UserName);
                sqlCommand.Parameters.AddWithValue("thisRoleId", user.NormalizedUserName);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

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

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            //UNTESTED SINCE MOVE TO DEDICATED IDENTITY DB

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();

            List<string> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRolesByUserId";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisUserId", user.Id);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while(await sqlDataReader.ReadAsync(cancellationToken))
                {
                    lstream.Add(sqlDataReader["Name"].ToString());
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

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            long matchingRoles = 0;
            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetRoleIdFromNormalizedName";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisNormalizedName", roleName.ToUpper());

                await sqlConnection.OpenAsync(cancellationToken);

                int roleId = (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
                if (roleId == default) return false;

                using MySqlConnection sqlConnectionGetCountOfRoles = new(_connectionString);
                using MySqlCommand sqlCommandGetCountOfRoles = new();
                sqlCommandGetCountOfRoles.Connection = sqlConnection;
                sqlCommandGetCountOfRoles.CommandText = "spGetCountOfRolesForUserId";
                sqlCommandGetCountOfRoles.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommandGetCountOfRoles.Parameters.AddWithValue("thisUserId", user.Id);
                sqlCommandGetCountOfRoles.Parameters.AddWithValue("thisRoleId", roleId);

                await sqlConnectionGetCountOfRoles.OpenAsync(cancellationToken);
                matchingRoles = (long)await sqlCommandGetCountOfRoles.ExecuteScalarAsync(cancellationToken);

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }

            //using var connection = new MySqlConnection(_connectionString);
            //var roleId = await connection.ExecuteScalarAsync<int?>("SELECT Id FROM ApplicationRole WHERE NormalizedName = @normalizedName", new { normalizedName = roleName.ToUpper() });
            //if (roleId == default(int)) return false;
            //var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ApplicationUserRole WHERE UserId = @userId AND RoleId = @{nameof(roleId)}",
            //    new { userId = user.Id, roleId });

            return matchingRoles > 0;
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            //UNTESTED SINCE MOVE TO DEDICATED IDENTITY DB


            cancellationToken.ThrowIfCancellationRequested();
            List<ApplicationUser> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spGetUsersInRole";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisroleName", roleName);

                await sqlConnection.OpenAsync(cancellationToken);

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    ApplicationUser applicationUser = new()
                    {
                        Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                        UserName = sqlDataReader["UserName"].ToString(),
                        NormalizedUserName = sqlDataReader["NormalizedUserName"].ToString(),
                        Email = sqlDataReader["Email"].ToString(),
                        NormalizedEmail = sqlDataReader["NormalizedEmail"].ToString(),
                        EmailConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("EmailConfirmed")),
                        PasswordHash = sqlDataReader["PasswordHash"].ToString(),
                        PhoneNumber = sqlDataReader["PhoneNumber"].ToString(),
                        PhoneNumberConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("PhoneNumberConfirmed")),
                        TwoFactorEnabled = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("TwoFactorEnabled"))
                    };

                    lstream.Add(applicationUser);

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
        
        public void Dispose()
        {
            // Nothing to dispose.
        }

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);
            
            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLogin";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);
                sqlCommand.Parameters.AddWithValue("thisLoginProvider", login.LoginProvider);
                sqlCommand.Parameters.AddWithValue("thisProviderKey", login.ProviderKey);
                sqlCommand.Parameters.AddWithValue("thisDisplayName", login.ProviderDisplayName);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
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

        public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteByLogin";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);
                sqlCommand.Parameters.AddWithValue("loginProvider", loginProvider);
                sqlCommand.Parameters.AddWithValue("providerKey", providerKey);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

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

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();
            List<UserLoginInfo> lstream = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindByUserId";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);

                await sqlConnection.OpenAsync(cancellationToken);

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    UserLoginInfo userLoginInfo = new(
                        sqlDataReader["LoginProvider"].ToString(), 
                        sqlDataReader["ProviderKey"].ToString(), 
                        sqlDataReader["ProviderKey"].ToString()
                        );
                    lstream.Add(userLoginInfo);
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

        public async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ApplicationUser applicationUser = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindByLogin";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("loginProvider", loginProvider);
                sqlCommand.Parameters.AddWithValue("providerKey", providerKey);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationUser.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationUser.UserName = sqlDataReader["UserName"].ToString();
                    applicationUser.NormalizedUserName = sqlDataReader["NormalizedUserName"].ToString();
                    applicationUser.Email = sqlDataReader["Email"].ToString();
                    applicationUser.NormalizedEmail = sqlDataReader["NormalizedEmail"].ToString();
                    applicationUser.EmailConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("EmailConfirmed"));

                    if (sqlDataReader["PasswordHash"] != DBNull.Value)
                    {
                        applicationUser.PasswordHash = sqlDataReader["PasswordHash"].ToString();
                    }

                    applicationUser.PhoneNumber = sqlDataReader["PhoneNumber"].ToString();
                    applicationUser.PhoneNumberConfirmed = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("PhoneNumberConfirmed"));
                    applicationUser.TwoFactorEnabled = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("TwoFactorEnabled"));
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

            if (applicationUser.Id != 0)
            {
                return applicationUser;
            }
            else
            {
                return await Task.FromResult<ApplicationUser>(null);
            }
        }
    }
}
