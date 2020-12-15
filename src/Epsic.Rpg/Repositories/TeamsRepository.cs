using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Data;
using Epsic.Rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace Epsic.Rpg.Repositories
{

    public class TeamsRepository : ITeamsRepository
    {
        private readonly EpsicRpgDataContext _context;
        public TeamsRepository(EpsicRpgDataContext context)
        {
            _context = context;
        }

        public Task<TeamDetailViewModel> GetSingle(int id)
        {
            return _context.Teams.Include(t => t.Characters)
                                 .Select(t => new TeamDetailViewModel
                                 {
                                     Id = t.Id,
                                     Name = t.Name,
                                     Characters = t.Characters.Select(c => new CharacterSummaryViewModel
                                     {
                                         Id = c.Id,
                                         Name = c.Name
                                     })
                                 }).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Team> UpdateAsync(int id, UpdateTeamDto teamToUpdate)
        {
            var team = await _context.Teams.FirstAsync(c => c.Id == id);

            team.Name = teamToUpdate.Name;

            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<Team> CreateAsync(CreateTeamDto teamToCreate)
        {
            var teamDb = new Team();
            teamDb.Name = teamToCreate.Name;

            _context.Teams.Add(teamDb);
            await _context.SaveChangesAsync();

            return teamDb;
        }

        public Task<List<TeamSummaryViewModel>> Search(string name)
        {
            return _context.Teams.Where(c => string.IsNullOrWhiteSpace(name) || c.Name.Contains(name)).Select(t => 
            new TeamSummaryViewModel
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync();
        }

        public async Task<int> Delete(int id)
        {
            _context.Teams.Remove(await _context.Teams.FirstOrDefaultAsync(c => c.Id == id));
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddCharacterToTeam(int teamId, int characterId)
        {
            var teamDb = await _context.Teams.Include(t => t.Characters).FirstOrDefaultAsync(c => c.Id == teamId);

            teamDb.Characters.Add(_context.Characters.Find(characterId));

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveCharacterFromTeam(int teamId, int characterId)
        {
            var teamDb = await _context.Teams.Include(t => t.Characters).FirstOrDefaultAsync(c => c.Id == teamId);

            teamDb.Characters.Remove(teamDb.Characters.First(c => c.Id == characterId));

            return await _context.SaveChangesAsync();
        }

        public Task<bool> ExistsById(int id)
        {
            return _context.Teams.AnyAsync(c => c.Id == id);
        }

        public Task<bool> ExistsByName(string name)
        {
            return _context.Teams.AnyAsync(c => c.Name == name);
        }
    }
}