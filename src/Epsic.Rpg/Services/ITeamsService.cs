using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Services
{
    public interface ITeamsService
    {
        Task<bool> AddCharacterToTeam(AddCharacterToTeamDto addCharacterToTeam);
        Task<Team> CreateAsync(CreateTeamDto teamToCreate);
        Task<bool> Delete(int id);
        Task<List<TeamSummaryViewModel>> GetAll(string filterByName);
        Task<TeamDetailViewModel> GetSingle(int id);
        Task<bool> RemoveCharacterFromTeam(RemoveCharacterFromTeamDto removeCharacterFromTeam);
        Task<Team> UpdateAsync(int id, UpdateTeamDto teamToUpdate);
    }
}