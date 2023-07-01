using System.Net.Http.Headers;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public DataContext _context { get; }
        public IMapper _mapper { get; }
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(p => p.Photos)
            .Include(p => p.Projects)
            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var querry = _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            querry = querry.Where(u => u.UserName != userParams.CurrentUsername);
            querry = querry.Where(u => u.Gender != userParams.Gender);
            
            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            querry = querry.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            querry = userParams.OrderBy switch
            {
                "created" => querry.OrderByDescending(u => u.Created),
                   _ => querry.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(querry, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }
    }
}