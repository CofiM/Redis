using ForumApp.Core.Models;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ForumApp.Core.Context;
using ForumApp.Core.Interfaces;

namespace ForumApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService { get; set; }

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("GetFollowCommunity/{userId}")]
        [EnableCors("CORS")]
        [HttpGet]
        public async Task<ActionResult> GetFollowCommunity(int userId)
        {
            try
            {
                var communities = await userService.GetCommunitiesForUserAsync(userId);
                return Ok(communities.Select(p => new { p.Id, p.Name }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetAccount/{username}/{password}")]
        [EnableCors("CORS")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> GetAccount(string username, string password)
        {
            try
            {
                return Ok(await userService.GetUserAccountAsync(username, password));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("CreateAccount/{username}/{mail}/{password}/{confPassword}")]
        [EnableCors("CORS")]
        [HttpPost]
        public async Task<ActionResult> CreateAccount(string username, string mail, string password, string confPassword)
        {
            try
            {
                if (await userService.CreateUserAccountAsync(username, mail, password, confPassword))
                {
                    return Ok("User account created.");
                }

                return BadRequest("User account didnt create.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
