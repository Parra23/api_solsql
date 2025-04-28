using api_solsql.Models;
using Microsoft.EntityFrameworkCore;

namespace api_solsql.Context {
    public class ContextDB : DbContext{
        public ContextDB(DbContextOptions<ContextDB> options) : base(options) {
        }
        public DbSet<users> Users { get; set; }
    }
}
