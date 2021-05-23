using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Interfaces
{
    public interface IUserRepository
    {
        void update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUserAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string username);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<MemberDto> GetMemberAsync(string username);
    }
}