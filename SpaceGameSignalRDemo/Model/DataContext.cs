using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Model
{
	public class DataContext : DbContext
	{
		private static bool _created = false;
		public DataContext()
		{
			if (!_created)
			{
				_created = true;
				Database.EnsureDeleted();
				Database.EnsureCreated();
			}
		}

		string dbPath = ".\\data.db";

		public DbSet<SpaceObject> SpaceObjects { get; set; }
		public DbSet<Player> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source="+dbPath);
		}
	}
}
