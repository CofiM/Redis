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
    public class DislikeCommentContoller : ControllerBase
    {
        public ForumContext Context { get; set; }

        private ICommentLikeOrDislikeService commentDislikeService { get; set; } 

        public DislikeCommentContoller(ForumContext context, ICommentLikeOrDislikeService commentDislikeService)
        {
            Context = context;
            this.commentDislikeService = commentDislikeService;
        }

        [EnableCors("CORS")]
        [Route("AddDislikeComment/{userId}/{commentId}")]
        [HttpPost, Authorize(Roles = "K")]
        
        public async Task<ActionResult> AddDislikeComment(int userId, int commentId)
        {
            try
            {
                await commentDislikeService.DislikeCommentAsync(userId, commentId);
                return Ok("Uspesno dodat dislike na komentaru");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableCors("CORS")]
        [Route("RemoveDislikeComment/{userId}/{commentId}")]
        [HttpDelete, Authorize(Roles = "K")]

        public async Task<ActionResult> RemoveDislikeComment(int userId, int commentId)
        {
            try
            {
                await commentDislikeService.RemoveDislikeCommentAsync(userId, commentId);
                return Ok("Uspesno izbirisan dislike na komentaru");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
