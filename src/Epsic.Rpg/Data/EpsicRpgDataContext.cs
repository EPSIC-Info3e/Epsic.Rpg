using Epsic.Rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace Epsic.Rpg.Data
{
    public class EpsicRpgDataContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        public EpsicRpgDataContext(DbContextOptions<EpsicRpgDataContext> options)
            : base(options) 
        {
        
        }
    }
}