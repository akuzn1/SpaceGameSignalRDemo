using Microsoft.EntityFrameworkCore;

namespace SpaceGameDataModel
{
	public class DataContext : DbContext
	{
		public DataContext()
		{
		}

		public DataContext(string path, bool createNew = false)
		{
			dbPath = path;
			if (createNew)
			{
				Database.EnsureDeleted();
				Database.EnsureCreated();
				//DataInitializer.Initialize(dbPath);
			}
		}

		string dbPath = "..\\data.db";

		public DbSet<SpaceObject> SpaceObjects { get; set; }
		public DbSet<Player> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source="+dbPath);
		}
	}
}
