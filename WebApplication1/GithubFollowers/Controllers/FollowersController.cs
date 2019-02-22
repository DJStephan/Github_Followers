using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ServiceLayer;

namespace WebApplication1.Controllers
{
    [Route("github/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private IFollowerService _followerService;
        
        public FollowersController(IFollowerService followerService)
        {
            _followerService = followerService;
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowers([FromRoute] string username)
        {
            return await _followerService.GetFollowers(username);
        }
    }
}