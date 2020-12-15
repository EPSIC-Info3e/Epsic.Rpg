using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Repositories
{
    public interface ITeamsRepository
    {
        Task<int> AddCharacterToTeam(int teamId, int characterId);
        Task<Team> CreateAsync(CreateTeamDto teamToCreate);
        Task<int> Delete(int id);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByName(string name);
        Task<TeamDetailViewModel> GetSingle(int id);
        Task<int> RemoveCharacterFromTeam(int teamId, int characterId);
        Task<List<TeamSummaryViewModel>> Search(string name);
        Task<Team> UpdateAsync(int id, UpdateTeamDto teamToUpdate);
    }
}