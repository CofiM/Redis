using ForumApp.Core.Models;
using ForumApp.Core.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumApp.Core.Interfaces;

namespace ForumApp.Controllers
{
    
    public class LikePostController : ControllerBase
    {
        private IPostLikeService postLikeService { get; set; }

        public LikePostController(IPostLikeService postLikeService)
        {
            this.postLikeService = postLikeService;
        }

        [Route("AddLikePost/{idUser}/{idPost}")]
        [HttpPost, Authorize(Roles = "K")]
        public async Task<ActionResult> AddLikePost(int idUser, int idPost)
        {
            try 
            {
                return Ok(await postLikeService.LikePostAsync(idUser, idPost));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DeleteLikePost/{idUser}/{idPost}")]
        [HttpDelete, Authorize(Roles = "K")]
        public async Task<ActionResult> DeleteLikePost(int idUser, int idPost)
        {
            try
            {
                return Ok(await postLikeService.RemoveLikeAsync(idUser, idPost));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
