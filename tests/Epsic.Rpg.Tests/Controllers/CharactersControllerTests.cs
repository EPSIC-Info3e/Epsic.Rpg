using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Enums;
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
        [DataRow(4, "TestPlayer", 10)]
        public async Task CharactersCreate(int id, string name, int hitPoints)
        {
            // Arrange
            var character = new Character { Id = id, Name = name, HitPoints = hitPoints };
            
            // Act
            var response = await PostAsync<Character>("/characters", character);

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(hitPoints, response.HitPoints);
        }

        [TestMethod, TestCategory("Ex2")]
        [DataRow(3, "TestPlayer", RpgClass.Cleric)]
        public async Task CharactersUpdate(int id, string name, RpgClass rpgClass)
        {
            // Arrange
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

        [TestMethod, TestCategory("Ex3")]
        [DataRow(6, "TestPlayer", 10)]
        public async Task IntegrationTestCreate(int id, string name, int hitPoints)
        {
            //Arrange
            await CharactersCreate(id, name, hitPoints);

            // Act
            var response = await GetAsync<Character>($"/characters/{id}");

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(hitPoints, response.HitPoints);
        }

        [TestMethod, TestCategory("Ex3")]
        [DataRow(6, "TestPlayer2", RpgClass.Cleric)]
        public async Task IntegrationTestUpdate(int id, string name, RpgClass rpgClass)
        {
            //Arrange
            await CharactersCreate(id, name, 10);
            await CharactersUpdate(id, name, rpgClass);

            // Act
            var response = await GetAsync<Character>($"/characters/{id}");

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(rpgClass, response.Class);
        }
    }
}
