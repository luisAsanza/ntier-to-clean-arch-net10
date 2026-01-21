using CleanCRUDSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanCRUDSolution.Infrastructure.Persistence.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            // Set table name
            builder.ToTable("Persons");

            // Primary Key
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("PersonId");

            // PROPERTIES
            // ==========

            // TODO: PersonName column has to be changed to not nullable to match Entity
            // TODO: Should I rename it? Maybe not, there is not real benefit. Does doing it could be risky?
            builder.Property(p => p.Name)
                   .HasColumnName("PersonName")
                   //.IsRequired(false)
                   .HasMaxLength(80);

            // Email
            builder.Property(p => p.Email)
                .HasMaxLength(80);

            // Date Of Birth
            builder.Property(p => p.DateOfBirth);

            // Gender
            builder.Property(p => p.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Address
            builder.Property(p => p.Address)
                .HasMaxLength(400);

            // ReceiveNewsLetters
            builder.Property(p => p.ReceiveNewsLetters)
                .IsRequired();

            // TaxIdentificationNumber
            builder.Property(p => p.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .IsUnicode(false)
                .HasDefaultValue("ABC12345")
                .HasMaxLength(8);

            // RELATIONSHIPS
            // ===========
            builder.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .Metadata.DependentToPrincipal!.SetPropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
        }
    }
}
