using GesEmploy.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Gemploy.models
{
    public class CatalogDbContext: IdentityDbContext<IdentityUser>
    {
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Horaire> Horaire { get; set; }
        public DbSet<DayOff> DayOffs { get; set; }
        public DbSet<GetIn> GetIn { get; set; }
        public DbSet<GetOut> GetOut { get; set; }
        public DbSet<DayOfDay> DayOfDay { get; set; }
        public DbSet<DayOffEmployer> DayOffEmployer { get; set; }
        public DbSet<PublicHolidays> PublicHolidays { get; set; }
        public CatalogDbContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateOnly>()
                                .HaveConversion<DateOnlyEFConverter>()
                                .HaveColumnType("date");

            configurationBuilder.Properties<TimeOnly>()
                      .HaveConversion<TimeOnlyEFConverter>()
                      .HaveColumnType("time");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //timeOnly conversion
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employer>().Property(d=>d.NameEmp).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Employer>().Property(d => d.Occupation).IsRequired().HasMaxLength(60);
            modelBuilder.Entity<Employer>().Property(c => c.BirthDay).HasColumnType("date");
            //.Entity<Employer>().Property(p => p.BirthDay).HasConversion<DateOnlyConverter>().HaveColumnType("date");

            modelBuilder.Entity<Login>().Property(p=>p.UserName).IsRequired().HasMaxLength(60);
            modelBuilder.Entity<Login>().Property(p => p.PassWord).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Login>().HasOne<Employer>(l=>l.Employ).WithOne(e=>e.Login).HasForeignKey<Login>().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Horaire>().HasMany(h => h.Employers).WithOne(e => e.Horaire).HasForeignKey(e => e.HoraireId);
            modelBuilder.Entity<DayOffEmployer>().HasKey(i => new { i.IdEmp, i.IdDayOff });
            modelBuilder.Entity<Employer>().HasMany(g => g.getIns).WithOne(e => e.Employer).HasForeignKey(e => e.EmployerId);
            modelBuilder.Entity<Employer>().HasMany(g => g.getOuts).WithOne(e => e.Employer).HasForeignKey(e => e.EmployerId);
            modelBuilder.Entity<Employer>().HasMany(g => g.DayOfDays).WithOne(e => e.Employer).HasForeignKey(e => e.EmployerId);
            modelBuilder.Entity<Employer>().HasMany(g => g.DayOffEmployer).WithOne(e => e.Employer);
            modelBuilder.Entity<DayOff>().HasMany(g => g.DayOffEmployer).WithOne(e => e.DayOff);
        }

        
    }
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }
}
