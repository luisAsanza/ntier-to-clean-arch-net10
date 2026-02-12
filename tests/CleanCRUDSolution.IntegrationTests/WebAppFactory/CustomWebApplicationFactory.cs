using System.Data.Common;
using CleanCRUDSolution.Domain.Entities;
using CleanCRUDSolution.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanCRUDSolution.IntegrationTests.WebAppFactory
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private const string _testingEnv = "Testing";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment(_testingEnv);

            builder.UseSetting("ConnectionStrings:DefaultConnection", "InMemory_Dummy_Value");

            builder.ConfigureServices(services =>
            {
                // Remove the app's real Db Context objects registration
                services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                services.RemoveAll<ApplicationDbContext>();
                services.RemoveAll<IDbContextFactory<ApplicationDbContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();

                var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
                keepAliveConnection.Open();
                services.AddSingleton<DbConnection>(keepAliveConnection);

                services.AddDbContext<ApplicationDbContext>((sp, options) =>
                {
                    options.UseSqlite(sp.GetRequiredService<DbConnection>());
                    options.UseSeeding(SeedPersonsDbForTesting);
                });

                services.AddTransient<IStartupFilter, SeedDataFilter>();
            });            
        }

        public class SeedDataFilter : IStartupFilter
        {
            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                return app =>
                {
                    using var scope = app.ApplicationServices.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();

                    next(app);
                };
            }
        }

        #region Helpers for seeding the in-memory database with test data

        private void SeedPersonsDbForTesting(DbContext context, bool storeOpPerformed)
        {
            if(!storeOpPerformed) return;

            var db = (ApplicationDbContext)context;

            //Seed Countries if not seeded yet
            if(!db.Countries.Any())
            {
                var country1 = new Country("Aland");
                var country2 = new Country("Belgium");

                db.Countries.AddRange(country1, country2);
                db.SaveChanges();
            }

            //Seed Persons if not seeded yet
            if(!db.Persons.Any())
            {
                var country = db.Countries.First();

                var person1 = new Person("John");
                person1.UpdateEmail("john@example.com");
                person1.UpdateDateOfBirth(new DateOnly(1990, 1, 1), DateOnly.FromDateTime(DateTime.Now));
                person1.UpdateGender(Domain.Enums.GenderOptions.Male);
                person1.UpdateReceiveNewsLetters(true);
                person1.MoveTo(country.Id, "Some Street 1");
                person1.UpdateTin("12345123");

                var person2 = new Person("Jane");
                person2.UpdateEmail("jane@example.com");
                person2.UpdateDateOfBirth(new DateOnly(1992, 2, 2), DateOnly.FromDateTime(DateTime.Now));
                person2.UpdateGender(Domain.Enums.GenderOptions.Female);
                person2.UpdateReceiveNewsLetters(false);
                person2.MoveTo(country.Id, "Another Street 2");
                person2.UpdateTin("67890123");

                db.Persons.AddRange(person1, person2);
                db.SaveChanges();

                var otherCountry = db.Countries.Skip(1).First();

                var person3 = new Person("Bob");
                person3.UpdateEmail("bob@example.com");
                person3.UpdateDateOfBirth(new DateOnly(1985, 5, 5), DateOnly.FromDateTime(DateTime.Now));
                person3.UpdateGender(Domain.Enums.GenderOptions.Male);
                person3.UpdateReceiveNewsLetters(true);
                person3.MoveTo(otherCountry.Id, "Different Street 3");
                person3.UpdateTin("54321123");

                db.Persons.Add(person3);
                db.SaveChanges();

                var person4 = new Person("Alice");
                person4.UpdateEmail("alice@example.com");
                person4.UpdateDateOfBirth(new DateOnly(1994, 4, 4), DateOnly.FromDateTime(DateTime.Now));
                person4.UpdateReceiveNewsLetters(true);
                person4.UpdateTin("11111123");

                var person5 = new Person("Charlie");
                person5.UpdateEmail("charlie@example.com");
                person5.UpdateGender(Domain.Enums.GenderOptions.Male);
                person5.UpdateReceiveNewsLetters(false);

                var person6 = new Person("Eve");
                person6.UpdateEmail("eve@example.com");
                person6.UpdateDateOfBirth(new DateOnly(2000, 12, 12), DateOnly.FromDateTime(DateTime.Now));
                person6.UpdateGender(Domain.Enums.GenderOptions.Female);
                person6.UpdateReceiveNewsLetters(true);

                db.Persons.AddRange(person4, person5, person6);
                db.SaveChanges();
            }
        }

        #endregion

    }
}