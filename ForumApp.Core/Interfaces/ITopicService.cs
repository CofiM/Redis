using ForumApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAllTopicsAsync();

        Task<List<Post>> GetPostsForTopicAsync(int topicId);

        Task<List<object>> GetPostForTopicStringAsync(string name, int userId);

        Task CreateTopicAsync(string name);
    }
}
