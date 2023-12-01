using ForumApp.Core.Context;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ForumApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        public ForumContext context { get; set; }

        private ITopicService topicService { get; set; }

        public TopicController(ForumContext context,ITopicService topicService)
        {
            this.context = context;
            this.topicService = topicService;
        }

        [Route("GetAllPostForTopic/{name}/{userId}")]
        [EnableCors("CORS")]
        [HttpGet, Authorize(Roles = "K")]
        public async Task<ActionResult> GetAllPostForTopic(string name, int userId)
        {
            try
            {
                var posts = await topicService.GetPostForTopicStringAsync(name, userId); 
                return Ok(posts);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetAllTopic")]
        [EnableCors("CORS")]
        [HttpGet, Authorize(Roles = "K")]
        public async Task<ActionResult> GetAllTopic()
        {
            try
            {
                var topics = await topicService.GetAllTopicsAsync();
                return Ok(topics);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("CreateTopic/{Name}")]
        [EnableCors("CORS")]
        [HttpPost, Authorize(Roles = "K")]
        public async Task<ActionResult> CreateTopic(string name)
        {
            try
            {
                await topicService.CreateTopicAsync(name);
                return Ok("Success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
