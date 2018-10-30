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
			await Clients.All.SendAsync("MoveMessage", GameLogic.Move(playerId, x, y));
		}

		public async Task TakeCommand(Guid playerId, Guid objectId)
		{
			var res = GameLogic.Take(playerId, objectId);
			if(res != null)
				await Clients.All.SendAsync("TakeMessage", res);
		}

		public async Task NewPlayerCommand(Guid playerId)
		{
			var res = GameLogic.Link(playerId, Context.ConnectionId);
			await Clients.All.SendAsync("ObjectAdded", res);
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			Clients.All.SendAsync("ObjectRemoved", GameLogic.Unlink(Context.ConnectionId));
			return base.OnDisconnectedAsync(exception);
		}
	}
}
