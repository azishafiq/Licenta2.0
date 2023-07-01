using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AddsController: BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddsRepository _addsRepository;
        public AddsController(IUserRepository userRepository, IAddsRepository addsRepository)
        {
            _addsRepository = addsRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddAdd(string username)
        {
            var sourceUserId = User.GetUserId();
            var addedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _addsRepository.GetUserWithAdds(sourceUserId);

           if(addedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You cannot add yourself");

            var userAdd = await _addsRepository.GetUserAdd(sourceUserId, addedUser.Id);

            if (userAdd != null) return BadRequest("You already added this user");

            userAdd = new UserAdd
            {
                SourceUserId = sourceUserId,
                TargetUserId = addedUser.Id
            };

            sourceUser.AddedUsers.Add(userAdd);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to add user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<AddDto>>> GetUserAdds([FromQuery]AddsParams addsParams)
        {
            addsParams.UserId = User.GetUserId();

            var users = await _addsRepository.GetUserAdds(addsParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, 
                users.PageSize, users.TotalCount, users.TotalPages));
            
            return Ok(users);
        }
    }
}