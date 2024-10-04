using MedManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedManager.Data;

public class ApplicationDbContext: IdentityDbContext<Doctor>
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Medicament> Medicaments => Set<Medicament>();
    public DbSet<MedicalHistory> MedicalHistories => Set<MedicalHistory>();
    public DbSet<Allergy> Allergies => Set<Allergy>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Prescriptions)
            .WithOne(p => p.Patient);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Allergies)
            .WithMany(a => a.Patients);
        
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.MedicalHistories)
            .WithMany(m => m.Patients);

        modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Prescriptions)
            .WithOne(p => p.Doctor)
            .HasForeignKey(p => p.DoctorId)
            .HasPrincipalKey(d => d.DoctorId);

        modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Patients)
            .WithOne(p => p.Doctor)
            .HasForeignKey(p => p.DoctorId)
            .HasPrincipalKey(d => d.DoctorId);

        modelBuilder.Entity<Prescription>()
            .HasMany(p => p.Medicaments)
            .WithMany(m => m.Prescriptions);

        modelBuilder.Entity<Medicament>()
            .HasMany(m => m.Allergies)
            .WithMany(a => a.Medicaments);

        modelBuilder.Entity<Medicament>()
            .HasMany(m => m.MedicalHistories)
            .WithMany(m => m.Medicaments);
        
        base.OnModelCreating(modelBuilder);
    }
}