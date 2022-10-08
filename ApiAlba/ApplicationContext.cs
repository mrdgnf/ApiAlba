using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
    {
        public DbSet<News> NEWS { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydb.db");
        }
    }








