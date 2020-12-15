using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Exceptions;
using Epsic.Rpg.Models;
using Epsic.Rpg.Repositories;

namespace Epsic.Rpg.Services
{

    public class TeamsService : ITeamsService
    {
        private readonly ITeamsRepository _teamsRepository;
        public TeamsService(ITeamsRepository teamsRepository)
        {
            _teamsRepository = teamsRepository;
        }

        public async Task<TeamDetailViewModel> GetSingle(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Id cannot be lower than 1.");

            var teamDb = await _teamsRepository.GetSingle(id);

            return teamDb;
        }

        public async Task<Team> UpdateAsync(int id, UpdateTeamDto teamToUpdate)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Id cannot be lower than 1.");

            if (teamToUpdate == null)
                throw new ArgumentNullException(nameof(teamToUpdate));

            if (teamToUpdate.Name.Length > 32)
                throw new ArgumentOutOfRangeException(nameof(teamToUpdate.Name), teamToUpdate.Name, "Team name length cannot be greater than 32.");

            if (!await _teamsRepository.ExistsById(id))
                throw new DataNotFoundException($"Team Id:{id} doesn't exists.");

            if (await _teamsRepository.ExistsByName(teamToUpdate.Name))
                throw new ArgumentException(nameof(teamToUpdate.Name), $"Team {teamToUpdate.Name} already exists.");

            return await _teamsRepository.UpdateAsync(id, teamToUpdate);
        }

        public async Task<Team> CreateAsync(CreateTeamDto teamToCreate)
        {
            if (teamToCreate == null)
                throw new ArgumentNullException(nameof(teamToCreate));

            if (teamToCreate.Name.Length > 32)
                throw new ArgumentOutOfRangeException(nameof(teamToCreate.Name), teamToCreate.Name, "Team name length cannot be greater than 32.");

            if (await _teamsRepository.ExistsByName(teamToCreate.Name))
                throw new ArgumentException(nameof(teamToCreate.Name), $"Team {teamToCreate.Name} already exists.");

            var modelDb = await _teamsRepository.CreateAsync(teamToCreate);

            return modelDb;
        }

        public Task<List<TeamSummaryViewModel>> GetAll(string filterByName)
        {
            if (filterByName?.Length < 4)
                throw new ArgumentOutOfRangeException("Team name length must be greater than 3.");

            return _teamsRepository.Search(filterByName);
        }

        public async Task<bool> Delete(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Id cannot be lower than 1.");

            var result = await _teamsRepository.Delete(id);

            //If result == 1, one entity has been deleted from the database
            if (result == 1)
                return true;
            else
                return false;
        }

        public async Task<bool> AddCharacterToTeam(AddCharacterToTeamDto addCharacterToTeam)
        {
            if (addCharacterToTeam == null)
                throw new ArgumentNullException(nameof(addCharacterToTeam));

            if (addCharacterToTeam.TeamId < 1)
                throw new ArgumentOutOfRangeException(nameof(addCharacterToTeam.TeamId), addCharacterToTeam.TeamId, "Team Id cannot be lower than 1.");

            if (addCharacterToTeam.CharacterId < 1)
                throw new ArgumentOutOfRangeException(nameof(addCharacterToTeam.CharacterId), addCharacterToTeam.CharacterId, "Character Id cannot be lower than 1.");

            if (!await _teamsRepository.ExistsById(addCharacterToTeam.TeamId))
                throw new DataNotFoundException($"Team Id:{addCharacterToTeam.TeamId} doesn't exists.");

            // Ici, si on avait un CharacterRepository on devrait checker si le CharacterId existe dans la db
            // if (!_characterRepository.ExistsById(addCharacterToTeam.CharacterId))
            //     throw new DataNotFoundException($"CharacterId:{addCharacterToTeam.CharacterId} doesn't exists.");

            var result = await _teamsRepository.AddCharacterToTeam(addCharacterToTeam.TeamId, addCharacterToTeam.CharacterId);

            if (result == 1)
                return true;
            else
                return false;
        }

        public async Task<bool> RemoveCharacterFromTeam(RemoveCharacterFromTeamDto removeCharacterFromTeam)
        {
            if (removeCharacterFromTeam == null)
                throw new ArgumentNullException(nameof(removeCharacterFromTeam));

            if (removeCharacterFromTeam.TeamId < 1)
                throw new ArgumentOutOfRangeException(nameof(removeCharacterFromTeam.TeamId), removeCharacterFromTeam.TeamId, "Team Id cannot be lower than 1.");

            if (removeCharacterFromTeam.CharacterId < 1)
                throw new ArgumentOutOfRangeException(nameof(removeCharacterFromTeam.CharacterId), removeCharacterFromTeam.CharacterId, "Character Id cannot be lower than 1.");

            if (!await _teamsRepository.ExistsById(removeCharacterFromTeam.TeamId))
                throw new DataNotFoundException($"Team Id:{removeCharacterFromTeam.TeamId} doesn't exists.");

            // Ici, si on avait un CharacterRepository on devrait checker si le CharacterId existe dans la db
            // if (!_characterRepository.ExistsById(addCharacterToTeam.CharacterId))
            //     throw new DataNotFoundException($"CharacterId:{addCharacterToTeam.CharacterId} doesn't exists.");

            var result = await _teamsRepository.RemoveCharacterFromTeam(removeCharacterFromTeam.TeamId, removeCharacterFromTeam.CharacterId);

            if (result == 1)
                return true;
            else
                return false;
        }
    }
}