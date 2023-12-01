using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ForumApp.Core.Context;
using ForumApp.Core.Models;
using ForumApp.Core.Interfaces;

namespace ForumApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private ForumContext context { get; set; }

        private IPostService postService { get; set; }

        private ICommentService commentService { get; set; }

        public PostController(ForumContext con,IPostService postService, ICommentService commentService)
        {
            this.context = con;
            this.postService = postService;
            this.commentService = commentService;
        }

        [HttpPost, Authorize(Roles = "K")]
        [Route("AddPost/{title}/{text}/{topic}/{userId}/{communityId}")]
        public async Task<ActionResult> AddPost(string title, string text,string topic,int userId,int communityId)
        {
            try
            {
                await postService.AddPost(title, text, topic, userId, communityId);
                return Ok("Success");
            }
            catch (SystemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete, Authorize(Roles = "K")]
        [Route("RemovePost/{idPost}")]
        public async Task<ActionResult> RemovePost(int idPost)
        {
            try
            {
                await postService.RemovePost(idPost);
                return Ok("Deleted!");
            }
            catch(SystemException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet, Authorize(Roles = "K")]
        [Route("GetCommentsForPost/{idPost}")]
        public ActionResult GetCommentsForPost(int idPost)
        {
            try
            {
                var comments =  commentService.GetCommentsForPost(idPost);
                return Ok(comments);
            }
            catch(SystemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Authorize(Roles = "K")]
        [Route("GetPostsForCommunity/{communityId}/{userId}")]
        public async Task<ActionResult> GetPostsForCommunity(int communityId, int userId)
        {
            try
            {
                var posts = await postService.GetPostsForCommunity(communityId, userId);
                return Ok(posts);
            }
            catch (SystemException e)
            {
                return BadRequest(e.Message);
            }
        }      

        [HttpGet, Authorize(Roles = "K")]
        [Route("GetPostsForUser/{userId}")]
        public async Task<ActionResult> GetPostsForUser(int userId)
        {
            try
            {
                var posts = await postService.GetPostsByCommuntyForUserAsync(userId);
                return Ok(posts);
            }
            catch (SystemException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet, Authorize(Roles = "K")]
        [Route("GetPostsForTopic/{topicId}")]
        public async Task<ActionResult> GetPostsForTopic(int topicId)
        {
            try
            {
                var posts = await postService.GetPostsForTopic(topicId);
                posts.Select(p => new
                {
                    p.ID,
                    p.Title,
                    p.Text,
                    p.Community.Name,
                    Likes = p.Likes.Count,
                    Dislikes = p.Dislikes.Count,
                    Comments = p.Comments.Select(q => new
                    {
                        q.ID,
                        q.Text,
                        LikesComment = q.LikeComments.Count,
                        DislikeComment = q.DislikeComments.Count
                    })

                });
                return Ok(posts);
            }
            catch (SystemException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
