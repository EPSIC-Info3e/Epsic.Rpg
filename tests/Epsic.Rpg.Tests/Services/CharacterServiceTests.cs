using System.Collections.Generic;
using System.Threading.Tasks;
using Epsic.Rpg.Enums;
using Epsic.Rpg.Models;
using Epsic.Rpg.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Epsic.Rpg.Tests.Services
{
    [TestClass]
    public class CharacterServiceTests
    {
        [TestMethod, TestCategory("Ex4")]
        public void GetAllTest()
        {
            //Arrange
            var service = new CharacterService();

            // Act
            var response = service.GetAll();

            // Assert
            Assert.AreEqual(3, response.Count);
        }
    }
}