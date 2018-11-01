using Microsoft.AspNetCore.SignalR;
using SpaceGameDataModel;
using SpaceGameSignalRDemo.Logic;
using SpaceGameSignalRDemo.Model;
using SpaceGameSignalRDemo.Model.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceGameSignalRDemo.Hubs
{
	public class AdminHub : Hub
	{
		private readonly IHubContext<GameHub> _gameHubContext;
		public AdminHub(IHubContext<GameHub> gameHubContext)
		{
			_gameHubContext = gameHubContext;
		}

		public async Task AddObjects(int level, List<SpaceObject> objects)
		{
			await _gameHubContext.Clients.Group("Level" + level).SendAsync("ObjectAdded", objects);
		}

		public async Task MoveObjects(int level, List<SpaceObject> objects)
		{
			await _gameHubContext.Clients.Group("Level" + level).SendAsync("ObjectUpdated", objects);
		}

		public async Task PlayerJump(Player player)
		{
			var ship = GameLogic.GetPlayerShipById(player.Id);
			await _gameHubContext.Clients.GroupExcept("Level" + (player.SpaceLevel - 1), new List<string>() { player.ConnectionId })
				.SendAsync("ObjectRemoved", new PlayerInfo(player));

			await _gameHubContext.Clients.Client(player.ConnectionId).SendAsync("Jump");
		}
	}
}
