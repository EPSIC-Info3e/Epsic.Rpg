using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Data;
using Epsic.Rpg.Models;
using Epsic.Rpg.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Epsic.Rpg.Tests.Services
{
    [TestClass, TestCategory("Ex10")]
    public class TeamsRepositoryTests
    {
        private readonly DbContextOptions<EpsicRpgDataContext> _options;

        public TeamsRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<EpsicRpgDataContext>().UseSqlite(CreateInMemoryDatabase()).Options;
        }

        // Called once before each test after the constructor
        [TestInitialize]
        public async Task TestInitializeAsync()
        {
            using (var context = new EpsicRpgDataContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                
                //Creating characters
                var character1 = new Character { Id = 1, Name = "Char1" };
                var character2 = new Character { Id = 2, Name = "Char2" };
                var character3 = new Character { Id = 3, Name = "Char3" };
                var character4 = new Character { Id = 4, Name = "Char4" };

                //Creating teams
                var team1 = new Team { Id = 1, Name = "Team1" };
                var team2 = new Team { Id = 2, Name = "Team2" };
                var team3 = new Team { Id = 3, Name = "Equipe" };

                team1.Characters.Add(character1);
                team1.Characters.Add(character2);

                team2.Characters.Add(character3);

                context.Add(team1);
                context.Add(team2);
                context.Add(team3);
                context.Add(character4);

                await context.SaveChangesAsync();
            }
        }

        // Called once after each test
        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            using (var context = new EpsicRpgDataContext(_options))
            {
                await context.Database.EnsureDeletedAsync();
                await context.DisposeAsync();
            }
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task GetSingle_ReturnsTeam_WhenIdExists(int teamId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamDb = await teamsRepository.GetSingle(teamId);

            //Assert
            Assert.IsNotNull(teamDb);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1111111)]
        public async Task GetSingle_ReturnsNull_WhenIdExists(int teamId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamDb = await teamsRepository.GetSingle(teamId);

            //Assert
            Assert.IsNull(teamDb);
        }

        [TestMethod]
        public async Task GetSingle_ReturnsTeamWithCharacters_WhenIdExists() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamDb = await teamsRepository.GetSingle(1);

            //Assert
            Assert.AreEqual(2, teamDb.Characters.Count());
        }

        [TestMethod]
        public async Task GetSingle_ReturnsTeamWithEmptyCharacters_WhenIdExists() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamDb = await teamsRepository.GetSingle(3);

            //Assert
            Assert.AreEqual(0, teamDb.Characters.Count());
        }

        [TestMethod]
        public async Task Update_UpdatesTeam_WhenItExists() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            var newTeamName = "TeamUpdateTest";
            var teamid = 1;
            var updateTeam = new UpdateTeamDto { Name =  newTeamName };

            //Act
            var teamDb = await teamsRepository.UpdateAsync(teamid, updateTeam);

            //Assert
            Assert.AreEqual(newTeamName, (await teamsRepository.GetSingle(teamid)).Name);
        }

        [TestMethod]
        public async Task Update_ThrowsException_WhenTeamDoesntExists() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            var newTeamName = "TeamUpdateTest";
            var teamid = -1;
            var updateTeam = new UpdateTeamDto { Name =  newTeamName };

            //Act => Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await teamsRepository.UpdateAsync(teamid, updateTeam));
        }

        [TestMethod]
        public async Task Create_CreatesTeam_WhenNameIsCorrect() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            var newTeam = new CreateTeamDto { Name = "TeamTestCreate" };

            //Act
            var teamDb = await teamsRepository.CreateAsync(newTeam);

            //Assert
            Assert.AreEqual(newTeam.Name, (await teamsRepository.GetSingle(teamDb.Id)).Name);
        }
        
        [TestMethod]
        public async Task Create_ThrowsException_WhenCreateTeamDtoIsNull() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await teamsRepository.CreateAsync(null));
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow(null)]
        public async Task Search_ReturnsFullList_WhenSearchIsNullOrEmpty(string search) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.Search(search);

            //Assert
            Assert.AreEqual(3, teamsDb.Count());
        }

        [DataTestMethod]
        [DataRow("T")]
        [DataRow("Te")]
        [DataRow("ea")]
        [DataRow("m")]
        public async Task Search_ReturnsExpectedElements_WhenSearchExists(string search) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.Search(search);

            //Assert
            Assert.AreEqual(2, teamsDb.Count());
        }

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow("TeamX")]
        [DataRow("team")]
        public async Task Search_ReturnsEmptyList_WhenSearchDoesntExists(string search) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.Search(search);

            //Assert
            Assert.AreEqual(0, teamsDb.Count());
        }

        [TestMethod]
        public async Task Delete_RemovesTeams_WhenTeamHasntAnyMembers() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.Delete(3);

            //Assert
            Assert.IsFalse(await teamsRepository.ExistsById(3));
        }

        [TestMethod]
        public async Task Delete_ThrowsException_WhenTeamHasMembers() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<DbUpdateException>(async () => await teamsRepository.Delete(1));
        }

        [DataTestMethod]
        [DataRow(-1, 1)]
        [DataRow(0, 1)]
        [DataRow(9999, 1)]
        public async Task AddCharacterToTeam_ThrowsException_WhenTeamDoesntExists(int teamId, int characterId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await teamsRepository.AddCharacterToTeam(teamId, characterId));
        }

        [DataTestMethod]
        [DataRow(1, -1)]
        [DataRow(1, 0)]
        [DataRow(1, 9999)]
        public async Task AddCharacterToTeam_ThrowsException_WhenCharacterDoesntExists(int teamId, int characterId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await teamsRepository.AddCharacterToTeam(teamId, characterId));
        }

        [TestMethod]
        public async Task AddCharacterToTeam_DoesNothing_WhenCharacterIsAlreadyInTheTeam() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.AddCharacterToTeam(1, 1);

            //Assert
            Assert.IsTrue((await teamsRepository.GetSingle(1)).Characters.Any(c => c.Id == 1));
        }

        [TestMethod]
        public async Task AddCharacterToTeam_AddToTeam_WhenCharacterIsNotInTheTeam() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.AddCharacterToTeam(1, 4);

            //Assert
            Assert.IsTrue((await teamsRepository.GetSingle(1)).Characters.Any(c => c.Id == 4));
        }

        [TestMethod]
        public async Task AddCharacterToTeam_AddToTeam_WhenCharIsAlreadyInAnotherTeam() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.AddCharacterToTeam(1, 3);

            //Assert
            Assert.IsTrue((await teamsRepository.GetSingle(1)).Characters.Any(c => c.Id == 1));
        }

        [DataTestMethod]
        [DataRow(-1, 1)]
        [DataRow(0, 1)]
        [DataRow(9999, 1)]
        public async Task RemoveCharacterFromTeam_ThrowsException_WhenTeamDoesntExists(int teamId, int characterId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await teamsRepository.RemoveCharacterFromTeam(teamId, characterId));
        }

        [DataTestMethod]
        [DataRow(1, -1)]
        [DataRow(1, 0)]
        [DataRow(1, 9999)]
        public async Task RemoveCharacterFromTeam_ThrowsException_WhenCharacterDoesntExists(int teamId, int characterId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await teamsRepository.RemoveCharacterFromTeam(teamId, characterId));
        }

        [TestMethod]
        public async Task RemoveCharacterFromTeam_ThrowException_WhenCharacterIsNotInTheTeam() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act => Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await teamsRepository.RemoveCharacterFromTeam(1, 4));
        }

        [TestMethod]
        public async Task RemoveCharacterFromTeam_RemovesCharacterFromTheTeam_WhenCharacterIsInTheTeam() 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));

            //Act
            var teamsDb = await teamsRepository.RemoveCharacterFromTeam(1, 1);

            //Assert
            Assert.IsFalse((await teamsRepository.GetSingle(1)).Characters.Any(c => c.Id == 1));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task ExistsById_ReturnsTrue_WhenIdExists(int teamId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            
            //Act
            var result = await teamsRepository.ExistsById(teamId);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(99999)]
        public async Task ExistsById_ReturnsFalse_WhenIdDontExists(int teamId) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            
            //Act
            var result = await teamsRepository.ExistsById(teamId);

            //Assert
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow("Team1")]
        [DataRow("Team2")]
        [DataRow("Equipe")]
        public async Task ExistsByName_ReturnsTrue_WhenNameExists(string name) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            
            //Act
            var result = await teamsRepository.ExistsByName(name);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("Team11")]
        [DataRow("Team")]
        [DataRow("team1")]
        public async Task ExistsByName_ReturnsFalse_WhenNameDontExists(string name) 
        {
            //Arrange
            var teamsRepository = new TeamsRepository(new EpsicRpgDataContext(_options));
            
            //Act
            var result = await teamsRepository.ExistsByName(name);

            //Assert
            Assert.IsFalse(result);
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection($"Filename=:memory:");

            connection.Open();
            
            return connection;
        }
    }
}