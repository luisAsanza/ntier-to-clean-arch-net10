using CleanCRUDSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanCRUDSolution.Infrastructure.Persistence.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Set table name
            builder.ToTable("Countries");

            // Set primary key
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("CountryId");

            // Set properties: Name was configured as varchar(max) previously
            builder.Property(c => c.Name)
                .HasColumnName("CountryName");
        }
    }
}
