using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Model
{
	public static class DataInitializer
	{
		public static void Initialize()
		{
			using (var context = new DataContext())
			{
				if(context.SpaceObjects.Any())
				{
					return;
				}

				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 100, Y = 100, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid1 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 100, Y = 200, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid2 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 200, Y = 100, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid2 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 200, Y = 200, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid1 });

				context.SaveChanges();
			}
		}
	}
}
