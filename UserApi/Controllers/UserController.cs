using JwtAuthenticationManager.Model;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly JwtTokenHandler jwtTokenHandler;
        public UserController(JwtTokenHandler jwtTokenHandler)
        {
            this.jwtTokenHandler = jwtTokenHandler;
        }
        [HttpGet(Name = "GetUser")]
        public ActionResult<List<UserAccount>> Get()
        {
            return jwtTokenHandler.GetUserAccount();
        }
        [HttpPost(Name = "PostUser")]
        public ActionResult Post(UserAccount userAccount)
        {
            jwtTokenHandler.PostUserAccount(userAccount);
            return StatusCode(201);
        }
    }
}