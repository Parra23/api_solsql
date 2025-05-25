using api_solsql.Models;
using Microsoft.EntityFrameworkCore;

namespace api_solsql.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {
        }
        public DbSet<vw_user> VW_users { get; set; }
        public DbSet<LoginRequest> LoginRequests { get; set; }
        public DbSet<Departments> departments { get; set; }
        public DbSet<Cities> cities { get; set; }
        public DbSet<PlaceTypes> placeTypes { get; set; }
        public DbSet<Photos> photos { get; set; }
        public DbSet<Comments> comments { get; set; }
        public DbSet<CommentsPlace> commentsPlace { get; set; }
        public DbSet<Places> places { get; set; }
        public DbSet<Favorites> favorites { get; set; }
        public DbSet<Reactions> reactions { get; set; }

        public DbSet<PlaceDetail> placeDetail { get; set; }
        public DbSet<vw_general_lugar> vw_general_lugar { get; set; }
        public DbSet<vw_logs_register> vw_logs_registers { get; set; }
        public DbSet<vw_totalcomments_date> vw_totalcomments_dates { get; set; }
        public DbSet<vw_totalcomments_users> vw_totalcomments_users { get; set; }
        public DbSet<vw_logs> vw_log { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vw_logs_register>().HasNoKey().ToView(null); ;
            modelBuilder.Entity<vw_totalcomments_date>().HasNoKey().ToView(null); ;
            modelBuilder.Entity<vw_totalcomments_users>().HasNoKey().ToView(null); ;
            modelBuilder.Entity<CommentsPlace>().HasNoKey().ToView(null);
            modelBuilder.Entity<vw_general_lugar>().HasNoKey().ToView(null);
            modelBuilder.Entity<PlaceDetail>().HasNoKey().ToView(null);
            base.OnModelCreating(modelBuilder);
        }
    }

}
