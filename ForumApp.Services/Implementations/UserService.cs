using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Cache.Interfaces;
using ForumApp.Core.Context;
using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Interfaces.CacheServices;
using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private ForumContext context { get; set; }

        private readonly IConfiguration _configuration;

        private ICacheProvider cacheProvider;

        private IUserInfoCacheService userInfoCacheService {get; set; }

        private IDistributedCache cache { get; set; }

        private IFollowedCommunityServices followedCommunityServices { get; set; }

        private ICommunityService communityService { get; set; }

        public UserService(ForumContext context, IConfiguration _configuration, IDistributedCache cache, ICacheProvider cacheProvider, IUserInfoCacheService userInfoCacheService, IFollowedCommunityServices followedCommunityServices, ICommunityService communityService)
        {
            this.context = context;
            this._configuration = _configuration;
            this.cache = cache;
            this.cacheProvider = cacheProvider;
            this.userInfoCacheService = userInfoCacheService;
            this.followedCommunityServices = followedCommunityServices;
            this.communityService = communityService;
        }

        public async Task<List<CommunityModel>> GetCommunitiesForUserAsync(int userId)
        {
            if (!await IsUserExistAsync(userId))
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var followedCommunities = await followedCommunityServices.GetCommunitiesFollowedByUserAsync(userId);
            var communities = await communityService.GetAllCommunities();
            var result = new List<CommunityModel>();

            foreach (var followedCommunityId in followedCommunities.CommunityIds)
            {
                var cacheItem = new CommunityModel
                {
                    Id = followedCommunityId,
                    Name = communities.FirstOrDefault(p => p.Id == followedCommunityId)?.Name
                };
                result.Add(cacheItem);
            }

            return result;
        }

        public async Task<string> GetUserAccountAsync(string username, string password)
        {
            string[] arguments = { username, password };

            if (!checkArgument(arguments))
            {
                throw new InvalidOperationException("Invalid input.");
            }

            var userRedis = await cacheProvider.GetAsync<UserCacheItem>(username);

            if (userRedis != null)
            {
                return userRedis.Token;
            }

            var user = await context.Users
                .Where(p => p.Username == username)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new InvalidOperationException("Inncorect password");
                }

                var obj = new UserCacheItem
                {
                    Token = CreateToken(user)
                };

                await cacheProvider.SetAsync(username, obj, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });

                return CreateToken(user);
            }
            else
            {
                throw new InvalidOperationException("User does not exist.");
            }
        }

        public async Task<bool> CreateUserAccountAsync(string username, string mail, string password, string confPassword)
        {
            string[] arguments = { username, mail, password, confPassword };

            if (!checkArgument(arguments))
            {
                throw new InvalidOperationException("Invalid input.");
            }

            var user = await context.Users
                .Where(p =>
                    p.Username == username &&
                    p.Mail == mail
                ).FirstOrDefaultAsync();

            if (user != null)
            {
                throw new InvalidOperationException("User already exist!");
            }

            if (password == confPassword)
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                User u = new User();
                u.PasswordHash = passwordHash;
                u.PasswordSalt = passwordSalt;
                u.Username = username;
                u.Mail = mail;

                context.Users.Add(u);
                await context.SaveChangesAsync();

                return true;
            }

            return false;

        }

        private async Task<bool> IsUserExistAsync(int userId)
        {
            var user = userInfoCacheService.GetUser(userId);
            if (user != null)
            {
                return true;
            }

            return await context.Users.FirstOrDefaultAsync(p => p.ID == userId) != null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmc = new HMACSHA512())
            {
                passwordSalt = hmc.Key;
                passwordHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User u)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, u.Username),
                new Claim(ClaimTypes.Role, "K"),
                new Claim(ClaimTypes.SerialNumber, u.ID.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool checkArgument(string[] arguments)
        {
            foreach(string item in arguments)
            {
                if (string.IsNullOrWhiteSpace(item) || item.Length > 50)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
