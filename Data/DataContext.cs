using ASP_111.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_111.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User>    Users    { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic>   Topics   { get; set; }
        public DbSet<Theme>   Themes   { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Visit>   Visits   { get; set; }
        public DbSet<Rate>    Rates    { get; set; }


        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("asp111");

            modelBuilder.Entity<Rate>()     // Указываем что в таблице Rate
                .HasKey(                    // используется композитный
                    nameof(Rate.ItemId),    // ключ по двум полям
                    nameof(Rate.UserId));   // 

            // настройка связей и навигационных свойств
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Author)
                .WithMany()
                .HasForeignKey(s => s.AuthorId);
        }
    }
}
