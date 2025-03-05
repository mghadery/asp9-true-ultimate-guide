using Microsoft.EntityFrameworkCore;
using People.Entities;

namespace People.Persistent;

public class AppDbContext : DbContext
{
    //public AppDbContext()
    //{
        
    //}
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {        
    }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>()
            .HasKey(c => c.CountryId);
        modelBuilder.Entity<Country>()
            .HasAlternateKey(c => c.CountryName);

        modelBuilder.Entity<Country>()
            .Property(c => c.CountryName)
            .HasMaxLength(100);

        modelBuilder.Entity<Person>()
            .HasKey(p => p.PersonId);
        
        modelBuilder.Entity<Person>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Person>()
            .ToTable(tb => tb.HasCheckConstraint("CK_Person_Email", "Email LIKE '%_@_%._%'"));

        modelBuilder.Entity<Person>()
            .Property(p => p.PersonName)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Person>()
            .Property(p => p.Address)
            .HasMaxLength(500);
        modelBuilder.Entity<Person>()
            .Property(p => p.Gender)
            .HasColumnType("varchar(10)");
        modelBuilder.Entity<Person>()
            .Property(p => p.Email)
            .HasColumnType("varchar(100)")
            .IsRequired();

        //as ap practice as we have already navigation properties
        //modelBuilder.Entity<Person>()
        //    .HasOne(p => p.Country)
        //    .WithMany(c => c.Persons)
        //    .HasForeignKey(p => p.CountryId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
