using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Data;
using Epsic.Rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace Epsic.Rpg.Repositories
{

    public class UsersRepository : IUsersRepository
    {
        private readonly EpsicRpgDataContext _context;
        public UsersRepository(EpsicRpgDataContext context)
        {
            _context = context;
        }

        public Task<UserDetailViewModel> GetSingle(int id)
        {
            return _context.Users.Include(t => t.Characters)
                                 .Select(t => new UserDetailViewModel
                                 {
                                     Id = t.Id,
                                     Username = t.Username,
                                     Characters = t.Characters.Select(c => new CharacterSummaryViewModel
                                     {
                                         Id = c.Id,
                                         Name = c.Name
                                     })
                                 }).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<User> UpdateAsync(int id, UpdateUserDto userToUpdate)
        {
            var user = await _context.Users.FirstAsync(c => c.Id == id);

            user.Username = userToUpdate.Username;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> CreateAsync(CreateUserDto userToCreate)
        {
            var userDb = new User();
            userDb.Username = userToCreate.Username;

            _context.Users.Add(userDb);
            await _context.SaveChangesAsync();

            return userDb;
        }

        public Task<List<UserSummaryViewModel>> Search(string name)
        {
            return _context.Users.Where(c => string.IsNullOrWhiteSpace(name) || c.Username.Contains(name)).Select(t => 
            new UserSummaryViewModel
            {
                Id = t.Id,
                Username = t.Username
            }).ToListAsync();
        }

        public async Task<int> Delete(int id)
        {
            _context.Users.Remove(await _context.Users.FirstOrDefaultAsync(c => c.Id == id));
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddCharacterToUser(int userId, int characterId)
        {
            var userDb = await _context.Users.Include(t => t.Characters).FirstOrDefaultAsync(c => c.Id == userId);

            userDb.Characters.Add(_context.Characters.Find(characterId));

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveCharacterFromUser(int userId, int characterId)
        {
            var userDb = await _context.Users.Include(t => t.Characters).FirstOrDefaultAsync(c => c.Id == userId);

            userDb.Characters.Remove(userDb.Characters.First(c => c.Id == characterId));

            return await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsById(int id)
        {
            return _context.Users.AnyAsync(c => c.Id == id);
        }

        public Task<bool> ExistsByName(string name)
        {
            return _context.Users.AnyAsync(c => c.Username == name);
        }
    }
}