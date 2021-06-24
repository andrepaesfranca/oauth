using Microsoft.Extensions.Logging;
using oauth_service.Helpers;
using oauth_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace oauth_service.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly AuthDbContext Ctx;
        protected readonly ILogger Logger;
        protected readonly ICrypto Crypto;

        public UserRepository(AuthDbContext ctx, ILoggerFactory loggerFactory, ICrypto crypto)
        {
            Ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            Logger = loggerFactory.CreateLogger(GetType());
            Crypto = crypto ?? throw new ArgumentNullException(nameof(Crypto));
        }

        public void Create(User user)
        {
            try
            {
                user.Password = Crypto.EncryptPassword(user.Password);

                Ctx.Users.Add(user);
                Ctx.SaveChanges();

                Logger.LogInformation($"Salvando usuário -> {user.UserId}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return Ctx.Users
                    .Where(x => x.IsEnabled)
                    .Select(x => new User
                    {
                        Email = x.Email,
                        IsEnabled = x.IsEnabled,
                        Name = x.Name,
                        Type = x.Type,
                        UserId = x.UserId
                    });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public User GetById(int id)
        {
            try
            {
                var user = Ctx.Users.FirstOrDefault(x => x.UserId == id && x.IsEnabled);

                return user;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public User GetByEmail(string email, string password)
        {
            try
            {
                var userExists = Ctx.Users.FirstOrDefault(x => x.Email == email && Crypto.VerifyPassword(password) && x.IsEnabled);

                if (userExists == null)
                    throw new Exception("Email e/ou senha está(ão) inválido(s).");

                return userExists;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public void Update(User user)
        {
            try
            {
                user.Password = Crypto.EncryptPassword(user.Password);

                Ctx.Users.Update(user);
                Ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }
    }
}