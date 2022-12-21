using Microsoft.EntityFrameworkCore;

namespace RethinkSample.Services.DataAccess;

public class PatientContext : DbContext
{
    public virtual DbSet<Patient> Patients { get; set; }

    public PatientContext(DbContextOptions<PatientContext> options) 
        : base(options)
    {
    }

    public PatientContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasKey(x => x.Id);
        modelBuilder.Entity<Patient>().Property(x => x.FirstName).HasMaxLength(255).HasColumnType("varchar(255)").IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.LastName).HasMaxLength(255).HasColumnType("varchar(255)").IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.Birthday).HasColumnType("date").IsRequired();
        modelBuilder.Entity<Patient>().Property(x => x.Gender).HasMaxLength(1).HasColumnType("varchar(1)").IsRequired();
        modelBuilder.Entity<Patient>().ToTable(nameof(Patient));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>().HaveConversion<DateOnlyConverter>().HaveColumnType("date");
    }
}