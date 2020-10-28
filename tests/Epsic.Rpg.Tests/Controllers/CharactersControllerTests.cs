using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg;
using Epsic.Rpg.Controllers;
using Epsic.Rpg.Models;
using Microsoft.AspNetCore.Mvc.Testing;
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
            var response = await GetAsync<IList<Character>>("/characters/getall");

            // Assert
            Assert.AreEqual(3, response.Count);
        }

        [TestMethod, TestCategory("Ex1")]
        [DataRow("P", 2)]
        [DataRow("Pierre", 1)]
        [DataRow("cques", 1)]
        public async Task CharactersPersonnagesNom_Ok(string nom, int expectedCount)
        {
            // Act
            var response = await GetAsync<IList<Character>>($"/personnages?nom={nom}");

            // Assert
            Assert.AreEqual(expectedCount, response.Count);
        }

        [TestMethod, TestCategory("Ex1")]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task CharactersGetSinleId_Ok(int id)
        {
            // Act
            var response = await GetAsync<Character>($"/characters/getsingle/{id}");

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

        [TestMethod, TestCategory("Ex2")]
        public async Task CharactersPut()
        {
            // Arrange
            var name = "TestPlayer";
            var id = 3;
            var character = new Character { Id = id, Name = name, HitPoints = 6 };

            // Act
            var response = await PutAsync<Character>("/characters", character);

            // Assert
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual(name, response.Name);
            Assert.AreEqual(6, response.HitPoints);
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
