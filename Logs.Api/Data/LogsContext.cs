using Microsoft.EntityFrameworkCore;
using Logs.Api.Models;

namespace Logs.Api.Data
{
    public class LogsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public LogsContext(DbContextOptions options) : base(options)
        {

        }
    }
}
