﻿using SpaceGameSignalRDemo.Model;
using System;
using System.Linq;

namespace SpaceGameSignalRDemo.Logic
{
	public class GameLogic
	{
		public static GameState StartGame(string playerName)
		{
			GameState state = new GameState();
			using (var context = new DataContext())
			{
				Player player = context.Players.FirstOrDefault(p => p.Login == playerName);
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
						TargetX = 0,
						TargetY = 0,
						Level = player.Level,
						Visible = true,
						Life = 100,
						Speed = 5,
						Type = ObjectType.Ship1
					};
					context.Players.Add(player);
				}
				else
				{
					player.Ship.Visible = true;
				}
				context.SaveChanges();

				var all = context.SpaceObjects;
				var objects = context.SpaceObjects.Where(p => p.Level == player.Level && p.Visible).ToList();

				state.Player = player;
				state.SpaceObjects = objects;

				return state;
			}
		}
	}
}
