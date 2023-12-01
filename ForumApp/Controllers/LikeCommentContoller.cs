using ForumApp.Core.Models;
using ForumApp.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ForumApp.Core.Interfaces;

namespace ForumApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LikeCommentController : ControllerBase
    {
        public ForumContext Context { get; set; }

        public ICommentLikeOrDislikeService commentLikeService { get; set; }

        public LikeCommentController(ForumContext context, ICommentLikeOrDislikeService commentLikeService)
        {
            this.Context = context;
            this.commentLikeService = commentLikeService;
        }

        //[Route("GetAllCommentForPost/{postId}")]
        //[HttpGet, Authorize(Roles = "K")]

        //public async Task<ActionResult> GetAllCommentForPost(int postId)
        //{
        //    try
        //    {
        //        var comments = await Context.Comments
        //            .Where(p => p.Post.ID == IDPost)
        //            .ToListAsync();
        //        return Ok(comments);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [EnableCors("CORS")]
        [Route("AddLikeComment/{userId}/{commentId}")]
        [HttpPost, Authorize(Roles = "K")]

        public async Task<ActionResult> AddLikeComment(int userId, int commentId)
        {
            try
            {
                await commentLikeService.LikeCommentAsync(userId, commentId);

                return Ok("Like is added.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("RemoveLikeComment/{userId}/{commentId}")]
        [HttpDelete, Authorize(Roles = "K")]

        public async Task<ActionResult> RemoveLikeComment(int userId, int commentId)
        {
            try
            {

                await commentLikeService.RemoveLikeCommentAsync(userId, commentId);

                return Ok("Comment isn't liked.");
                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("LikeIsAlreadyExist/{userId}/{commentId}")]
        [HttpGet, Authorize(Roles = "K")]
        public async Task<ActionResult> LikeIsAlreadyExist(int userId, int commentId)
        {
            try
            {

                bool check = await commentLikeService.IsCommentAlreadyLikeAsync(userId, commentId);
                
                return Ok(check);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
