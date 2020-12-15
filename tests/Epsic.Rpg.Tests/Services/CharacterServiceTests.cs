using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Epsic.Rpg.Data;
using Epsic.Rpg.Enums;
using Epsic.Rpg.Models;
using Epsic.Rpg.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Epsic.Rpg.Tests.Services
{
    [TestClass]
    public class CharacterServiceTests
    {
        [TestMethod, TestCategory("Ex8")]
        public async Task TeamTestAsync() 
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EpsicRpgDataContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            using (var context = new EpsicRpgDataContext(options))
            {
                await context.Database.EnsureCreatedAsync();
            }

            //Act 
            using (var context = new EpsicRpgDataContext(options))
            {
                var personnage = new Character { Id = 9999 };
                personnage.Team = new Team { Id = 1, Name = "Lol" };
                context.Characters.Add(personnage);
                context.SaveChanges();
            }

            //Assert
            using (var context = new EpsicRpgDataContext(options))
            {
                var personnage2 = context.Characters.Include(c => c.Team).FirstOrDefault(c => c.Id == 9999);
                Assert.AreEqual(1, personnage2.Team.Id);
            }
        }

        [TestMethod, TestCategory("Ex8")]
        public async Task TeamMembersTestAsync() 
        {
            //Arrange
            var options = new DbContextOptionsBuilder<EpsicRpgDataContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            using (var context = new EpsicRpgDataContext(options))
            {
                await context.Database.EnsureCreatedAsync();
            }

            var p1 = new Character { Name = "P1" };
            var p2 = new Character { Name = "P2" };
            var p3 = new Character { Name = "P3" };
            var p4 = new Character { Name = "P4" };

            //Act 
            using (var context = new EpsicRpgDataContext(options))
            {
                var team1 = new Team { Name = "TestTeam 1"};
                var team2 = new Team { Name = "TestTeam 2"};

                p1.Team = team1;
                p2.Team = team1;
                p3.Team = team1;
                p4.Team = team2;

                context.Characters.Add(p1);
                context.Characters.Add(p2);
                context.Characters.Add(p3);
                context.Characters.Add(p4);
                context.SaveChanges();
            }

            //Assert
            using (var context = new EpsicRpgDataContext(options))
            {
                var personnage2 = context.Characters.Find(9999);
                Assert.AreEqual(p1.Team.Id, context.Characters.Include(c => c.Team).FirstOrDefault(c => c.Id == p1.Id).Team.Id);
                Assert.AreEqual(p2.Team.Id, context.Characters.Include(c => c.Team).FirstOrDefault(c => c.Id == p2.Id).Team.Id);
                Assert.AreEqual(p3.Team.Id, context.Characters.Include(c => c.Team).FirstOrDefault(c => c.Id == p3.Id).Team.Id);
                Assert.AreEqual(p4.Team.Id, context.Characters.Include(c => c.Team).FirstOrDefault(c => c.Id == p4.Id).Team.Id);
            }
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection($"Filename=:memory:");

            connection.Open();
            
            return connection;
        }
    }
}