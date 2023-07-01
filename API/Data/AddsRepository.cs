using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AddsRepository : IAddsRepository
    {
        private readonly DataContext _context;
        public AddsRepository(DataContext context)
        {
            _context = context;
            
        }

        public async Task<UserAdd> GetUserAdd(int sourceUserId, int targetUserId)
        {
            return await _context.Adds.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<AddDto>> GetUserAdds(AddsParams addsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var adds = _context.Adds.AsQueryable();

            if (addsParams.Predicate == "added")
            {
                adds = adds.Where(add => add.SourceUserId == addsParams.UserId);
                users = adds.Select(add => add.TargetUser);
            }

            if (addsParams.Predicate == "addedBy")
            {
                adds = adds.Where(add => add.TargetUserId == addsParams.UserId);
                users = adds.Select(add => add.SourceUser);
            }

            var addedUser = users.Select(user => new AddDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<AddDto>.CreateAsync(addedUser, addsParams.PageNumber, addsParams.PageSize);

         
        }

        public async Task<AppUser> GetUserWithAdds(int userId)
        {
            return await _context.Users
                .Include(x => x.AddedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}