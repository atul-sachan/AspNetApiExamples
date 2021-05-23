using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Interfaces;
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
    [Authorize()]
    public class UsersController : BaseApiController
    {
        public IUserRepository userRepository { get; }
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userRepository.GetUserAsync();
            return Ok(mapper.Map<IEnumerable<MemberDto>>(users));
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetUser(int id)
        // {
        //     var user = await userRepository.GetUserByIdAsync(id);
        //     return Ok(mapper.Map<MemberDto>(user));
        // }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUseByUserNamer(string username)
        {
            var user = await userRepository.GetMemberAsync(username);
            return Ok(user);
        }


    }
}
