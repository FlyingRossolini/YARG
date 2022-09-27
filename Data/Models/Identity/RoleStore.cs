using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Threading;
using System.Threading.Tasks;
using YARG.Models;

namespace YARG.Data
{
    public class RoleStore : IRoleStore<ApplicationRole>
    {
        private readonly string _connectionString;

        public RoleStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("IdentityConnection");
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);
            try
            {
                using MySqlCommand mySqlCommand = new();
                mySqlCommand.Connection = sqlConnection;
                mySqlCommand.CommandText = "spAddRole";
                mySqlCommand.Parameters.AddWithValue("thisName", role.Name);
                mySqlCommand.Parameters.AddWithValue("thisNormalizedName", role.NormalizedName);
                mySqlCommand.Parameters.Add("LID", MySqlDbType.Int32);
                mySqlCommand.Parameters["LID"].Direction = System.Data.ParameterDirection.Output;

                await sqlConnection.OpenAsync(cancellationToken);
                await mySqlCommand.ExecuteNonQueryAsync(cancellationToken);

                role.Id = (int)mySqlCommand.Parameters["LID"].Value;

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

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spUpdateRole";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", role.Id);
                sqlCommand.Parameters.AddWithValue("thisName", role.Name);
                sqlCommand.Parameters.AddWithValue("thisNormalizedName", role.NormalizedName);

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

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            cancellationToken.ThrowIfCancellationRequested();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spDeleteRole";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", role.Id);

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

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationRole applicationRole = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindRoleById";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisid", roleId);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationRole.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationRole.Name = sqlDataReader["Name"].ToString();
                    applicationRole.NormalizedName = sqlDataReader["NormalizedName"].ToString();
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

            return applicationRole;

        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ApplicationRole applicationRole = new();
            using MySqlConnection sqlConnection = new(_connectionString);

            try
            {
                using MySqlCommand sqlCommand = new();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "spFindRoleByName";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("thisname", normalizedRoleName);

                await sqlConnection.OpenAsync(cancellationToken);
                await using MySqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    applicationRole.Id = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id"));
                    applicationRole.Name = sqlDataReader["Name"].ToString();
                    applicationRole.NormalizedName = sqlDataReader["NormalizedName"].ToString();
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

            return applicationRole;

        }

        public void Dispose()
        {
            // Nothing to dispose.
        }
    }
}