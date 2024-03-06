using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class SysDataContext : DbContext
    {
        public SysDataContext(DbContextOptions<SysDataContext> options) : base(options)
        {
        }
        public DbSet<book> book { get; set; }
        public DbSet<borrower> borrower { get; set; }
    }
}
