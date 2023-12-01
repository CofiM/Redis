using ForumApp.Core.Context;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ForumApp.Controllers
{    
    public class CommunityController : ControllerBase
    {
        private ICommunityService communityService { get; set; } 

        private IDistributedCache cache { get; set; }

        public CommunityController(ICommunityService communityService, IDistributedCache cache)
        {
            this.communityService = communityService;
            this.cache = cache;
        }

        [Route("GetAllComunities")]
        [EnableCors("CORS")]
        [HttpGet]
        public async Task<ActionResult> GetAllCommunities()
        {
            try
            {
                var communities = await communityService.GetAllCommunities();
                return Ok(communities);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("CreateCommunity/{name}")]
        [EnableCors("CORS")]
        [HttpPost]
        public async Task<ActionResult> CreateCommunity(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid name for community!");
            }
            try
            {
                await communityService.AddCommunityAsync(name);
                return Ok($"New community is add {name}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [Route("FollowCommunity/{userId}/{communityId}")]
        [EnableCors("CORS")]
        [HttpPut]
        public async Task<ActionResult> FollowCommunity(int userId, int communityId)
        {
            try
            {
                await communityService.FollowCommunityAsync(userId, communityId);
                return Ok("User follow community.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("UnfollowCommunity/{userId}/{communityId}")]
        [EnableCors("CORS")]
        [HttpDelete]
        public async Task<ActionResult> UnfollowCommunity(int userId, int communityId)
        {
            try
            {
                await communityService.UnfollowCommunityAsync(userId, communityId);
                return Ok("User unfollow community!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        


        [Route("GetAllPost/{communityId}")]
        [EnableCors("CORS")]
        [HttpGet]
        public async Task<ActionResult> GetAllPost(int communityId)
        {
            try
            {
                var posts = await communityService.GetAllPostForCommunity(communityId);
                return Ok(posts);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetAllAccount/{communityId}")]
        [EnableCors("CORS")]
        [HttpGet]
        public async Task<ActionResult> GetAllAccount(int communityId)
        {
            try
            {
                var connections = await communityService.GetAllAccountWhichFollowCommunity(communityId);
                return Ok(connections.Select(p => new
                {
                    p.Community.Name,
                    p.User.Username,
                    p.User.Mail
                }));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    
    }
}
