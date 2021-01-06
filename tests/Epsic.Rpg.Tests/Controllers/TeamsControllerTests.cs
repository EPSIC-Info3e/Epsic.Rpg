using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Epsic.Rpg.Tests.Controllers
{
    [TestClass]
    public class TeamsControllerTests  : ApiControllerTestBase
    {
        [TestMethod, TestCategory("Ex11")]
        public async Task CreateTeamNameValidatorSuccess()
        {
            // Arrange
            var teamDto = new CreateTeamDto { Name = "Info3e Test" };

            // Act
            var response = await PostAsync<CreateTeamDto, RfcError>("/teams", teamDto);

            // Assert
            Assert.IsNull(response.Errors);
        }

        [TestMethod, TestCategory("Ex11")]
        public async Task CreateTeamNameValidatorKo()
        {
            // Arrange
            var teamDto = new CreateTeamDto { Name = "Info4e Test" };

            // Act
            var response = await PostAsync<CreateTeamDto, RfcError>("/teams", teamDto);

            // Assert
            Assert.AreEqual(1, response.Errors.Name.Count());
            Assert.AreEqual(400, response.Status);
            Assert.AreEqual("One or more validation errors occurred.", response.Title);
        }
    }
}