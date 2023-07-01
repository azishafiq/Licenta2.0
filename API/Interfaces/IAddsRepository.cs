using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IAddsRepository
    {
        Task<UserAdd> GetUserAdd(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithAdds(int userId);
        Task<PagedList<AddDto>> GetUserAdds(AddsParams addsParams);
    }
}