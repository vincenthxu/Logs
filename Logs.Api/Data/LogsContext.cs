using Microsoft.EntityFrameworkCore;
using Logs.Api.Models;

namespace Logs.Api.Data
{
    public class LogsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public LogsContext(DbContextOptions<LogsContext> options) : base(options)
        {

        }
    }
}
