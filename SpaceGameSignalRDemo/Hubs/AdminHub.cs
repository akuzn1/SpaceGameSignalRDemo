using Microsoft.AspNetCore.SignalR;
using SpaceGameDataModel;
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

		public async Task AddObjects(List<SpaceObject> objects)
		{
			await _gameHubContext.Clients.All.SendAsync("ObjectAdded", objects);
		}

		public async Task MoveObjects(List<SpaceObject> objects)
		{
			await _gameHubContext.Clients.All.SendAsync("ObjectUpdated", objects);
		}
	}
}
