using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using YARG.Data;
using YARG.Models;

namespace YARG.SeedData
{
    public class SeedData
    {
        private readonly IConfiguration _config;

        public SeedData(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async void SeedAdminUser()
        {
            var user = new ApplicationUser
            {
                UserName = _config["DefaultAdmin:UserName"],
                NormalizedUserName = _config["DefaultAdmin:UserName"].ToUpper(),
                Email = _config["DefaultAdmin:UserName"],
                NormalizedEmail = _config["DefaultAdmin:UserName"].ToUpper(),
                EmailConfirmed = true
            };
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, _config["DefaultAdmin:Password"]);
            user.PasswordHash = hashed;            

            CancellationToken cancellationToken = CancellationToken.None;

            var roleStore = new RoleStore(_config);
            if(await roleStore.FindByNameAsync("ADMIN",cancellationToken) == null)
            {
                await roleStore.CreateAsync(new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN" }, cancellationToken);
            }

            var userStore = new UserStore(_config);

            var lookedUpUser = await userStore.FindByEmailAsync(_config["DefaultAdmin:UserName"].ToUpper(),cancellationToken);

            if (lookedUpUser.Id == 0)
            {
                await userStore.CreateAsync(user, cancellationToken);
            }

            lookedUpUser = await userStore.FindByEmailAsync(_config["DefaultAdmin:UserName"].ToUpper(), cancellationToken);
            await userStore.AddToRoleAsync(lookedUpUser, "Admin", cancellationToken);



        }
    }
}