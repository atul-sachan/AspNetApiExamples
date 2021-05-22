using DatingApp.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class UsersController : BaseApiController
    {
        private readonly DataContext context;

        public UsersController(DataContext context)
        {
            this.context = context;
        }

        [AllowAnonymous()]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this.context.Users.ToListAsync();
            return Ok(users);
        }

        [Authorize()]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this.context.Users.FindAsync(id);
            return Ok(user);
        }


    }
}
