using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Epsic.Rpg.Models;
using Epsic.Rpg.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Epsic.Rpg.Exceptions;

namespace Epsic.Rpg.Tests.Services
{
    [TestClass, TestCategory("Ex8")]
    public class TeamsServiceTests
    {
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-0)]
        [DataRow(-098765)]
        public async Task GetSingle_ThrowsArgumentOutOfRangeException_WhenIdIsLowerThan1(int teamId)
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryMock());

            //Act => Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await sut.GetSingle(teamId));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(098765)]
        public async Task GetSingle_ReturnTeamWithId_WhenIdIsGreaterThan0(int teamId)
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryMock());

            //Act
            var result = await sut.GetSingle(teamId);

            //Assert
            Assert.AreEqual(teamId, result.Id);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-0)]
        [DataRow(-098765)]
        public async Task UpdateAsync_ThrowsArgumentOutOfRangeException_WhenIdIsLowerThan1(int teamId)
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryMock());

            //Act => Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await sut.UpdateAsync(teamId, null));
        }

        [TestMethod]
        public async Task UpdateAsync_ThrowsArgumentNullException_WhenTeamToUpdateIsNull()
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryMock());

            //Act => Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await sut.UpdateAsync(1, null));
        }

        [DataTestMethod]
        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [DataRow("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task UpdateAsync_ThrowsArgumentOutOfRangeException_WhenNameIsGreatherThan32(string name)
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryMock());
            var team = new UpdateTeamDto { Name = name };

            //Act => Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await sut.UpdateAsync(1, team));
        }

        [TestMethod]
        public async Task UpdateAsync_ThrowsDataNotFoundException_WhenTeamIdIsNotFound()
        {
            //Arrange
            var sut = new TeamsService(new TeamsRepositoryExistsFalseMock());
            var team = new UpdateTeamDto { Name = "name" };

            //Act => Assert
            await Assert.ThrowsExceptionAsync<DataNotFoundException>(async () => await sut.UpdateAsync(1, team));
        }

        //.... et ainsi de suite.
    }
}