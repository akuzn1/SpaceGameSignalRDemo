using Microsoft.AspNetCore.SignalR;
using SpaceGameSignalRDemo.Logic;
using System;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Hubs
{
	public class GameHub : Hub
	{
		public async Task MoveCommand(Guid playerId, int x, int y)
		{
			var message = GameLogic.Move(playerId, x, y);
			await Clients.Group("Level" + message.Level).SendAsync("MoveMessage", message);
		}

		public async Task TakeCommand(Guid playerId, Guid objectId)
		{
			var res = GameLogic.Take(playerId, objectId);
			if(res != null)
				await Clients.Group("Level" + res.TakedBy.SpaceLevel).SendAsync("TakeMessage", res);
		}

		public async Task NewPlayerCommand(Guid playerId)
		{
			var player = GameLogic.GetPlayerById(playerId);
			if (player != null)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, "Level" + player.SpaceLevel);
				GameLogic.Link(playerId, Context.ConnectionId);
			}
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			var info = GameLogic.Unlink(Context.ConnectionId);
			Clients.Group("Level" + info.Id).SendAsync("ObjectRemoved", info);
			Groups.RemoveFromGroupAsync(Context.ConnectionId, "Level" + info.Id);
			return base.OnDisconnectedAsync(exception);
		}
	}
}
