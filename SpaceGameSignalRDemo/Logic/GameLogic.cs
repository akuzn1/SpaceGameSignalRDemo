using Microsoft.EntityFrameworkCore;
using SpaceGameDataModel;
using SpaceGameSignalRDemo.Model;
using SpaceGameSignalRDemo.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SpaceGameSignalRDemo.Logic
{
	public class GameLogic
	{
		public static GameState StartGame(string playerName)
		{
			GameState state = new GameState();
			using (var context = new DataContext())
			{
				var player = context.Players.FirstOrDefault(p => p.Login == playerName);
				if (player == null)
				{
					player = new Player() {
						Id = Guid.NewGuid(),
						Login = playerName,
						Level = 1,
						Expirience = 0,
					};
					player.Ship = new SpaceObject()
					{
						Id = Guid.NewGuid(),
						X = 250,
						Y = 250,
						Width = 128,
						Height = 128,
						TargetX = 0,
						TargetY = 0,
						Level = player.Level,
						Visible = false,
						Life = 100,
						Speed = 0,
						Type = ObjectType.Ship1
					};
					context.Players.Add(player);
				}
				else
				{
					player.Ship.Visible = false;
				}
				context.SaveChanges();

				var all = context.SpaceObjects;
				var objects = context.SpaceObjects.Where(p => p.Level == player.Level && p.Visible).ToList();

				state.Player = new PlayerInfo(player);
				state.SpaceObjects = objects;

				return state;
			}
		}

		public static IEnumerable<SpaceObject> Link(Guid playerId, string connectionId)
		{
			using (var context = new DataContext())
			{
				var player = context.Players.Include(p => p.Ship).FirstOrDefault(p => p.Id == playerId);
				if (player == null)
					throw new ArgumentException(string.Format("Player with Id {0} was not found", playerId));

				player.ConnectionId = connectionId;
				player.Active = true;
				player.Ship.Visible = true;
				context.SaveChanges();

				return new List<SpaceObject>() { player.Ship };
			}
		}

		public static PlayerInfo Unlink(string connectionId)
		{
			using (var context = new DataContext())
			{
				var player = context.Players.Include(p => p.Ship).FirstOrDefault(p => p.ConnectionId == connectionId);
				if (player == null)
					return null;

				var info = new PlayerInfo(player);

				player.Ship.Visible = false;
				player.Active = false;
				player.ConnectionId = string.Empty;
				context.SaveChanges();

				return info;
			}
		}

		internal static TakeMessage Take(Guid playerId, Guid objectId)
		{
			using (var context = new DataContext())
			{
				var player = context.Players.Include(p => p.Ship).FirstOrDefault(p => p.Id == playerId);
				if (player == null)
					throw new ArgumentException(string.Format("Player with Id {0} was not found", playerId));

				var objs = context.SpaceObjects.Select(p => p.Id);
				var asteroid = context.SpaceObjects.FirstOrDefault(p => p.Id == objectId);
				if (asteroid == null)
					return null;

				if (asteroid.Type != ObjectType.Asteroid1 && asteroid.Type != ObjectType.Asteroid2 && asteroid.Type != ObjectType.Asteroid3)
					return null;

				player.Expirience += asteroid.Life;
				if (player.Expirience >= Constants.NewLevel && player.Ship.Type == ObjectType.Ship1)
					player.Ship.Type = ObjectType.Ship2;

				var message = new TakeMessage()
				{
					RemovedObjectId = asteroid.Id,
					TakedBy = player.Ship,
				};

				context.SpaceObjects.Remove(asteroid);

				context.SaveChanges();

				return message;
			}
		}

		public static MoveMessage Move(Guid playerId, int targetX, int targetY)
		{
			if(targetX < 0 || targetX > 2000 || targetY < 0 || targetY > 2000)
				throw new ArgumentException("Target position is out of bounds");

			using (var context = new DataContext())
			{
				var player = context.Players.Include(p => p.Ship).FirstOrDefault(p => p.Id == playerId && p.Active);
				if (player == null)
					throw new ArgumentException(string.Format("Player with Id {0} was not found", playerId));

				var ship = player.Ship;
				ship.TargetX = targetX;
				ship.TargetY = targetY;
				ship.Speed = 5 + ship.Level * 2;
				context.SaveChanges();

				var message = new MoveMessage()
				{
					ShipId = ship.Id,
					X = ship.X,
					Y = ship.Y,
					TargetX = ship.TargetX,
					TargetY = ship.TargetY,
					Speed = ship.Speed,
				};

				return message;
			}
		}
	}
}
