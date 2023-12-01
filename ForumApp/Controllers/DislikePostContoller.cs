using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using ForumApp.Core.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApp.Controllers
{
    public class DislikePostContoller : ControllerBase
    { 

        private IPostLikeService postLikeService { get; set; }

        public DislikePostContoller( IPostLikeService postLikeService)
        {
            this.postLikeService = postLikeService;
        }

        [Route("AddDislikePost/{IDUser}/{IDPost}")]
        [HttpPost, Authorize(Roles = "K")]

        public async Task<ActionResult> AddDislikePost(int IDUser, int IDPost)
        {
            try
            {

                return Ok(await postLikeService.DislikePostAsync(IDUser, IDPost));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DeleteDislikePost/{IDUser}/{IDPost}")]
        [HttpDelete, Authorize(Roles = "K")]

        public async Task<ActionResult> DeleteDislikePost(int IDUser, int IDPost)
        {
            try
            {
                return Ok(await postLikeService.RemoveDislikeAsync(IDUser, IDPost));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
