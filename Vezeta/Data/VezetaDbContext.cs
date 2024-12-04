using Final_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Vezeta.Data
{
	public class VezetaDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=DESKTOP-B8FEL5F;Database=Vezeta_app;Trusted_Connection=True;TrustServerCertificate=True");
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<AppointmentRequest> AppointmentRequests { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Further configuration like indexes, constraints, etc.
			modelBuilder.Entity<AppointmentRequest>()
				.HasOne(ar => ar.Doctor)
				.WithMany()
				.HasForeignKey(ar => ar.DoctorId);

			modelBuilder.Entity<AppointmentRequest>()
				.HasOne(ar => ar.Patient)
				.WithMany()
				.HasForeignKey(ar => ar.PatientId);
		}
	}
}
