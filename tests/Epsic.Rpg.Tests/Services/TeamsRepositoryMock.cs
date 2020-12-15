using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Models;
using Epsic.Rpg.Repositories;

namespace Epsic.Rpg.Tests.Services
{
    public class TeamsRepositoryMock : ITeamsRepository
    {
        public Task<int> AddCharacterToTeam(int teamId, int characterId)
        {
            return Task.FromResult(1);
        }

        public Task<Team> CreateAsync(CreateTeamDto teamToCreate)
        {
           return Task.FromResult(new Team { Name = teamToCreate.Name });
        }

        public Task<int> Delete(int id)
        {
            return Task.FromResult(1);
        }

        public Task<bool> ExistsById(int id)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ExistsByName(string name)
        {
            return Task.FromResult(true);
        }

        public Task<TeamDetailViewModel> GetSingle(int id)
        {
            return Task.FromResult(new TeamDetailViewModel 
            {
                Id = id,
                Name = "T1", 
                Characters = new List<CharacterSummaryViewModel> 
                { 
                    new CharacterSummaryViewModel { Name = "C1"}
                }
            });
        }

        public Task<int> RemoveCharacterFromTeam(int teamId, int characterId)
        {
            return Task.FromResult(1);
        }

        public Task<List<TeamSummaryViewModel>> Search(string name)
        {
            return Task.FromResult(new List<TeamSummaryViewModel>{ new TeamSummaryViewModel 
            { 
                Name = name
            }});
        }

        public Task<Team> UpdateAsync(int id, UpdateTeamDto teamToUpdate)
        {
            return Task.FromResult(new Team { Name = teamToUpdate.Name, Id = id });
        }
    }
}