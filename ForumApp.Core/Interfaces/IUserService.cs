using ForumApp.Core.Dtos;
using ForumApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<CommunityModel>> GetCommunitiesForUserAsync(int userId);

        Task<string> GetUserAccountAsync(string username, string password);

        Task<bool> CreateUserAccountAsync(string username, string mail, string password, string confPassword);
    }
}
