using System.Net.Mime;
using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Controllers
{

    //[AllowAnonymous]
    //[Authorize]
    public class UsersController : BaseApiController
    {
        
        private readonly IUserRepository _userRepository;
        public IMapper _mapper { get; }
        public IPhotoService _photoService;
        
        private readonly IProjectService _projectService;

        public UsersController(IUserRepository userRepository, IMapper mapper,
         IPhotoService photoService, IProjectService projectService)
        {
            _projectService = projectService;
            
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
           
        }

        
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages));

            return Ok(users);
            
        }

        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           return await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user  = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync()) 
            {
                return CreatedAtAction(nameof(GetUser),
                    new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");
        }

         [HttpPost("add-project")]
        public async Task<ActionResult<ProjectDto>> AddProject(IFormFile file)
        {
            var user  = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _projectService.AddProjectAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var project = new Project
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                FileName= file.FileName

            };

            

            user.Projects.Add(project);

            if (await _userRepository.SaveAllAsync()) 
            {
                return CreatedAtAction(nameof(GetUser),
                    new {username = user.UserName}, _mapper.Map<ProjectDto>(project));
            }

            return BadRequest("Problem adding project");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");

        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Porblem deleting photo");
        }

        [HttpDelete("delete-project/{projectId}")]
        public async Task<ActionResult> DeleteProject(int projectId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var project = user.Projects.FirstOrDefault(x => x.Id == projectId);

            if (project == null) return NotFound();

            if (project.PublicId != null)
            {
                var result = await _projectService.DeleteProjectAsync(project.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Projects.Remove(project);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Porblem deleting project");
        }

       

        
        
    }
}