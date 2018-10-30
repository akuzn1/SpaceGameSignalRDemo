using SpaceGameDataModel;
using System.Collections.Generic;

namespace SpaceGameSignalRDemo.Model
{
	public class GameState
	{
		public PlayerInfo Player { get; internal set; }

		public List<SpaceObject> SpaceObjects { get; internal set; }
	}
}
