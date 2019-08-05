using Microsoft.EntityFrameworkCore;
using System;
using ClientModel = ClientBasicCrud.Model.Client;

namespace ClientBasicCrud.Repository.Client
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<ClientModel> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientModel>().ToTable("client");

            modelBuilder.Entity<ClientModel>().HasData(
                new ClientModel
                {
                    Id = 1,
                    Name = "Victor Soares",
                    Birthdate = new DateTime(1994, 08, 10),
                    RegistrationDate = new DateTime(2019, 07, 31)
                },
                new ClientModel
                {
                    Id = 2,
                    Name = "Maria Camargo",
                    Birthdate = new DateTime(1980, 02, 13),
                    RegistrationDate = new DateTime(2019, 07, 25)
                }
            );
        }
    }
}