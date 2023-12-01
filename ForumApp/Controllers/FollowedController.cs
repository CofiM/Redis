using ForumApp.Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForumApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowedController : ControllerBase
    {
        private IFollowedCommunityServices followedCommunityService { get; set; }

        public FollowedController(IFollowedCommunityServices follow)
        {
            followedCommunityService = follow;
        }


        [Route("CommunityFollowByUser/{userId}")]
        [EnableCors("CORS")]
        [HttpGet]
        public async Task<ActionResult> CommunityFollowByUser(int userId)
        {
            try
            {
                var communities = await followedCommunityService.GetCommunitiesFollowedByUserAsync(userId);
                return Ok(communities);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
