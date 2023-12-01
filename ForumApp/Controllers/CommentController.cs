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
    public class CommentController : ControllerBase
    {
        public ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [EnableCors("CORS")]
        [Route("GetAllComments/{postId}")]
        [HttpGet, Authorize(Roles = "K")]

        public ActionResult GetAllComents(int postId)
        {
            try
            {

                var comments = commentService.GetCommentsForPost(postId);

                return Ok(comments);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("AddComment/{userId}/{postId}/{textComment}")]
        [HttpPost, Authorize(Roles = "K")]

        public async Task<ActionResult> AddComment(int userId, int postId, string textComment)
        {
            try
            {

                await commentService.AddCommentAsync(userId, postId, textComment);

                return Ok("Your comment is add.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [EnableCors("CORS")]
        [Route("DeleteComment/{userId}/{commentId}")]
        [HttpDelete, Authorize(Roles = "K")]
        public async Task<ActionResult> RemoveComment(int userId, int commentId)
        {
            try
            {
                await commentService.RemoveCommentAsync(userId, commentId);

                return Ok("Your comment is deleted.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
