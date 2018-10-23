using System;
using System.Linq;

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

				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 100, Y = 100, Width = 64, Height = 64, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid1 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 100, Y = 200, Width = 64, Height = 64, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid2 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 200, Y = 100, Width = 64, Height = 64, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid2 });
				context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 200, Y = 200, Width = 64, Height = 64, Level = 1, Visible = true, Life = 100, Type = ObjectType.Asteroid1 });

				context.SaveChanges();
			}
		}
	}
}
