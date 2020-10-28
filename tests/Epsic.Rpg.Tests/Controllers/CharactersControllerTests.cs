using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epsic.Rpg.Tests.Controllers
{
    [TestClass]
    public class CharactersControllerTests  : ApiControllerTestBase
    {
        [TestMethod, TestCategory("Ex1")]
        public async Task CharactersGetAll()
        {
            // Act
            var response = await GetAsync<IList<Character>>("/characters");

            // Assert
            Assert.AreEqual(3, response.Count);
        }

        [TestMethod, TestCategory("Ex1")]
        [DataRow("P", 2)]
        [DataRow("Pierre", 1)]
        [DataRow("cques", 1)]
        public async Task CharactersPersonnagesNom_Ok(string name, int expectedCount)
        {
            // Act
            var response = await GetAsync<IList<Character>>($"/personnages?name={name}");

            // Assert
            Assert.AreEqual(expectedCount, response.Count);
        }

        [TestMethod, TestCategory("Ex1")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task CharactersGetSingleId_Ok(int id)
        {
            // Act
            var response = await GetAsync<Character>($"/characters/{id}");

            // Assert
            Assert.AreEqual(id, response.Id);
        }

        [TestMethod, TestCategory("Ex2")]
        public async Task CharactersCreate()
        {
            // Arrange
            var id = 4;
            var name = "TestPlayer";
            var hitPoints = 4;
            var character = new Character { Id = id, Name = name, HitPoints = hitPoints };
            
            // Act
            var response = await PostAsync<Character>("/characters", character);

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(hitPoints, response.HitPoints);
        }

        public async Task CharactersUpdate()
        {
            // Arrange
            var name = "TestPlayer";
            var id = 3;
            var rpgClass = Enums.RpgClass.Cleric;
            var character = new CharacterPatchViewModel { Name = name, Class = rpgClass };

            // Act
            var response = await PostAsync<CharacterPatchViewModel, Character>($"/characters/{id}", character);

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(rpgClass, response.Class);
        }

        [TestMethod, TestCategory("Ex2")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task CharactersDelete(int id)
        {
            // Act
            var response = await DeleteAsync($"/characters/{id}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
