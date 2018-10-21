using Microsoft.AspNetCore.Mvc;
using SpaceGameSignalRDemo.Logic;
using SpaceGameSignalRDemo.Model;

namespace SpaceGameSignalRDemo.Controllers
{
	[Route("api/game")]
	[ApiController]
	public class GameController : ControllerBase
	{
		[HttpPost]
		public ActionResult<GameState> Index([FromBody] string name)
		{
			return GameLogic.StartGame(name);
		}
	}
}