using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Extensions;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IPhotoService photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.photoService = photoService;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var users = await userRepository.GetUserAsync();
        //    return Ok(mapper.Map<IEnumerable<MemberDto>>(users));
        //}

        [Authorize(Roles = "Member")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
            userParams.CurrentUsername = user.UserName;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male"? "female": "male";
            }

            var users = await userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetUser(int id)
        // {
        //     var user = await userRepository.GetUserByIdAsync(id);
        //     return Ok(mapper.Map<MemberDto>(user));
        // }

        [Authorize(Roles ="Member")]
        [HttpGet("{username}", Name = "GetUserNyName")]
        public async Task<IActionResult> GetUseByUserNamer(string username)
        {
            var user = await userRepository.GetMemberAsync(username);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsers(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUserName();
            var user = await userRepository.GetUserByUserNameAsync(username);

            mapper.Map(memberUpdateDto, user);
            userRepository.update(user);
            if (await userRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("failed to update user");

        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var username = User.GetUserName();
            var user = await userRepository.GetUserByUserNameAsync(username);
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null)
                return BadRequest(result.Error.Message);

            var photo = new Entities.Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUserNyName", new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Problem Adding Photo");
        }


        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain)
            {
                return BadRequest("This is already your main photo");
            }

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if (await userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x=> x.Id == photoId);
            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null){
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null){
                    return BadRequest(result.Error.Message);
                }
            }
            
            user.Photos.Remove(photo);

            if(await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Photo delete unsuccessful");

        }



    }
}
