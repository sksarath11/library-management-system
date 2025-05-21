using libarary.Models;
using Microsoft.EntityFrameworkCore;

namespace libarary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<IssueRecord> IssueRecords { get; set; } = null!;
        public DbSet<UserBook> UserBooks { get; set; } = null!;

        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<IQuery> IQuery { get; set; }
        public DbSet<Loan> Loans { get; set; }   // Add Loans DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId });

            modelBuilder.Entity<IQuery>()
                .HasKey(q => q.QueryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
