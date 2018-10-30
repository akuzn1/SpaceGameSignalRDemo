using Microsoft.AspNetCore.SignalR.Client;
using SpaceGameDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SpaceGameProcessor
{
	class Program
	{
		static Random rnd;
		static void Main(string[] args)
		{
			rnd = new Random();
			var dbPath = "..\\..\\..\\..\\data.db";
			var res = File.Exists(dbPath);

			if (args.Length != 0)
				dbPath = args[0];

			bool createNew = false;
			if (args.Length == 2)
				createNew = bool.Parse(args[1]);

				HubConnection connection;
				connection = new HubConnectionBuilder()
					.WithUrl("http://localhost:51582/AdminHub")
					.Build();
				connection.StartAsync().Wait();

				while (true)
				{
				using (DataContext context = new DataContext(dbPath, createNew))
				{
					for (int i = 1; i <= Constants.LevelCount; i++)
					{
						var asteroidsCount = context.SpaceObjects.Count(p => p.Visible && p.Level == i && (p.Type == ObjectType.Asteroid1 || p.Type == ObjectType.Asteroid2 || p.Type == ObjectType.Asteroid3));
						if (asteroidsCount < Constants.MinAsteroidsPerLevel)
						{
							var newObjects = new List<SpaceObject>();
							for (int j = 0; j < rnd.Next(5); j++)
							{
								var asteroid = new SpaceObject()
								{
									Id = Guid.NewGuid(),
									X = Constants.ObjectSize * rnd.Next(Constants.MapWidth / Constants.ObjectSize) + Constants.ObjectSize / 2,
									Y = Constants.ObjectSize * rnd.Next(Constants.MapWidth / Constants.ObjectSize) + Constants.ObjectSize / 2,
									Width = Constants.ObjectSize,
									Height = Constants.ObjectSize,
									Level = i,
									Visible = true,
									Life = 100,
									Type = GetAsteroidType(),
								};
								context.SpaceObjects.Add(asteroid);
								newObjects.Add(asteroid);
							}
							context.SaveChanges();
							connection.InvokeAsync("AddObjects", newObjects);
						}
						var moveObjects = context.SpaceObjects.Where(p => p.Speed >= 1 && p.Level == i && p.Visible).ToList();
						if (moveObjects.Count() != 0)
						{
							foreach (var item in moveObjects)
							{
								Console.WriteLine("1. ({0}, {1}) - ({2}, {3}), {4}, {5}", item.X, item.Y, item.TargetX, item.TargetY, item.Speed, item.Id);
							}
							UpdatePositions(moveObjects);
							foreach (var item in moveObjects)
							{
								Console.WriteLine("2. ({0}, {1}) - ({2}, {3}), {4}, {5}", item.X, item.Y, item.TargetX, item.TargetY, item.Speed, item.Id);
							}
							context.SaveChanges();
							connection.InvokeAsync("MoveObjects", moveObjects);
						}
					}
				}
					Thread.Sleep(50);
				}
			}
		

		private static ObjectType GetAsteroidType()
		{
			int typeId = rnd.Next(3);
			switch(typeId)
			{
				case 0: return ObjectType.Asteroid1;
				case 1: return ObjectType.Asteroid1;
				default: return ObjectType.Asteroid3;
			}
		}

		private static void UpdatePositions(List<SpaceObject> moveObjects)
		{
			foreach (var spaceObject in moveObjects)
			{
				var D = Math.Sqrt((spaceObject.TargetX - spaceObject.X) * (spaceObject.TargetX - spaceObject.X) + (spaceObject.TargetY - spaceObject.Y) * (spaceObject.TargetY - spaceObject.Y));
				var koef = spaceObject.Speed / D;
				if (koef < 1)
				{
					spaceObject.X += (int)((spaceObject.TargetX - spaceObject.X) * koef);
					spaceObject.Y += (int)((spaceObject.TargetY - spaceObject.Y) * koef);
				}
				else
				{
					spaceObject.X = spaceObject.TargetX;
					spaceObject.Y = spaceObject.TargetY;
					spaceObject.Speed = 0;
				}
			}
		}
	}
}