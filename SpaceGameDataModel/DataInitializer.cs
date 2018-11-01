using System;
using System.Linq;

namespace SpaceGameDataModel
{
	public static class DataInitializer
	{
		public static void Initialize(DataContext context)
		{
			context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 10, Y = 10, Width = 300, Height = 300, SpaceLevel = 1, Visible = true, Life = 100, Type = ObjectType.SpacePortal });
			//context.SpaceObjects.Add(new SpaceObject() { Id = Guid.NewGuid(), X = 10, Y = 10, Width = 300, Height = 300, SpaceLevel = 2, Visible = true, Life = 100, Type = ObjectType.SpacePortal });
		}
	}
}
