using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SpaceGameDataModel;
using SpaceGameSignalRDemo.Hubs;
using SpaceGameSignalRDemo.Logic;
using SpaceGameSignalRDemo.Model;
using System.Collections.Generic;

namespace SpaceGameSignalRDemo.Controllers
{
	[Route("api/game")]
	[ApiController]
	//public class GameController : ControllerBase
	//{
	//	[HttpPost]
	//	public ActionResult<GameState> Index([FromBody] string name)
	//	{
	//		return GameLogic.StartGame(name);
	//	}
	//}


	public class GameController : ControllerBase
	{
		private readonly IHubContext<GameHub> _gameHubContext;

		public GameController(IHubContext<GameHub> gameHubContext)
		{
			_gameHubContext = gameHubContext;
		}

		[HttpPost]
		public ActionResult<GameState> Index([FromBody] string name)
		{
			GameState state = GameLogic.StartGame(name);			
			return state;
		}
	}
}