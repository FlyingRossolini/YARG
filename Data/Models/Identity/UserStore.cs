using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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
            _connectionString = configuration.GetConnectionString("GardenConnection");
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO ApplicationUser (UserName, NormalizedUserName, Email,
                    NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled)
                    VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                    @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.PasswordHash)},
                    @{nameof(ApplicationUser.PhoneNumber)}, @{nameof(ApplicationUser.PhoneNumberConfirmed)}, @{nameof(ApplicationUser.TwoFactorEnabled)});
                    SELECT LAST_INSERT_ID()", user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM ApplicationUser WHERE Id = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM ApplicationUser
                    WHERE Id = @{nameof(userId)}", new { userId });
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM ApplicationUser
                    WHERE NormalizedUserName = @{nameof(normalizedUserName)}", new { normalizedUserName });
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

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE ApplicationUser SET
                    UserName = @{nameof(ApplicationUser.UserName)},
                    NormalizedUserName = @{nameof(ApplicationUser.NormalizedUserName)},
                    Email = @{nameof(ApplicationUser.Email)},
                    NormalizedEmail = @{nameof(ApplicationUser.NormalizedEmail)},
                    EmailConfirmed = @{nameof(ApplicationUser.EmailConfirmed)},
                    PasswordHash = @{nameof(ApplicationUser.PasswordHash)},
                    PhoneNumber = @{nameof(ApplicationUser.PhoneNumber)},
                    PhoneNumberConfirmed = @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                    TwoFactorEnabled = @{nameof(ApplicationUser.TwoFactorEnabled)}
                    WHERE Id = @{nameof(ApplicationUser.Id)}", user);
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

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM ApplicationUser
                    WHERE NormalizedEmail = @{nameof(normalizedEmail)}", new { normalizedEmail });
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
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var normalizedName = roleName.ToUpper();
            var roleId = await connection.ExecuteScalarAsync<int?>($"SELECT Id FROM ApplicationRole WHERE NormalizedName = @{nameof(normalizedName)}", new { normalizedName });
            if (!roleId.HasValue)
                roleId = await connection.ExecuteAsync($"INSERT INTO ApplicationRole(Name, NormalizedName) VALUES(@{nameof(roleName)}, @{nameof(normalizedName)})",
                    new { roleName, normalizedName });

            await connection.ExecuteAsync($"IF NOT EXISTS(SELECT 1 FROM ApplicationUserRole WHERE UserId = @userId AND RoleId = @{nameof(roleId)}) " +
                $"INSERT INTO ApplicationUserRole(UserId, RoleId) VALUES(@userId, @{nameof(roleId)})",
                new { userId = user.Id, roleId });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var roleId = await connection.ExecuteScalarAsync<int?>("SELECT Id FROM ApplicationRole WHERE NormalizedName = @normalizedName", new { normalizedName = roleName.ToUpper() });
            if (!roleId.HasValue)
                await connection.ExecuteAsync($"DELETE FROM ApplicationUserRole WHERE UserId = @userId AND RoleId = @{nameof(roleId)}", new { userId = user.Id, roleId });
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var queryResults = await connection.QueryAsync<string>("SELECT r.Name FROM ApplicationRole r INNER JOIN ApplicationUserRole ur ON ur.RoleId = r.Id " +
                "WHERE ur.UserId = @userId", new { userId = user.Id });

            return queryResults.ToList();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            var roleId = await connection.ExecuteScalarAsync<int?>("SELECT Id FROM ApplicationRole WHERE NormalizedName = @normalizedName", new { normalizedName = roleName.ToUpper() });
            if (roleId == default(int)) return false;
            var matchingRoles = await connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ApplicationUserRole WHERE UserId = @userId AND RoleId = @{nameof(roleId)}",
                new { userId = user.Id, roleId });

            return matchingRoles > 0;
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new MySqlConnection(_connectionString);
            var queryResults = await connection.QueryAsync<ApplicationUser>("SELECT u.* FROM ApplicationUser u " +
                "INNER JOIN ApplicationUserRole ur ON ur.UserId = u.Id INNER JOIN ApplicationRole r ON r.Id = ur.RoleId WHERE r.NormalizedName = @normalizedName",
                new { normalizedName = roleName.ToUpper() });

            return queryResults.ToList();
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

            try
            {
                using MySqlConnection sqlConnection = new(_connectionString);
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spAddLogin";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);
                sqlCommand.Parameters.AddWithValue("thisLogin_Provider", login.LoginProvider);
                sqlCommand.Parameters.AddWithValue("thisProvider_Key", login.ProviderKey);
                sqlCommand.Parameters.AddWithValue("thisDisplay_Name", login.ProviderDisplayName);

                await sqlConnection.OpenAsync(cancellationToken);
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                using MySqlConnection sqlConnection = new(_connectionString);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            try
            {
                using MySqlConnection sqlConnection = new(_connectionString);
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindByUserId";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", user.Id);

                await sqlConnection.OpenAsync(cancellationToken);

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (await sqlDataReader.ReadAsync())
                {
                    UserLoginInfo userLoginInfo = new(sqlDataReader["Login_Provider"].ToString(), sqlDataReader["Provider_Key"].ToString(), sqlDataReader["Provider_Key"].ToString());
                    lstream.Add(userLoginInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstream;
        }

        public async Task<ApplicationUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ApplicationUser applicationUser = new();

            try
            {
                using MySqlConnection sqlConnection = new(_connectionString);
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindByLogin";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("loginProvider", loginProvider);
                sqlCommand.Parameters.AddWithValue("providerKey", providerKey);

                await sqlConnection.OpenAsync(cancellationToken);

                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            if(applicationUser.Id != 0)
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
