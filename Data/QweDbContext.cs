using RouteModel = fleet_tracking.Models.Route;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using fleet_tracking.Models;
using Microsoft.AspNetCore.Identity;

namespace fleet_tracking.Data
{
    public class QweDbContext : IdentityDbContext<ApplicationUser>
    {
        public QweDbContext(DbContextOptions<QweDbContext> options) : base(options) { }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<RouteModel> Routes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<SecurityTeam> SecurityTeams { get; set; }
    }

}
