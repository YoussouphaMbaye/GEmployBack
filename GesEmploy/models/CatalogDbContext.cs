using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gemploy.models
{
    public class CatalogDbContext:DbContext
    {
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public CatalogDbContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employer>().Property(d=>d.NameEmp).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Employer>().Property(d => d.Occupation).IsRequired().HasMaxLength(60);
            modelBuilder.Entity<Employer>().Property(c => c.BirthDay).HasColumnType("date");
            //.Entity<Employer>().Property(p => p.BirthDay).HasConversion<DateOnlyConverter>().HaveColumnType("date");

            modelBuilder.Entity<Login>().Property(p=>p.UserName).IsRequired().HasMaxLength(60);
            modelBuilder.Entity<Login>().Property(p => p.PassWord).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Login>().HasOne<Employer>(l=>l.Employ).WithOne(e=>e.Login).HasForeignKey<Login>().OnDelete(DeleteBehavior.Cascade);
           
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
