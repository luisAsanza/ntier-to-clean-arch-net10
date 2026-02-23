using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanCRUDSolution.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //Apply Provider-specific configurations
            var personEntity = modelBuilder.Entity<Person>();

            if (Database.IsSqlServer())
            {
                personEntity.ToTable("Persons", t =>
                {
                    t.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");
                });
            }
            else if (Database.IsSqlite())
            {
                personEntity.ToTable("Persons", t =>
                {
                    t.HasCheckConstraint("CHK_TIN", "length([TaxIdentificationNumber]) = 8");
                });
            }
        }

        public IQueryable<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]");
        }
    }
}
