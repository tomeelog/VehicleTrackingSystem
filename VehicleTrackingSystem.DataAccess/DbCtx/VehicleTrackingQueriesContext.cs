using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VehicleTrackingSystem.Entities;

namespace VehicleTrackingSystem.DataAccess.DbCtx
{
    public class VehicleTrackingQueriesContext : DbContext
    {
        public VehicleTrackingQueriesContext(DbContextOptions<VehicleTrackingQueriesContext> options) : base(options)
        {
        }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Location> Location { get; set; }
    }
}
