using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Models
{
    public class VegaDbContext: DbContext
    {
        private IConfiguration configuration;

        public DbSet<Brand> Brands { get; set;  }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleFeature> VehicleFeatures { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public VegaDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            // this.configuration.LazyLoadingEnabled = false; 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VehicleFeature>().HasKey(vf => new { vf.VehicleId, vf.FeatureId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>> DbContext.OnConfiguring()! ConnectionString: ", configuration["ConnectionStrings:Default"]);
            optionsbuilder.UseSqlServer(configuration["ConnectionStrings:Default"]); //  ("Server=LEOP-HPOMEN\\SQLEXPRESS;Database=VegaDB;Trusted_Connection=True");  


        }
        

    }


}