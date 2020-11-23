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
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);
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
            await CharactersCreate(id, "lolilol", 10);
            await CharactersUpdate(id, name, rpgClass);

            // Act
            var response = await GetAsync<Character>($"/characters/{id}");

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(rpgClass, response.Class);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(-999999)]
        [DataRow(-1)]
        [DataRow(-0)]
        public async Task GetSingleIdPlusPetitQueUn(int id)
        {
            // Act
            var response = await GetAsync($"/characters/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(123)]
        [DataRow(1234)]
        [DataRow(5)]
        public async Task GetSingleIdExistePas(int id)
        {
            // Act
            var response = await GetAsync($"/characters/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow("adasdasdadasdasdadadadadadadadasdasdasda")]
        [DataRow("111111111111111111111111111111111")]
        public async Task UpdateNamePlusGrandQue32(string name)
        {
            //Arrange
            var model = new CharacterPatchViewModel { Name = name };

            // Act
            var response = await PostBasicAsync($"/characters/{1}", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(-999999)]
        [DataRow(-1)]
        [DataRow(-0)]
        public async Task UpdateIdPlusPetitQueUn(int id)
        {
            //Arrange
            var model = new CharacterPatchViewModel();

            // Act
            var response = await PostBasicAsync($"/characters/{id}", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(123)]
        [DataRow(1234)]
        [DataRow(5)]
        public async Task UpdateIdExistePas(int id)
        {
            //Arrange
            var model = new CharacterPatchViewModel { Name = "TestUpdate" };

            // Act
            var response = await PostBasicAsync($"/characters/{id}", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow("Pierre")]
        [DataRow("Paul")]
        [DataRow("Jacques")]
        public async Task UpdateNameExisteDeja(string name)
        {
            //Arrange
            var model = new CharacterPatchViewModel { Name = name };

            // Act
            var response = await PostBasicAsync($"/characters/{1}", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(-999999)]
        [DataRow(-1)]
        [DataRow(-0)]
        public async Task AddIdPlusPetitQueUn(int id)
        {
            //Arrange
            var model = new Character { Name = "TestUpdate", Id = id };

            // Act
            var response = await PostBasicAsync($"/characters/", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow("Pierre")]
        [DataRow("Paul")]
        [DataRow("Jacques")]
        public async Task AddNameExisteDeja(string name)
        {
            //Arrange
            var model = new Character { Name = name, Id = 6 };

            // Act
            var response = await PostBasicAsync($"/characters", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task AddIdExisteDeja(int id)
        {
            //Arrange
            var model = new Character { Name = "TestAdd", Id = id };

            // Act
            var response = await PostBasicAsync($"/characters", model);

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task DeleteDejaDelete(int id)
        {
            // Act
            var response = await DeleteAsync($"/characters/{id}");
            var response2 = await DeleteAsync($"/characters/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response2.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(5)]
        public async Task DeleteInexistant(int id)
        {
            // Act
            var response = await DeleteAsync($"/characters/{id}");
            var response2 = await DeleteAsync($"/characters/{id}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response.StatusCode);
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent, (int)response2.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("ab")]
        [DataRow("abc")]
        public async Task SearchplusPetitQue3(string name)
        {
            // Act
            var response = await GetAsync($"/personnages?name={name}");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }

        [TestMethod, TestCategory("Ex5")]
        public async Task Teapot()
        {
            // Act
            var response = await GetAsync($"/personnages?name=teapot");

            // Assert
            Assert.AreEqual(Microsoft.AspNetCore.Http.StatusCodes.Status418ImATeapot, (int)response.StatusCode);
        }
    }
}
