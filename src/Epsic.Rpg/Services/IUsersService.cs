using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Models;

namespace Epsic.Rpg.Services
{
    public interface IUsersService
    {
        Task<bool> AddCharacterToUser(AddCharacterToUserDto addCharacterToUser);
        Task<User> CreateAsync(CreateUserDto userToCreate);
        Task<bool> Delete(int id);
        Task<List<UserSummaryViewModel>> GetAll(string filterByName);
        Task<UserDetailViewModel> GetSingle(int id);
        Task<bool> RemoveCharacterFromUser(RemoveCharacterFromUserDto removeCharacterFromUser);
        Task<User> UpdateAsync(int id, UpdateUserDto userToUpdate);
    }
}