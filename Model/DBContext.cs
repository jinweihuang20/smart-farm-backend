using Microsoft.EntityFrameworkCore;
namespace Smart_farm.Model
{
    public class DBContext : DbContext
    {
        public DbSet<RecordData> recordDatas { get; set; }

        private IConfiguration _config;

        public DBContext(IConfiguration config)
        {
            this._config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = _config.GetSection("ConnectionStrings").GetValue("heroku", "");
            optionsBuilder.UseNpgsql(connectionStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
            modelBuilder.Entity<RecordData>().HasKey(p => p.datetime);
            modelBuilder.Entity<RecordData>().Property(p => p.datetime).HasColumnType("timestamp without time zone");
        }
    }
}
