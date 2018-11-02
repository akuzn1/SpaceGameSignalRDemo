using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
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
					.WithUrl("http://localhost:51582/ServiceHub")
					.Build();
				connection.StartAsync().Wait();

				while (true)
				{
				using (DataContext context = new DataContext(dbPath, createNew))
				{
					for (int level = 1; level <= Constants.LevelCount; level++)
					{
						AddAsteroids(connection, context, level);

						var moveObjects = context.SpaceObjects.Where(p => p.Speed >= 1 && p.SpaceLevel == level && p.Visible).ToList();
						if (moveObjects.Count() != 0)
						{
							foreach (var item in moveObjects)
							{
								Console.WriteLine("1. ({0}, {1}) - ({2}, {3}), {4}, {5}", item.X, item.Y, item.TargetX, item.TargetY, item.Speed, item.Id);
							}
							UpdatePositions(moveObjects);
							JumpShips(connection, context, level, moveObjects);

							foreach (var item in moveObjects)
							{
								Console.WriteLine("2. ({0}, {1}) - ({2}, {3}), {4}, {5}", item.X, item.Y, item.TargetX, item.TargetY, item.Speed, item.Id);
							}
							try
							{
								context.SaveChanges();
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.Message);
								continue;
							}
							connection.InvokeAsync("MoveObjects", level, moveObjects.Where(p => p.SpaceLevel == level));
						}
					}
				}
					Thread.Sleep(50);
				}
			}

		private static void JumpShips(HubConnection connection, DataContext context, int level, List<SpaceObject> moveObjects)
		{
			var portals = context.SpaceObjects.Where(p => p.Type == ObjectType.SpacePortal && p.SpaceLevel == level);
			if (portals != null)
			{
				foreach (var portal in portals)
				{
					foreach (var item in moveObjects)
					{
						if (item.Type == ObjectType.SpacePortal)
							continue;
						var D = Math.Sqrt((portal.X - item.X) * (portal.X - item.X) + (portal.Y - item.Y) * (portal.Y - item.Y));
						if (D < 50 && item.SpaceLevel < Constants.LevelCount)
						{
							item.SpaceLevel += 1;
							item.TargetX = item.X;
							item.TargetY = item.Y;
							item.Speed = 0;
							context.SaveChanges();

							var player = context.Players.Include(p => p.Ship).FirstOrDefault(p => p.ShipId == item.Id);
							player.SpaceLevel += 1;

							connection.InvokeAsync("PlayerJump", player);
						}
					}
				}
			}
		}

		private static void AddAsteroids(HubConnection connection, DataContext context, int level)
		{
			var asteroidsCount = context.SpaceObjects.Count(p => p.Visible && p.SpaceLevel == level && (p.Type == ObjectType.Asteroid1 || p.Type == ObjectType.Asteroid2 || p.Type == ObjectType.Asteroid3));
			if (asteroidsCount < Constants.MinAsteroidsPerLevel)
			{
				var newObjects = new List<SpaceObject>();
				for (int j = 0; j < rnd.Next(5); j++)
				{
					var asteroidType = GetAsteroidType();
					var asteroid = new SpaceObject()
					{
						Id = Guid.NewGuid(),
						X = Constants.ObjectSize * rnd.Next(Constants.MapWidth / Constants.ObjectSize) + Constants.ObjectSize / 2,
						Y = Constants.ObjectSize * rnd.Next(Constants.MapWidth / Constants.ObjectSize) + Constants.ObjectSize / 2,
						Width = Constants.ObjectSize,
						Height = Constants.ObjectSize,
						SpaceLevel = level,
						Type = asteroidType,
						Life = GetAsteroidLife(asteroidType),
						Visible = true,
					};
					context.SpaceObjects.Add(asteroid);
					newObjects.Add(asteroid);
				}
				context.SaveChanges();
				connection.InvokeAsync("AddObjects", level, newObjects);
			}
		}

		private static int GetAsteroidLife(ObjectType type)
		{
			switch (type)
			{
				case ObjectType.Asteroid1: return 10;
				case ObjectType.Asteroid2: return 20;
				case ObjectType.Asteroid3: return 30;
				default: throw new ArgumentException("Invalid asteroid type");
			}
		}

		private static ObjectType GetAsteroidType()
		{
			int typeId = rnd.Next(3);
			switch(typeId)
			{
				case 0: return ObjectType.Asteroid1;
				case 1: return ObjectType.Asteroid2;
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